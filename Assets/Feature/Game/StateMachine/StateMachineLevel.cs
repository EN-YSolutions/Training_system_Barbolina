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
    private List<MistakeModel> _allMistakes = new();
    private List<TermModel> _termModels = new();
    private List<TermModel> _termUsedModels = new();
    private int _needTerms = 3;
    private int _indexConnectingGames;
    private bool _isNeedConnectingGames;

    private int _numQuestion = 0;

    private Roads _trueRoad;

    private BaseState _nowState;

    public void ChangeState(BaseState state)
    {
        _nowState.End();
        if (state == stateShow)
        {
            if (_isNeedConnectingGames && _indexConnectingGames + 1 == _numQuestion)
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

    public void AddMistake()
    {
        _allMistakes.Add(new MistakeModel(_usedQuestions[_numQuestion].Id, DatabaseConnector.IdNowUser, DatabaseConnector.IdCources));
    }

    private void Awake()
    {
        _allQuestions = DatabaseConnector.AllCoursesQuestionsForRepetition();
        _termModels = DatabaseConnector.AllCoursTermsForRepetition();

        _isNeedConnectingGames = false;

        if (_termModels.Count >= _needTerms)
        {
            _isNeedConnectingGames = true;
            System.Random R = new System.Random();
            for (int i = 0; i < _needTerms; i++)
            {
                var index = R.Next(_termModels.Count);
                _termUsedModels.Add(_termModels[index]);
                _termModels.RemoveAt(index);
            }
            _indexConnectingGames = Random.Range(1, DatabaseConnector.MaxQuantityQuestion - 2);
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

        StartLoopGame();
    }

    private void Update()
    {
        if (_nowState != null)
            _nowState.LogicUpdate();
    }

    private void StartLoopGame()
    {
        if (_numQuestion >= _usedQuestions.Count)
        {
            EndLoopGame();
            return;
        }
        _trueRoad = (Roads)Random.Range(0, 3);
        _nowState = stateShow;
        stateShow.Enter(_usedQuestions[_numQuestion], _trueRoad);
    }

    private void EndLoopGame()
    {
        int percantalresult = (int)((float)(_usedQuestions.Count - _allMistakes.Count) / (float)_usedQuestions.Count * 100f);

        viewResult.ShowResult(DatabaseConnector.MaxQuantityQuestion, _allMistakes.Count, percantalresult);

        //string id = DatabaseConnector.AddRepetition(percantalresult);

        //foreach (var mistake in _allMistakes)
        //{
        //    mistake.IdRepetitionCource = id;
        //    DatabaseConnector.AddMistake(mistake);
        //}
    }
}
