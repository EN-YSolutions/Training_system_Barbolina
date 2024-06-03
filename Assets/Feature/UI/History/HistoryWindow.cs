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

    private Dictionary<string, string> _idAndTitle = new();
    private List<string> _canChooseCources;
    private List<string> _needCources;

    private void Awake()
    {
        comeBackButton.onClick.AddListener(ComeBack);
        selectButton.onClick.AddListener(ShowAttach);
    }

    private void OnEnable()
    {
        cources.ClearOptions();
        _idAndTitle.Clear();
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
        cources.RefreshShownValue();

        ShowAdvice();
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
        _needCources = new();

        foreach (var i in _canChooseCources)
        {
            if (!DatabaseConnector.WasCourseRepetitionToday(i))
                _needCources.Add(i);
        }

        var temp = $"Процент правильности ответов в последней попытке: {lastTry.PassProgressPoint}\n" +
            $"Дата последней попытки:{lastTry.Date}\n";

        if(_needCources.Count != 0)
        {
            temp += "Курсы, которые вы давно не повторяли:\n ";

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
}
