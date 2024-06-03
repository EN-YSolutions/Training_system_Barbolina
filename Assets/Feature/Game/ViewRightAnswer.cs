using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ViewRightAnswer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI explanationText;
    [Space]
    [SerializeField] Button closeButton;

    public void Show(AnswerModel question)
    {
        gameObject.SetActive(true);

        if (question.Type == AnswersType.Question)
        {
            var questionModel = DatabaseConnector.GetQuestion(question.IdQuestion);
            explanationText.text = "\tВопрос: " + questionModel.QuestionText;
            explanationText.text += "\n\tПравильный ответ: " + questionModel.TrueAnswer;
        }
        else
        {
            var term = DatabaseConnector.GetTerm(question.IdQuestion);
            explanationText.text = term.Terminology + " — " + term.Description;
        }

        closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
