using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : BaseWindow
{
    [SerializeField] private Button playbutton;
    [SerializeField] private Button historyButton;
    [SerializeField] private Button exitAccountButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button questionButton;
    [Space]
    [SerializeField] private SelectWindow selectWindow;
    [SerializeField] private HistoryWindow historyWindow;
    [SerializeField] private QuestionWindow questionWindow;

    private void Awake()
    {
        playbutton.onClick.AddListener(OpenSelectWindow);
        historyButton.onClick.AddListener(OpenHistoryWindow);
        questionButton.onClick.AddListener(OpenQuestionWindow);
        exitAccountButton.onClick.AddListener(OpenAuthorizationWindow);
        exitButton.onClick.AddListener(Exit);
    }

    private void OnEnable()
    {
        questionButton.gameObject.SetActive(DatabaseConnector.CheckAuthorCources());
    }

    private void OnDestroy()
    {
        playbutton.onClick.RemoveListener(OpenSelectWindow);
        historyButton.onClick.RemoveListener(OpenHistoryWindow);
        exitAccountButton.onClick.RemoveListener(OpenAuthorizationWindow);
        questionButton.onClick.RemoveListener(OpenQuestionWindow);
        exitButton.onClick.RemoveListener(Exit);
    }

    private void OpenAuthorizationWindow()
    {
        WindowAggregator.Close();
    }

    private void OpenSelectWindow() => WindowAggregator.Open(selectWindow);
    private void OpenHistoryWindow() => WindowAggregator.Open(historyWindow);
    private void OpenQuestionWindow() => WindowAggregator.Open(questionWindow);

    private void Exit() => Application.Quit();
}
