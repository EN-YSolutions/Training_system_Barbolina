public class TermModel
{
    public string Id;
    public string IdCources;
    public string Terminology;
    public string Description;
    public int StartPoint;
    public int Time;
    public int PercentRight;

    public TermModel(string id, string idCource, string terminology, string description, int time, int startPoint)
    {
        Id = id;
        IdCources = idCource;
        Terminology = terminology;
        Description = description;
        StartPoint = startPoint;
        Time = time;
    }

    public TermModel() { }
}
