using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateRun : BaseState
{
    [SerializeField] private StateMachineLevel stateMachine;
    [SerializeField] private StateCheckAnswer stateCheck;
    [SerializeField] private ObstacleController obstacle;
    [Space]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI rightRoadAnswer;
    [SerializeField] private TextMeshProUGUI centerRoadAnswer;
    [SerializeField] private TextMeshProUGUI leftRoadAnswer;
    [Space]
    [SerializeField] private Transform leftRoad;
    [SerializeField] private Transform centerRoad;
    [SerializeField] private Transform rightRoad;
    [Space]
    [SerializeField] private int changeSpeed;

    private Animator _animatorPlayer;
    private GameObject _player;
    private QuestionModel _questionModel;
    private Roads _nowRoad;
    private Roads _trueRoad;

    private bool _isStartLevel = true;

    private Dictionary<Roads, Transform> _roadTransform;
    private float _timer = 0;
    private float _timeToChangeRoad = .5f;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animatorPlayer = _player.GetComponent<Animator>();
    }

    public override void Enter(QuestionModel questionModel, Roads trueRoad)
    {
        if (_isStartLevel)
        {
            _isStartLevel = false;
            _nowRoad = Roads.CentralRoad;
            _player.transform.position = centerRoad.position;
            _roadTransform = new Dictionary<Roads, Transform>()
                {
                    { Roads.LeftRoad, leftRoad},
                    { Roads.CentralRoad, centerRoad },
                    { Roads.RightRoad, rightRoad }
                };
        }

        _questionModel = questionModel;
        _trueRoad = trueRoad;
        SetText();
        obstacle.Init(_questionModel.Time);
        obstacle.EndMove += EndRun;
    }

    public override void LogicUpdate()
    {
        if (_timer < Time.time)
            ChangeRoad();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Run();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopRun();
    }

    public override void End()
    {
        questionText.text = "";
        rightRoadAnswer.text = "";
        centerRoadAnswer.text = "";
        leftRoadAnswer.text = "";

        stateCheck.NowRoad = _nowRoad;
        obstacle.EndMove -= EndRun;
    }

    private void ChangeRoad()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            _timer = _timeToChangeRoad + Time.time;
            if (Input.GetAxis("Horizontal") > 0 && (int)_nowRoad + 1 < System.Enum.GetNames(typeof(Roads)).Length)
            {
                _nowRoad = (Roads)((int)_nowRoad + 1);
                _player.transform.position = _roadTransform[_nowRoad].position;
            }
            else if (Input.GetAxis("Horizontal") < 0 && (int)_nowRoad - 1 >= 0)
            {
                _nowRoad = (Roads)((int)_nowRoad - 1);
                _player.transform.position = _roadTransform[_nowRoad].position;
            }
        }
    }

    private void SetText()
    {
        switch (_trueRoad)
        {
            case Roads.LeftRoad:
                leftRoadAnswer.text = _questionModel.TrueAnswer;
                rightRoadAnswer.text = _questionModel.OneFalseAnswer;
                centerRoadAnswer.text = _questionModel.TwoFalseAnswer;
                break;
            case Roads.CentralRoad:
                centerRoadAnswer.text = _questionModel.TrueAnswer;
                rightRoadAnswer.text = _questionModel.OneFalseAnswer;
                leftRoadAnswer.text = _questionModel.TwoFalseAnswer;
                break;
            case Roads.RightRoad:
                rightRoadAnswer.text = _questionModel.TrueAnswer;
                centerRoadAnswer.text = _questionModel.OneFalseAnswer;
                leftRoadAnswer.text = _questionModel.TwoFalseAnswer;
                break;
        }
        questionText.text = _questionModel.QuestionText;
    }

    private void Run()
    {
        obstacle.Speed = changeSpeed;
        _animatorPlayer.Play("Run");
    }

    private void StopRun()
    {
        obstacle.Speed = 1;
        _animatorPlayer.Play("Walk");
    }

    private void EndRun()
    {
        obstacle.Speed = 1;
        stateMachine.ChangeState(stateCheck);
    }
}
