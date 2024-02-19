using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistoryWindow : BaseWindow
{
    [SerializeField] private AttempWindow attempWindow;
    [Space]
    [SerializeField] private Button comeBackButton;
    [SerializeField] private Button selectButton;
    [Space]
    [SerializeField] private TextMeshProUGUI adviceText;
    [SerializeField] private TMP_Dropdown cources;

    Dictionary<string, string> _idAndTitle = new();

    private void Awake()
    {
        comeBackButton.onClick.AddListener(ComeBack);
        selectButton.onClick.AddListener(ShowAttach);
    }

    private void OnEnable()
    {
        cources.ClearOptions();
        _idAndTitle.Clear();
        ShowAdvice();
        List<string> idCources = DatabaseConnector.AllCoursesUserTakes();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            cources.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
    }

    private void OnDestroy()
    {
        comeBackButton.onClick.RemoveListener(ComeBack);
        selectButton.onClick.RemoveListener(ShowAttach);
    }

    private void ShowAttach()
    {
        DatabaseConnector.AddCourcesId(_idAndTitle[cources.options[cources.value].text]);
        WindowAggregator.Open(attempWindow);
    }

    private void ShowAdvice()
    {
        var lastTry = DatabaseConnector.GetLastRetition();
        List<string> needCources = DatabaseConnector.AllNeedCoursesRepetition();

        var temp = $"Процент правильности ответов в последней попытке: {lastTry.PassProgressPoint}\n" +
            $"Дата последней попытки:{lastTry.Date}\n";

        if(needCources.Count != 0)
        {
            temp += "Курсы, которые вы давно не повторяли: ";

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
