using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateCheckAnswer : BaseState
{
    public Roads NowRoad { set => _nowRoad = value; }

    [SerializeField] private StateMachineLevel stateMachine;
    [SerializeField] private StateShowQuestion stateShow;
    [Space]
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI rightAnswerText;
    [SerializeField] private TextMeshProUGUI explanationText;
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private TextMeshProUGUI goodResult;
    [SerializeField] private float timeForShowResult;
    [Space]
    [SerializeField] private ObstacleController obstacle;
    [SerializeField] private Animator animatorPlayer;

    private Roads _nowRoad;
    private QuestionModel _questionModel;
    private Roads _trueRoad;
    private bool _isRightAnswer;
    private float _timer;

    public override void Enter(QuestionModel questionModel, Roads trueRoad)
    {
        _questionModel = questionModel;
        _trueRoad = trueRoad;

        if (_trueRoad == _nowRoad)
            RightAnswer();
        else
            FalseAnswer();
    }

    public override void LogicUpdate()
    {
        if (_isRightAnswer)
        {
            if (_timer <= Time.time)
                EndCheck();
        }

    }

    public override void End()
    {
        if (!_isRightAnswer)
        {
            window.SetActive(false);
            closeButton.onClick.RemoveListener(EndCheck);
        }
        else
            goodResult.text = "";
    }


    private void RightAnswer()
    {
        _timer = Time.time + timeForShowResult;
        StartCoroutine(obstacle.RightEnd(timeForShowResult));
        animatorPlayer.Play("05_Jump_Cat_Copy");
        _isRightAnswer = true;
        goodResult.text = "�������";
        obstacle.OpenFence(_trueRoad);
    }

    private void FalseAnswer()
    {
        _isRightAnswer = false;
        window.SetActive(true);
        rightAnswerText.text = _questionModel.TrueAnswer;
        explanationText.text = _questionModel.Explanation;
        closeButton.onClick.AddListener(EndCheck);
        stateMachine.AddMistake();
    }

    private void EndCheck()
    {
        stateMachine.ChangeState(stateShow);
        animatorPlayer.Play("03_Walk_Cat_Copy");
    }
}
