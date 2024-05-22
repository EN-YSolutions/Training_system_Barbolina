using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateQuestionWindow : BaseWindow
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button importButton;
    [Space]
    [SerializeField] private GameObject rightCreateWindow;
    [SerializeField] private TMP_Dropdown courcesDropdown;
    [SerializeField] private TMP_InputField startPointInput;
    [SerializeField] private TMP_InputField timeInput;
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField expenstionInput;
    [SerializeField] private TMP_InputField trueAnswerInput;
    [SerializeField] private TMP_InputField oneFalseAnswerInput;
    [SerializeField] private TMP_InputField twoFalseAnswerInput;
    [Space]
    [SerializeField] private ImportQuestionExcelWindow importExcel;

    private Dictionary<string, string> _idAndTitle = new();

    private void Awake()
    {
        createButton.onClick.AddListener(CreateQuestion);
        exitButton.onClick.AddListener(WindowAggregator.Close);
        importButton.onClick.AddListener(OpenImportWindow);
    }

    private void OnEnable()
    {
        createButton.image.color = Color.white;
        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        ClearForm();
        courcesDropdown.ClearOptions();
        _idAndTitle.Clear();
        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
        courcesDropdown.RefreshShownValue();
    }

    private void OnDestroy()
    {
        createButton.onClick.RemoveListener(CreateQuestion);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
        importButton.onClick.RemoveListener(OpenImportWindow);
    }

    private void CreateQuestion()
    {
        if (DatabaseConnector.AddQuestion(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text],
             startPointInput.text, questionInput.text, trueAnswerInput.text, oneFalseAnswerInput.text,
             twoFalseAnswerInput.text, expenstionInput.text, timeInput.text))
        {
            rightCreateWindow.SetActive(true);
            createButton.image.color = Color.white;
            ClearForm();
        }
        else
        {
            createButton.image.color = Color.red;
        }
    }

    private void ClearForm()
    {
        startPointInput.text = "";
        questionInput.text = "";
        trueAnswerInput.text = "";
        oneFalseAnswerInput.text = "";
        twoFalseAnswerInput.text = "";
        expenstionInput.text = "";
        timeInput.text = "";
    }

    private void OpenImportWindow()
    {
        importExcel.Init(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text], courcesDropdown.options[courcesDropdown.value].text);
    }
}
