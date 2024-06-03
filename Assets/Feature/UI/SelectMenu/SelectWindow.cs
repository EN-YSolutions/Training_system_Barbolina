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
    private int _minQuantity = 3;
    private int _numbText = 0;

    private List<string> _canChooseCources;
    private List<string> _needCources;

    private void Awake()
    {
        backButton.onClick.AddListener(ComeBack);
        playWindow.onClick.AddListener(GoPlay);
        cources.onValueChanged.AddListener(ChangeMaxQuantityInput);
        quantityQuestion.onValueChanged.AddListener(CheckInput);
    }

    private void OnEnable()
    {
        List<string> idCources = DatabaseConnector.AllCoursesUserTakes();
        _canChooseCources = new();

        foreach (string id in idCources)
        {
            DatabaseConnector.AddCourcesId(id);
            if (DatabaseConnector.AllCoursesQuestionsForRepetition(id).Count <= 3)
                continue;

            _canChooseCources.Add(id);
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            cources.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
        


        ShowAdvice();
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
        _needCources = new();

        foreach (var i in _canChooseCources)
        {
            if (!DatabaseConnector.WasCourseRepetitionToday(i))
                _needCources.Add(i);
        }

        var temp = "";

        if (_needCources.Count != 0)
        {
            temp += "Курсы, которые вы давно не повторяли: ";

            foreach (var i in _needCources)
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
        if (inputValue == "" || inputValue == "-")
            return;
        _numbText = System.Int32.Parse(inputValue);
        if (_numbText > _maxQuantity)
            quantityQuestion.text = _maxQuantity.ToString();
        else if (_numbText < _minQuantity)
            quantityQuestion.text = _minQuantity.ToString();   
    }


    private void ChangeMaxQuantityInput(int inputValue)
    {
        if (_idAndTitle.Count == 0)
            return;
        DatabaseConnector.AddCourcesId(_idAndTitle[cources.options[inputValue].text]);
        _maxQuantity =  DatabaseConnector.AllCoursesQuestionsForRepetition().Count;
        quantityText.text = $"Количество вопросов:\n(максимально {_maxQuantity}, минимально {_minQuantity}) ";
        quantityQuestion.text = _maxQuantity.ToString();
    }

}
