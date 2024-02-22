using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow : BaseWindow
{
    [SerializeField] private Button historyButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button exitButton;
    [Space]
    [SerializeField] private HistoryQuestionWindow historyWindow;
    [SerializeField] private CreateQuestionWindow createWindow;

    private void Awake()
    {
        historyButton.onClick.AddListener(OpenHistoryWindow);
        createButton.onClick.AddListener(OpenCreateQuestionWindow);
        exitButton.onClick.AddListener(WindowAggregator.Close);
    }

    private void OnDestroy()
    {
        historyButton.onClick.RemoveListener(OpenHistoryWindow);
        createButton.onClick.RemoveListener(OpenCreateQuestionWindow);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
    }
    private void OpenHistoryWindow() => WindowAggregator.Open(historyWindow);
    private void OpenCreateQuestionWindow() => WindowAggregator.Open(createWindow);
}
