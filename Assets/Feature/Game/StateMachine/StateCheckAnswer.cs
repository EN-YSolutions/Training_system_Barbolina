using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCheckAnswer : BaseState
{
    [SerializeField] private StateShowQuestion stateShow;
    public override void End()
    {
        throw new System.NotImplementedException();
    }

    public override void Enter(QuestionModel questionModel, Roads trueRoad)
    {
        throw new System.NotImplementedException();
    }

    public override void LogicUpdate()
    {
        throw new System.NotImplementedException();
    }

}
