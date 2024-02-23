using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        // TODO: Сортировать вопросы по % 

        if (_lines.Count != 0)
            ClearContent();

        foreach (var model in questionModels)
        {
            var line = Instantiate(lineQuestionPrefab, contentTransform).GetComponent<LineQuestion>();
            line.QuestionText = model.QuestionText;
            // TODO: a.StatisticText = ;
            _lines.Add(line.gameObject);
            line.ButtonQuestion.onClick.AddListener(() => ShowConcreteQuestion(model));
        }
    }

    private void ShowConcreteQuestion(QuestionModel question)
    {
        WindowAggregator.Open(showQuestionWindow);
        showQuestionWindow.Init(question);
    }

    private void ClearContent()
    {
        for (int i = 0; i < _lines.Count; i++)
            Destroy(_lines[i]);
        _lines.Clear();
    }
}
