using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistakeModel
{
    public string IdQuestion;
    public string IdUser;
    public string IdRepetitionCource;
    public string IdCource;

    public MistakeModel(string idQuestion, string idUser,
        string idCource)
    {
        IdQuestion = idQuestion;
        IdUser = idUser;
        IdCource = idCource;
    }
}
