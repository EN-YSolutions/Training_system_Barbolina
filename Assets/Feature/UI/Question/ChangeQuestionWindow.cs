using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuestionWindow : BaseWindow
{
    [SerializeField] private Button changeButton;
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private TMP_InputField startPointInput;
    [SerializeField] private TMP_InputField timeInput;
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField expenstionInput;
    [SerializeField] private TMP_InputField trueAnswerInput;
    [SerializeField] private TMP_InputField oneFalseAnswerInput;
    [SerializeField] private TMP_InputField twoFalseAnswerInput;
    [SerializeField] private TMP_Dropdown courcesDropdown;

    private QuestionModel _questionModel;
    private Dictionary<string, string> _idAndTitle = new();
    private int _selectCources;

    public void Init(QuestionModel questionModel)
    {
        _questionModel = questionModel;
        startPointInput.text = questionModel.StartPoint.ToString();
        timeInput.text = questionModel.Time.ToString();
        questionInput.text = questionModel.QuestionText;
        expenstionInput.text = questionModel.Explanation;
        trueAnswerInput.text = questionModel.TrueAnswer;
        oneFalseAnswerInput.text = questionModel.OneFalseAnswer;
        twoFalseAnswerInput.text = questionModel.TwoFalseAnswer;

        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        courcesDropdown.ClearOptions();
        _idAndTitle.Clear();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
            if(id == questionModel.IdCources)
            {
                _selectCources = courcesDropdown.options.Count - 1;
            }
        }
        courcesDropdown.value = _selectCources;
        courcesDropdown.Select();
        courcesDropdown.RefreshShownValue();
    }

    private void Awake()
    {
        changeButton.onClick.AddListener(ChangeQuestion);
        closeButton.onClick.AddListener(ComeBack);
    }

    private void OnDestroy()
    {
        changeButton.onClick.RemoveListener(ChangeQuestion);
        closeButton.onClick.RemoveListener(ComeBack);
    }

    private void ChangeQuestion()
    {
       DatabaseConnector.ChangeQuestion(_questionModel.Id, _idAndTitle[courcesDropdown.options[courcesDropdown.value].text], startPointInput.text, questionInput.text, trueAnswerInput.text, oneFalseAnswerInput.text,
            twoFalseAnswerInput.text, expenstionInput.text, timeInput.text);
        WindowAggregator.Close();
    }
}
