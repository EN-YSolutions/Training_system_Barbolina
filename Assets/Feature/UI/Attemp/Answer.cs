using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Answer : MonoBehaviour
{
    public event Action<AnswerModel> OnClick = delegate { };

    public TextMeshProUGUI TextAnswer => text;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Button button;
    [SerializeField] private Color rightColor;
    [SerializeField] private Color falseColor;

    private AnswerModel _answerModel;

    public AnswerModel Model
    {
        set
        {
            _answerModel = value;
            if(_answerModel.Type == AnswersType.Question)
            {
                text.text = DatabaseConnector.GetQuestion(_answerModel.IdQuestion).QuestionText;
            }
            else
            {
                text.text = DatabaseConnector.GetTerm(_answerModel.IdQuestion).Terminology;
            }

            imageBackground.color = _answerModel.Answer ? rightColor : falseColor;  
        }
    }

    private void Awake()
    {
        button.onClick.AddListener(Click);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Click);
    }

    private void Click() => OnClick.Invoke(_answerModel);
}
