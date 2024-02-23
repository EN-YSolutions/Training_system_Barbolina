using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameWindow : MonoBehaviour
{
    [SerializeField] private ObstacleController obstacle;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameWindowView view;
    [SerializeField] private ViewRightAnswer rightAnswer;
    [SerializeField] private ViewResult result;

    private List<QuestionModel> AllQuestions = new();
    private List<MistakeModel> AllMistakes = new();

    private int numberQ = 0;

    private Roads TrueRoad;

    private void Awake()
    {
        

        StartCoroutine(StartRun());
    }

    private IEnumerator StartRun()
    {
        if (numberQ == AllQuestions.Count)
            ResultRun();

        int rand = Random.Range(0, 2);
        TrueRoad = rand == 0 ? Roads.LeftRoad : Roads.RightRoad; 

        //view.ShowQuestion(AllQuestions[numberQ].QuestionText, rand == 0 ? AllQuestions[numberQ].TrueAnswer : AllQuestions[numberQ].FalseAnswer,
        //    rand == 1 ? AllQuestions[numberQ].TrueAnswer : AllQuestions[numberQ].FalseAnswer);
        obstacle.Init(AllQuestions[numberQ].Time);

        yield return new WaitForSeconds(AllQuestions[numberQ].Time);

        CheckRight();
        
    }
    
    private void CheckRight()
    {
        if (TrueRoad != player.NowRoad)
            WrongAnswer();

        if (numberQ < AllQuestions.Count)
        {
            numberQ++;
            if (numberQ == AllQuestions.Count)
            {
                ResultRun();
                return;
            }
            StartCoroutine(StartRun());
        }
    }

    private void WrongAnswer()
    {
        AllMistakes.Add(new MistakeModel(AllQuestions[numberQ].Id, DatabaseConnector.IdNowUser, DatabaseConnector.IdCources));
        rightAnswer.Show(AllQuestions[numberQ]);
    }

    private void ResultRun()
    {
        int percantalresult = (int)((float)(AllQuestions.Count - AllMistakes.Count) / (float)AllQuestions.Count * 100f);

        result.ShowResult(AllQuestions.Count, AllMistakes.Count, percantalresult);

        string id = DatabaseConnector.AddRepetition(percantalresult);

        foreach (var mistake in AllMistakes)
        {
            mistake.IdRepetitionCource = id;
            DatabaseConnector.AddMistake(mistake);
        }

    }
}
