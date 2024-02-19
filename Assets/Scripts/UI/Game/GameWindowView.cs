using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWindowView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TextMeshProUGUI leftAnswer;
    [SerializeField] private TextMeshProUGUI rightAnswer;

    public void ShowQuestion(string questionText, string leftAnswerText, string rightAnswerText)
    {
        question.text = questionText;
        leftAnswer.text = leftAnswerText;
        rightAnswer.text = rightAnswerText;
    }

    public void GoodResult()
    {
        question.text = "Молодец!";
        leftAnswer.text = "";
        rightAnswer.text = "";
    }
}
