using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateConnectingTerms : BaseState
{
    [SerializeField] private BaseState startState;
    [SerializeField] private StateMachineLevel stateMachine;
    [SerializeField] private TermObject[] termObjects;
    [SerializeField] private DefinitionObject[] definitionObjects;
    [SerializeField] private GameObject panelWithTerms;
    [SerializeField] private TextMeshProUGUI congratulationText;
    [Space]
    [SerializeField] private Button buttonFinish;
    [SerializeField] private Image timerImage;

    private List<TermModel> _termModels;
    private List<int> _indexModels = new List<int>() { 0, 1, 2 };
    private float _timer;
    private float _timeForSolution;
    private float _timerStart;

    private Coroutine _coroutineForCongratulation;
    private bool _isRightResult = false;

    public override void End()
    {
        panelWithTerms.SetActive(false);
        buttonFinish.onClick.RemoveListener(StartCheckAnswers);
    }

    public override void Enter(QuestionModel questionModel, Roads trueRoad)
    {
        
    }

    public void Init(List<TermModel> termModels)
    {
        _termModels = termModels;
        _timeForSolution = 0;
        _coroutineForCongratulation = null;

        for (int i = _indexModels.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _indexModels[j];
            _indexModels[j] = _indexModels[i];
            _indexModels[i] = temp;
        }

        for (int i = 0; i < _termModels.Count; i++)
        {
            _timeForSolution += _termModels[i].Time;
            termObjects[i].Init(_termModels[i]);
            definitionObjects[i].Init(_termModels[_indexModels[i]]);
            termObjects[i].Connecting(definitionObjects[i]);
            definitionObjects[i].Connect(termObjects[i]);
        }
        _timer = Time.time + _timeForSolution;
        _timerStart = Time.time;
        congratulationText.text = "";
        panelWithTerms.SetActive(true);

        buttonFinish.onClick.AddListener(StartCheckAnswers);
        timerImage.fillAmount = 0;
    }

    public override void LogicUpdate()
    {
        if (_coroutineForCongratulation != null)
            return;


        if (_timer < Time.time)
        {
            _coroutineForCongratulation = StartCoroutine(ShowResultSolution());
            timerImage.fillAmount = 1;
        }
        timerImage.fillAmount = (Time.time - _timerStart) / _timeForSolution;
    }

    private void StartCheckAnswers()
    {
        if (_coroutineForCongratulation != null)
            return;
        _coroutineForCongratulation = StartCoroutine(ShowResultSolution());
        timerImage.fillAmount = 1;
    }

    private IEnumerator ShowResultSolution()
    {
        _isRightResult = true;
        for (int i = 0; i < termObjects.Length; i++)
        {
            if (termObjects[i].Model != termObjects[i].ConnectingDefinition.Model)
            {
                _isRightResult = false;
                stateMachine.AddAnswer(new AnswerModel(termObjects[i].Model.Id, false), false);
            }
            else
                stateMachine.AddAnswer(new AnswerModel(termObjects[i].Model.Id, true), false);
        }


        if (_isRightResult)
        {
            congratulationText.text = "Всё верно!";
        }
        else
        {
            congratulationText.text = "Правильные ответы:";
            for (int i = 0; i < _termModels.Count; i++)
            {
                termObjects[i].Init(_termModels[i]);
                definitionObjects[i].Init(_termModels[i]);
            }
        }

        yield return new WaitForSeconds(3f);
        stateMachine.ChangeState(startState);
    }

}
