using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public abstract void Enter(QuestionModel questionModel, Roads trueRoad);
    public abstract void LogicUpdate();
    public abstract void End();
}
