using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowQuestionWindow : BaseWindow
{
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI exceptionText;
    [SerializeField] private TextMeshProUGUI trueAnswerText;
    [SerializeField] private TextMeshProUGUI oneFalseAnswerText;
    [SerializeField] private TextMeshProUGUI twoFalseAnswerText;

    public void Init(QuestionModel questionModel)
    {
        gameObject.SetActive(true);
        questionText.text = questionModel.QuestionText;
        exceptionText.text = questionModel.Explanation;
        trueAnswerText.text = questionModel.TrueAnswer;
        oneFalseAnswerText.text = questionModel.OneFalseAnswer;
        twoFalseAnswerText.text = questionModel.TwoFalseAnswer;
    }
}
