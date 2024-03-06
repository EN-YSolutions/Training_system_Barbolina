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
    [SerializeField] private TextMeshProUGUI generalStatistics;
    [SerializeField] private TextAutoSizeController sizeController;

    private Dictionary<string, string> _idAndTitle;
    private List<GameObject> _lines = new();

    private void Awake()
    {
        showButton.onClick.AddListener(ShowQuestions);
        exitButton.onClick.AddListener(WindowAggregator.Close);
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
    }

    private void ShowQuestions()
    {
        List<QuestionModel> questionModels = DatabaseConnector.AllCoursQuestions(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text]);
        generalStatistics.text = $"Общая статистика:\n{DatabaseConnector.AveragePassingValue(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text])}%";
        int allCountRepetion = DatabaseConnector.CountRepetiotion(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text]);

        foreach(var model in questionModels)
        {
            model.PercentRight = (int)((float)(allCountRepetion - DatabaseConnector.CountMistake(model.Id)) / (float)allCountRepetion * 100f);
        }

        questionModels = questionModels.OrderBy(x => x.PercentRight).ToList();

        if (_lines.Count != 0)
            ClearContent();

        List<TMP_Text> textsLine = new();

        foreach (var model in questionModels)
        {
            var line = Instantiate(lineQuestionPrefab, contentTransform).GetComponent<LineQuestion>();
            line.QuestionText = model.QuestionText;
            line.StatisticText = model.PercentRight.ToString();
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
}
