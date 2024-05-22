using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectWindow : BaseWindow
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button playWindow;
    [Space]
    [SerializeField] private TextMeshProUGUI adviceText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TMP_Dropdown cources;
    [SerializeField] private TMP_Dropdown playerHeroies;
    [SerializeField] private TMP_InputField quantityQuestion;

    private Dictionary<string, string> _idAndTitle = new();
    private int _maxQuantity = 0;

    private void Awake()
    {
        backButton.onClick.AddListener(ComeBack);
        playWindow.onClick.AddListener(GoPlay);
        cources.onValueChanged.AddListener(ChangeMaxQuantityInput);
        quantityQuestion.onValueChanged.AddListener(CheckInput);
    }

    private void OnEnable()
    {
        ShowAdvice();

        List<string> idCources = DatabaseConnector.AllCoursesUserTakes();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            cources.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
        cources.value = 0;
        cources.Select();
        cources.RefreshShownValue();
        ChangeMaxQuantityInput(0);
    }

    private void OnDisable()
    {
        cources.ClearOptions();
        _idAndTitle.Clear();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(ComeBack);
        playWindow.onClick.RemoveListener(GoPlay);
        quantityQuestion.onValueChanged.RemoveListener(CheckInput);
        cources.onValueChanged.RemoveListener(ChangeMaxQuantityInput);
    }

    private void GoPlay()
    {
        DatabaseConnector.AddCourcesId(_idAndTitle[cources.options[cources.value].text]);
        DatabaseConnector.PlayerHero = playerHeroies.options[playerHeroies.value].text;
        DatabaseConnector.MaxQuantityQuestion = System.Int32.Parse(quantityQuestion.text);
        WindowAggregator.Clear();
        SceneManager.LoadScene(1);
    }

    private void ShowAdvice()
    {
        List<string> needCources = DatabaseConnector.AllNeedCoursesRepetition();

        var temp = "";

        if (needCources.Count != 0)
        {
            temp = "Курсы, которые cтоит повторить: ";
            foreach (var i in needCources)
            {
                temp += DatabaseConnector.TitleCourse(i) + "; ";
            }
        }
        else
        {
            temp += "Всё повторено, вы молодцы!";
        }


        adviceText.text = temp;
    }

    private void CheckInput(string inputValue)
    {
        if (inputValue != "" && System.Int32.Parse(inputValue) > _maxQuantity)
            quantityQuestion.text = _maxQuantity.ToString();
    }


    private void ChangeMaxQuantityInput(int inputValue)
    {
        DatabaseConnector.AddCourcesId(_idAndTitle[cources.options[inputValue].text]);
        _maxQuantity =  DatabaseConnector.AllCoursesQuestionsForRepetition().Count;
        quantityText.text = $"Количество вопросов (максимально {_maxQuantity}): ";
        quantityQuestion.text = _maxQuantity.ToString();
    }

}
