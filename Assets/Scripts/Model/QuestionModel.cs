using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionModel
{
    public string Id;
    public string QuestionText;
    public string TrueAnswer;
    public string FalseAnswer;
    public string Explanation;
    public int Time;

    public QuestionModel(string id, string questionText, string trueAnswer, string falseAnswer, string explanation, int time)
    {
        Id = id;
        QuestionText = questionText;
        TrueAnswer = trueAnswer;
        FalseAnswer = falseAnswer;
        Explanation = explanation;
        Time = time;
    }

    public QuestionModel() { }
}
