using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttempWindow : BaseWindow
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject attempPrefab;
    [SerializeField] private GameObject answersPrefab;
    [SerializeField] private Transform content;
    [Space]
    [SerializeField] private ViewRightAnswer rightAnswer;
    [SerializeField] private TextAutoSizeController sizeController;

    private List<RepetitionModel> _repetitionModels = new();
    private List<GameObject> _lines = new();
    private List<LineAnswers> _answers = new();
    private List<TMP_Text> _texts = new();

    private void Awake()
    {
        backButton.onClick.AddListener(ComeBack);
    }

    private void OnEnable()
    {
        title.text = DatabaseConnector.TitleCourse(DatabaseConnector.IdCources);
        _repetitionModels = DatabaseConnector.AllCoursesRepetition();
        _repetitionModels.Reverse();

        foreach (var model in _repetitionModels)
        {
            var a = Instantiate(attempPrefab, content);
            a.GetComponent<LineAttachView>().Text = $"Дата: {model.Date}; Результат: {model.PercentageCorrectAnswers}%";
            _lines.Add(a);

            List<AnswerModel> answerModels = DatabaseConnector.AllAnswersQuestion(model.Id);
            answerModels.AddRange(DatabaseConnector.AllAnswersTerm(model.Id));
            var ans = Instantiate(answersPrefab, content).GetComponent<LineAnswers>();
            var texts = ans.Initialized(answerModels);
            ans.IsNeedShow += ShowRightAnswer;
            _answers.Add(ans);

            foreach (var t in texts)
            {
                _texts.Add(t.TextAnswer);
            }
        }
        sizeController.Init(_texts);
    }

    private void OnDisable()
    {
        foreach (var obj in _lines)
        {
            Destroy(obj);
        }
        _lines.Clear();
        sizeController.Clear();
        _texts.Clear();


        foreach (var obj in _answers)
        {
            obj.IsNeedShow -= ShowRightAnswer;
            Destroy(obj.gameObject);
        }
        _answers.Clear();
    }


    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(ComeBack);
    }

    private void ShowRightAnswer(AnswerModel question)
    {
        rightAnswer.Show(question);
    }
}
