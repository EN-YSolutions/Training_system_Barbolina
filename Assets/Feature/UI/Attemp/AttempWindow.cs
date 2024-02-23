using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttempWindow : BaseWindow
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject attempPrefab;
    [SerializeField] private GameObject mistakePrefab;
    [SerializeField] private Transform content;
    [Space]
    [SerializeField] private ViewRightAnswer rightAnswer;

    private List<RepetitionModel> _repetitionModels = new();
    private List<Button> buttons = new();
    private List<GameObject> lines = new();

    private void Awake()
    {
        backButton.onClick.AddListener(ComeBack);
    }

    private void OnEnable()
    {
        title.text = DatabaseConnector.TitleCourse(DatabaseConnector.IdCources);
        _repetitionModels = DatabaseConnector.AllCoursesRepetition();

        foreach(var model in _repetitionModels)
        {
            var a = Instantiate(attempPrefab, content);
            a.GetComponent<LineAttachView>().Text = $"Дата: {model.Date}; Результат: {model.PercentageCorrectAnswers}%";
            var mistakes = DatabaseConnector.AllRepetitionMistakes(model.Id);
            lines.Add(a);
            foreach (var mistake in mistakes)
            {
                var temp = Instantiate(mistakePrefab, content);
                lines.Add(temp);
                var view = temp.GetComponent<LineMistakeView>();
                view.Text = DatabaseConnector.GetQuestion(mistake).QuestionText;
                view.ClickButton.onClick.AddListener(() => ShowRightAnswer(DatabaseConnector.GetQuestion(mistake)));
                buttons.Add(view.ClickButton);
            }
        }
    }

    private void OnDisable()
    {
        foreach(var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        foreach(var obj in lines)
        {
            Destroy(obj);
        }

        lines.Clear();

        buttons.Clear();
    }


    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(ComeBack);
    }

    private void ShowRightAnswer(QuestionModel question)
    {
        rightAnswer.Show(question);
    }
}
