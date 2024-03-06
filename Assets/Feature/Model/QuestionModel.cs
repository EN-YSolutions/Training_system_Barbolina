using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionModel
{
    public string Id;
    public string QuestionText;
    public string TrueAnswer;
    public string OneFalseAnswer;
    public string TwoFalseAnswer;
    public string Explanation;
    public int Time;
    public int PercentRight;

    public QuestionModel(string id, string questionText, string trueAnswer, string oneFalseAnswer, string twoFalseAnswer, string explanation, int time)
    {
        Id = id;
        QuestionText = questionText;
        TrueAnswer = trueAnswer;
        OneFalseAnswer = oneFalseAnswer;
        TwoFalseAnswer = twoFalseAnswer;
        Explanation = explanation;
        Time = time;
    }

    public QuestionModel() { }
}
