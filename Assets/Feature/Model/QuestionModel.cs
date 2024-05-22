using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionModel
{
    public string Id;
    public string IdCources;
    public string QuestionText;
    public string TrueAnswer;
    public string OneFalseAnswer;
    public string TwoFalseAnswer;
    public string Explanation;
    public int StartPoint;
    public int Time;
    public int PercentRight;

    public QuestionModel(string id, string idCource, string questionText, string trueAnswer, string oneFalseAnswer, string twoFalseAnswer, string explanation, int time, int startPoint)
    {
        Id = id;
        IdCources = idCource;
        QuestionText = questionText;
        TrueAnswer = trueAnswer;
        OneFalseAnswer = oneFalseAnswer;
        TwoFalseAnswer = twoFalseAnswer;
        Explanation = explanation;
        Time = time;
        StartPoint = startPoint;
    }

    public QuestionModel() { }
}
