using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTermWindow : BaseWindow
{
    public event Action<string> OnDeleteQuestion;

    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI termText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [Space]
    [SerializeField] private Button buttonDelete;
    [SerializeField] private Button buttonChange;
    [Space]
    [SerializeField] private ChangeTempWindow changeTermWindow;

    private TermModel _termModel;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;

        gameObject.SetActive(true);
        termText.text = termModel.Terminology;
        descriptionText.text = termModel.Description;
    }

    private void Awake()
    {
        buttonChange.onClick.AddListener(ChangeQuestion);
        buttonDelete.onClick.AddListener(DeleteQuestion);
    }

    private void OnDestroy()
    {
        buttonChange.onClick.RemoveListener(ChangeQuestion);
        buttonDelete.onClick.RemoveListener(DeleteQuestion);
    }

    private void DeleteQuestion()
    {
        OnDeleteQuestion.Invoke(_termModel.Id);
        gameObject.SetActive(false);
    }

    private void ChangeQuestion()
    {
        gameObject.SetActive(false);
        WindowAggregator.Open(changeTermWindow);
        changeTermWindow.Init(_termModel);
    }
}
