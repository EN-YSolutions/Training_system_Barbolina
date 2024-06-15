using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryTermWindow : BaseWindow
{
    [SerializeField] private TMP_Dropdown courcesDropdown;
    [SerializeField] private Button showButton;
    [SerializeField] private Button exitButton;
    [Space]
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject lineQuestionPrefab;
    [Space]
    [SerializeField] private ShowTermWindow showTermWindow;
    [Space]
    [SerializeField] private TextMeshProUGUI generalStatistics;
    [SerializeField] private TextAutoSizeController sizeController;

    private Dictionary<string, string> _idAndTitle;
    private List<GameObject> _lines = new();

    private int _allAnswer;
    private int _allRightAnswer;

    private void Awake()
    {
        showButton.onClick.AddListener(ShowQuestions);
        exitButton.onClick.AddListener(WindowAggregator.Close);
        showTermWindow.OnDeleteQuestion += DeleteTerm;
    }

    private void OnEnable()
    {
        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        _idAndTitle = new();
        courcesDropdown.ClearOptions();
        generalStatistics.text = $"Общая статистика по курсу:\nxxx%";
        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
        courcesDropdown.value = 0;
        courcesDropdown.Select();
        courcesDropdown.RefreshShownValue();

        if (_lines.Count != 0)
            ClearContent();
    }

    private void OnDestroy()
    {
        showButton.onClick.RemoveListener(ShowQuestions);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
        showTermWindow.OnDeleteQuestion -= DeleteTerm;
    }

    private float _allResult;
    private float _allPercentRightResult;

    private void ShowQuestions()
    {
        List<TermModel> termModels = DatabaseConnector.AllCoursTerms(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text]);
        int allCountRepetion = DatabaseConnector.CountRepetiotion(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text]);

        _allResult = 0;
        _allPercentRightResult = 0;

        foreach (var model in termModels)
        {
            _allAnswer = DatabaseConnector.CountAllAnswerTerm(model.Id);
            if (_allAnswer == 0)
            {
                model.PercentRight = -1;
                continue;
            }
            _allResult++;
            _allRightAnswer = DatabaseConnector.CountAllRightAnswerTerm(model.Id);
            if (_allRightAnswer == 0)
            {
                model.PercentRight = 0;
                continue;
            }
            model.PercentRight = (int)((float)_allRightAnswer / (float)_allAnswer * 100f);
            _allPercentRightResult += model.PercentRight;
        }
        generalStatistics.text = $"Общая статистика по курсу:\n{System.Math.Round(_allPercentRightResult / _allResult,2)}%";

        termModels = termModels.OrderBy(x => x.PercentRight).ToList();

        if (_lines.Count != 0)
            ClearContent();

        List<TMP_Text> textsLine = new();

        foreach (var model in termModels)
        {
            var line = Instantiate(lineQuestionPrefab, contentTransform).GetComponent<LineQuestion>();
            line.QuestionText = model.Terminology;

            if (model.PercentRight != -1)
                line.StatisticText = model.PercentRight.ToString();
            else
                line.StatisticText = "-";

            _lines.Add(line.gameObject);
            line.ButtonQuestion.onClick.AddListener(() => ShowConcreteQuestion(model));
            textsLine.Add(line.QuestionTMPText);
        }

        sizeController.Init(textsLine);
    }

    private void ShowConcreteQuestion(TermModel question)
    {
        showTermWindow.Init(question);
    }

    private void ClearContent()
    {
        for (int i = 0; i < _lines.Count; i++)
            Destroy(_lines[i]);
        _lines.Clear();
        sizeController.Clear();
    }

    private void DeleteTerm(string id)
    {
        DatabaseConnector.DeleteTerm(id);
        ShowQuestions();
    }
}
