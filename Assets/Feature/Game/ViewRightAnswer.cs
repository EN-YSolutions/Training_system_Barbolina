using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ViewRightAnswer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rightAnswerText;
    [SerializeField] TextMeshProUGUI explanationText;
    [Space]
    [SerializeField] Button closeButton;

    public void Show(QuestionModel question)
    {
        gameObject.SetActive(true);
        rightAnswerText.text = question.TrueAnswer;
        explanationText.text = question.Explanation;
        Time.timeScale = 0;
        closeButton.onClick.AddListener(Close);
    }

    private void Close()
    {
        closeButton.onClick.RemoveListener(Close);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
