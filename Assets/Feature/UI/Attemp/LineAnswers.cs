using System.Collections.Generic;
using System;
using UnityEngine;

public class LineAnswers : MonoBehaviour
{
    public event Action<AnswerModel> IsNeedShow = delegate { };

    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject answerPrefab;

    private List<AnswerModel> _answerModels;
    private List<Answer> _answers = new();

    public List<Answer> Initialized(List<AnswerModel> answerModels)
    {
        _answerModels = answerModels;

        foreach(AnswerModel answer in _answerModels)
        {
            Answer answerObj = Instantiate(answerPrefab, contentTransform).GetComponent<Answer>();
            answerObj.Model = answer;
            answerObj.OnClick += Show;
            _answers.Add(answerObj);
        }
        return _answers;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _answers.Count; i++)
        {
            _answers[i].OnClick -= Show;
        }
    }

    private void Show(AnswerModel answerModel)
    {
        IsNeedShow.Invoke(answerModel);
    }
}
