using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineLevel : MonoBehaviour
{
    [SerializeField] private StateShowQuestion stateShow;
    [SerializeField] private StateConnectingTerms connectingTerms;
    [SerializeField] private ViewResult viewResult;
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GameObject _chipmunkPrefab;

    private List<QuestionModel> _allQuestions = new();
    private List<QuestionModel> _usedQuestions = new();
    private List<AnswerModel> _allAnswersQuestion = new();
    private List<AnswerModel> _allAnswersTerm = new();
    private List<TermModel> _termModels = new();
    private List<TermModel> _termUsedModels = new();
    private int _needTerms = 3;
    private int _indexConnectingGames;
    private bool _isNeedConnectingGames;

    private int _numQuestion = 0;
    private int _endCountQuestion = 0;
    private int _allAnswer = 0;
    private int _allRightAnswer = 0;

    private Roads _trueRoad;

    private BaseState _nowState;
    private bool _isEnd = false;

    public void ChangeState(BaseState state)
    {
        if (_isEnd)
            return;
        _nowState.End();
        if (state == stateShow)
        {
            if (_isNeedConnectingGames && _indexConnectingGames == _numQuestion + 1)
            {
                _nowState = connectingTerms;
                connectingTerms.Init(_termUsedModels);
                _isNeedConnectingGames = false;
                return;
            }
            _numQuestion++;
            StartLoopGame();
            return;
        }

        state.Enter(_usedQuestions[_numQuestion], _trueRoad);
        _nowState = state;
    }

    public void AddAnswer(AnswerModel answer, bool isQuestion = true)
    {
        if (isQuestion)
            _allAnswersQuestion.Add(answer);
        else
            _allAnswersTerm.Add(answer);
    }

    private void Awake()
    {
        _allQuestions = DatabaseConnector.AllCoursesQuestionsForRepetition();
        _termModels = DatabaseConnector.AllCoursTermsForRepetition();

        _isNeedConnectingGames = false;

        if (_termModels.Count >= _needTerms)
        {
            _isNeedConnectingGames = true;
            _endCountQuestion += 1;
            System.Random R = new System.Random();
            for (int i = 0; i < _needTerms; i++)
            {
                var index = R.Next(_termModels.Count);
                _termUsedModels.Add(_termModels[index]);
                _termModels.RemoveAt(index);
            }
            _indexConnectingGames = Random.Range(1, DatabaseConnector.MaxQuantityQuestion - 1);
        }


        if (DatabaseConnector.PlayerHero == "Котик")
            Instantiate(_catPrefab);
        else
            Instantiate(_chipmunkPrefab);

        System.Random RND = new System.Random();
        for (int i = 0; i < DatabaseConnector.MaxQuantityQuestion - (_isNeedConnectingGames ? 1 : 0); i++)
        {
            var index = RND.Next(_allQuestions.Count);
            _usedQuestions.Add(_allQuestions[index]);
            _allQuestions.RemoveAt(index);
        }
        _endCountQuestion += _usedQuestions.Count;

        StartLoopGame();
    }

    private void Update()
    {
        if (_nowState != null && !_isEnd)
            _nowState.LogicUpdate();
    }

    private void StartLoopGame()
    {
        if (_isEnd)
            return;
        if (_numQuestion >= _usedQuestions.Count )
        {
            _isEnd = true;
            EndLoopGame();
            return;
        }
        _trueRoad = (Roads)Random.Range(0, 3);
        _nowState = stateShow;
        stateShow.Enter(_usedQuestions[_numQuestion], _trueRoad);
    }

 

    private void EndLoopGame()
    {
        _allAnswer = _allAnswersQuestion.Count + _allAnswersTerm.Count;

        for (int i = 0; i < _allAnswersQuestion.Count; i++)
        {
            if (_allAnswersQuestion[i].Answer)
                _allRightAnswer++;
        }

        for (int i = 0; i < _allAnswersTerm.Count; i++)
        {
            if (_allAnswersTerm[i].Answer)
                _allRightAnswer++;
        }

        int percantalresult = 100;

        if (_allRightAnswer != _allAnswer)
            percantalresult = (int)((float)_allRightAnswer / (float)_allAnswer * 100f);

        viewResult.ShowResult(_allAnswer, _allAnswer - _allRightAnswer, percantalresult);

        string id = DatabaseConnector.AddRepetition(percantalresult);

        foreach (var answer in _allAnswersQuestion)
        {
            answer.IdRepetitionCource = id;
            DatabaseConnector.AddQuestionAnswer(answer);
        }

        foreach (var answer in _allAnswersTerm)
        {
            answer.IdRepetitionCource = id;
            DatabaseConnector.AddTermAnswer(answer);
        }
    }
}
