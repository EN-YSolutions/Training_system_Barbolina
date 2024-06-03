using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HistoryQuestionWindow : BaseWindow
{
    [SerializeField] private TMP_Dropdown courcesDropdown;
    [SerializeField] private Button showButton;
    [SerializeField] private Button exitButton;
    [Space]
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject lineQuestionPrefab;
    [Space]
    [SerializeField] private ShowQuestionWindow showQuestionWindow;
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
        showQuestionWindow.OnDeleteQuestion += DeleteQuestion;
    }

    private void OnEnable()
    {
        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        _idAndTitle = new();
        courcesDropdown.ClearOptions();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }

        if (_lines.Count != 0)
            ClearContent();
    }

    private void OnDestroy()
    {
        showButton.onClick.RemoveListener(ShowQuestions);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
        showQuestionWindow.OnDeleteQuestion -= DeleteQuestion;
    }


    private void ShowQuestions()
    {
        List<QuestionModel> questionModels = DatabaseConnector.AllCoursQuestions(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text]);
        generalStatistics.text = $"Общая статистика по курсу:\n{DatabaseConnector.AveragePassingValue(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text])}%";


        foreach (var model in questionModels)
        {
            _allAnswer = DatabaseConnector.CountAllAnswerQuestion(model.Id);

            if (_allAnswer == 0)
            {
                model.PercentRight = -1;
                continue;
            }
            _allRightAnswer = DatabaseConnector.CountAllRightAnswerQuestion(model.Id);
            if (_allRightAnswer == 0)
            {
                model.PercentRight = 0;

                continue;
            }
            model.PercentRight = (int)((float)_allAnswer / (float)_allRightAnswer * 100f);
        }

        questionModels = questionModels.OrderBy(x => x.PercentRight).ToList();

        if (_lines.Count != 0)
            ClearContent();

        List<TMP_Text> textsLine = new();

        foreach (var model in questionModels)
        {
            var line = Instantiate(lineQuestionPrefab, contentTransform).GetComponent<LineQuestion>();
            line.QuestionText = model.QuestionText;

            if (model.PercentRight != -1)
                line.StatisticText = model.PercentRight.ToString() + "%";
            else
                line.StatisticText = "-";

            _lines.Add(line.gameObject);
            line.ButtonQuestion.onClick.AddListener(() => ShowConcreteQuestion(model));
            textsLine.Add(line.QuestionTMPText);
        }

        sizeController.Init(textsLine);
    }

    private void ShowConcreteQuestion(QuestionModel question)
    {
        showQuestionWindow.Init(question);
    }

    private void ClearContent()
    {
        for (int i = 0; i < _lines.Count; i++)
            Destroy(_lines[i]);
        _lines.Clear();
        sizeController.Clear();
    }

    private void DeleteQuestion(string id)
    {
        DatabaseConnector.DeleteQuestion(id);
        ShowQuestions();
    }
}
