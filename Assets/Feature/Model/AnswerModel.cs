public class AnswerModel
{
    public string Id;
    public string IdQuestion;
    public string IdRepetitionCource;
    public bool Answer;
    public AnswersType Type;

    public AnswerModel(string id, string idQuestion, string idRepetitionCource, bool answer, AnswersType type)
    {
        Id = id;
        IdQuestion = idQuestion;
        IdRepetitionCource = idRepetitionCource;
        Answer = answer;
        Type = type;
    }

    public AnswerModel(string idQuestion, bool answer)
    {
        IdQuestion = idQuestion;
        Answer = answer;
    }
}

public enum AnswersType
{
    Question,
    Term
}
