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
    [SerializeField] private TMP_Dropdown cources;

    Dictionary<string, string> _idAndTitle = new();

    private void Awake()
    {
        backButton.onClick.AddListener(ComeBack);
        playWindow.onClick.AddListener(GoPlay);
    }

    private void OnEnable()
    {
        cources.ClearOptions();
        _idAndTitle.Clear();

        ShowAdvice();

        List<string> idCources = DatabaseConnector.AllCoursesUserTakes();
        
        foreach(string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            cources.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
    }

    private void OnDestroy()
    {
        backButton.onClick.AddListener(ComeBack);
        playWindow.onClick.AddListener(GoPlay);
    }

    private void GoPlay()
    {
        DatabaseConnector.AddCourcesId(_idAndTitle[cources.options[cources.value].text]);
        WindowAggregator.Clear();
        SceneManager.LoadScene(1);
    }

    private void ShowAdvice()
    {
        
        List<string> needCources = DatabaseConnector.AllNeedCoursesRepetition();

        var temp = "";
 
        if (needCources.Count != 0)
        {
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

}
