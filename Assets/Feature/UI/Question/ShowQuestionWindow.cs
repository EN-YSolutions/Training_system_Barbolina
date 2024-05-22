using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowQuestionWindow : BaseWindow
{
    public event Action<string> OnDeleteQuestion;

    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI exceptionText;
    [SerializeField] private TextMeshProUGUI trueAnswerText;
    [SerializeField] private TextMeshProUGUI oneFalseAnswerText;
    [SerializeField] private TextMeshProUGUI twoFalseAnswerText;
    [Space]
    [SerializeField] private Button buttonDelete;
    [SerializeField] private Button buttonChange;
    [Space]
    [SerializeField] private ChangeQuestionWindow changeQuestionWindow;

    private QuestionModel _questionModel;

    public void Init(QuestionModel questionModel)
    {
        _questionModel = questionModel;

        gameObject.SetActive(true);
        questionText.text = questionModel.QuestionText;
        exceptionText.text = questionModel.Explanation;
        trueAnswerText.text = questionModel.TrueAnswer;
        oneFalseAnswerText.text = questionModel.OneFalseAnswer;
        twoFalseAnswerText.text = questionModel.TwoFalseAnswer;
    }

    private void Awake()
    {
        buttonChange.onClick.AddListener(ChangeQuestion);
        buttonDelete.onClick.AddListener(DeleteQuestion);
    }

    private void OnDestroy()
    {
        buttonChange.onClick.RemoveListener(ChangeQuestion);
        buttonDelete.onClick.RemoveListener(DeleteQuestion);
    }

    private void DeleteQuestion()
    {
        OnDeleteQuestion.Invoke(_questionModel.Id);
        gameObject.SetActive(false);
    }

    private void ChangeQuestion()
    {
        gameObject.SetActive(false);
        WindowAggregator.Open(changeQuestionWindow);
        changeQuestionWindow.Init(_questionModel);
    }
}
