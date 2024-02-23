using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLevel : MonoBehaviour
{
    [SerializeField] private StateShowQuestion stateShow;

    private List<QuestionModel> _allQuestions = new();
    private List<MistakeModel> _allMistakes = new();

    private int _numQuestion = 0;

    private Roads _trueRoad;

    private BaseState _nowState;

    public void ChangeState(BaseState state)
    {
        _nowState.End();
        if (state is StateShowQuestion)
            StartLoopGame();

        state.Enter(_allQuestions[_numQuestion], _trueRoad);
        _nowState = state;
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
        _trueRoad = (Roads)Random.Range(1, 4);
        _nowState = stateShow;
        stateShow.Enter(_allQuestions[_numQuestion], _trueRoad);
        _numQuestion++;
    }
}
