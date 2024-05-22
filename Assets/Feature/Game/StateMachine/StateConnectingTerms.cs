using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateConnectingTerms : BaseState
{
    [SerializeField] private BaseState startState;
    [SerializeField] private StateMachineLevel stateMachine;
    [SerializeField] private TermObject[] termObjects;
    [SerializeField] private DefinitionObject[] definitionObjects;
    [SerializeField] private GameObject panelWithTerms;
    [SerializeField] private TextMeshProUGUI congratulationText;

    private List<TermModel> _termModels;
    private List<int> _indexModels = new List<int>() { 0, 1, 2 };
    private float _timer;
    private float _timeForSolution;

    private Coroutine _coroutineForCongratulation;

    public override void End()
    {
        panelWithTerms.SetActive(false);
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
            termObjects[i].ConnectingModel = _termModels[_indexModels[i]];
        }
        _timer = Time.time + _timeForSolution;
        congratulationText.text = "";
        panelWithTerms.SetActive(true);
    }

    public override void LogicUpdate()
    {
        if (_timer < Time.time && _coroutineForCongratulation == null)
        {
            _coroutineForCongratulation = StartCoroutine(ShowResultSolution());
        }
    }

    private bool _isRightResult = false;

    private IEnumerator ShowResultSolution()
    {
        _isRightResult = true;
        for (int i = 0; i < termObjects.Length; i++)
        {
            if (termObjects[i].Model != termObjects[i].ConnectingModel)
            {
                _isRightResult = false;
                break;
            }
        }

        if (_isRightResult)
        {
            congratulationText.text = "Всё верно!";
        }
        else
        {
            for (int i = 0; i < _termModels.Count; i++)
            {
                termObjects[i].Init(_termModels[i]);
                definitionObjects[i].Init(_termModels[i]);
            }
        }

        yield return new WaitForSeconds(5f);
        stateMachine.ChangeState(startState);
    }

}
