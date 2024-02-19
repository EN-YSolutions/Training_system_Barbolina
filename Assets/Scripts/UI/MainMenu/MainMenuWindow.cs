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
    [Space]
    [SerializeField] private SelectWindow selectWindow;
    [SerializeField] private HistoryWindow historyWindow;

    private void Awake()
    {
        playbutton.onClick.AddListener(OpenSelectWindow);
        historyButton.onClick.AddListener(OpenHistoryWindow);
        exitAccountButton.onClick.AddListener(OpenAuthorizationWindow);
        exitButton.onClick.AddListener(Exit);
    }

    private void OnDestroy()
    {
        playbutton.onClick.RemoveListener(OpenSelectWindow);
        historyButton.onClick.RemoveListener(OpenHistoryWindow);
        exitAccountButton.onClick.RemoveListener(OpenAuthorizationWindow);
        exitButton.onClick.RemoveListener(Exit);
    }

    private void OpenAuthorizationWindow()
    {
        WindowAggregator.Close();
    }

    private void OpenSelectWindow() => WindowAggregator.Open(selectWindow);
    private void OpenHistoryWindow() => WindowAggregator.Open(historyWindow);

    private void Exit() => Application.Quit();
}
