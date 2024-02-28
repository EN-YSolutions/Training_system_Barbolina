using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLevel : MonoBehaviour
{
    [SerializeField] private StateShowQuestion stateShow;
    [SerializeField] private ViewResult viewResult;

    private List<QuestionModel> _allQuestions = new();
    private List<MistakeModel> _allMistakes = new();

    private int _numQuestion = 0;

    private Roads _trueRoad;

    private BaseState _nowState;

    public void ChangeState(BaseState state)
    {
        _nowState.End();
        if (state is StateShowQuestion)
        {
            _numQuestion++;
            StartLoopGame();
            return;
        }

        state.Enter(_allQuestions[_numQuestion], _trueRoad);
        _nowState = state;
    }

    public void AddMistake()
    {
        _allMistakes.Add(new MistakeModel(_allQuestions[_numQuestion].Id, DatabaseConnector.IdNowUser, DatabaseConnector.IdCources));
    }

    private void Awake()
    {
        _allQuestions = DatabaseConnector.AllCoursesQuestionsForRepetition();

        System.Random RND = new System.Random();
        for (int i = 0; i < _allQuestions.Count; i++)
        {
            var tmp = _allQuestions[0];
            _allQuestions.RemoveAt(0);
            _allQuestions.Insert(RND.Next(_allQuestions.Count), tmp);
        }

        StartLoopGame();
    }

    private void Update()
    {
        _nowState.LogicUpdate();
    }

    private void StartLoopGame()
    {
        if (_numQuestion == _allQuestions.Count)
            EndLevel();
        _trueRoad = (Roads)Random.Range(0, 3);
        _nowState = stateShow;
        stateShow.Enter(_allQuestions[_numQuestion], _trueRoad);
    }

    private void EndLevel()
    {
        int percantalresult = (int)((float)(_allQuestions.Count - _allMistakes.Count) / (float)_allQuestions.Count * 100f);

        viewResult.ShowResult(_allQuestions.Count, _allMistakes.Count, percantalresult);

        string id = DatabaseConnector.AddRepetition(percantalresult);

        foreach (var mistake in _allMistakes)
        {
            mistake.IdRepetitionCource = id;
            DatabaseConnector.AddMistake(mistake);
        }
    }
}
