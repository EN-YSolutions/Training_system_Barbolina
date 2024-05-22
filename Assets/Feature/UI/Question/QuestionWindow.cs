using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionWindow : BaseWindow
{
    [SerializeField] private Button historyButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Button createTempButton;
    [SerializeField] private Button historyTempButton;
    [SerializeField] private Button exitButton;
    [Space]
    [SerializeField] private HistoryQuestionWindow historyWindow;
    [SerializeField] private CreateQuestionWindow createWindow;
    [SerializeField] private CreateTermWindow createTermWindow;
    [SerializeField] private HistoryTermWindow historyTermWindow;

    private void Awake()
    {
        historyButton.onClick.AddListener(OpenHistoryWindow);
        createButton.onClick.AddListener(OpenCreateQuestionWindow);
        createTempButton.onClick.AddListener(OpenCreateTermWindow);
        historyTempButton.onClick.AddListener(OpenHistoryTermWindow);
        exitButton.onClick.AddListener(WindowAggregator.Close);
    }

    private void OnDestroy()
    {
        historyButton.onClick.RemoveListener(OpenHistoryWindow);
        createButton.onClick.RemoveListener(OpenCreateQuestionWindow);
        createTempButton.onClick.RemoveListener(OpenCreateTermWindow);
        historyTempButton.onClick.RemoveListener(OpenHistoryTermWindow);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
    }
    private void OpenHistoryWindow() => WindowAggregator.Open(historyWindow);
    private void OpenHistoryTermWindow() => WindowAggregator.Open(historyTermWindow);
    private void OpenCreateQuestionWindow() => WindowAggregator.Open(createWindow);
    private void OpenCreateTermWindow() => WindowAggregator.Open(createTermWindow);
}
