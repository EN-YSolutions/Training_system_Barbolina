using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryQuestionWindow : BaseWindow
{
    [SerializeField] private Button showButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        showButton.onClick.AddListener(ShowQuestion);
        exitButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        showButton.onClick.RemoveListener(ShowQuestion);
        exitButton.onClick.RemoveListener(Close);
    }

    private void ShowQuestion()
    {
        Debug.Log(this);
    }
}
