using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateShowQuestion : BaseState
{
    [SerializeField] private StateRun stateRun;
    [SerializeField] private StateMachineLevel stateMachine;
    [SerializeField] private GameObject window;
    [SerializeField] private Button buttonClose;
    [SerializeField] private TextMeshProUGUI textQuestion;

    public override void Enter(QuestionModel questionModel, Roads trueRoad)
    {
        window.SetActive(true);
        textQuestion.text = questionModel.QuestionText;
        buttonClose.onClick.AddListener(EndShow);
    }

    public override void LogicUpdate()
    {
        
    }

    public override void End()
    {
        buttonClose.onClick.RemoveListener(EndShow);
        window.SetActive(false);
    }

    private void EndShow()
    {
        stateMachine.ChangeState(stateRun);
    }
}
