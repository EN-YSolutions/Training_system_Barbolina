using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTermWindow : BaseWindow
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button importButton;
    [Space]
    [SerializeField] private TMP_Dropdown courcesDropdown;
    [SerializeField] private TMP_InputField startPointInput;
    [SerializeField] private TMP_InputField timeInput;
    [SerializeField] private TMP_InputField termInput;
    [SerializeField] private TMP_InputField descriptionInput;
    [Space]
    [SerializeField] private ImportTermExcelWindow importExcel;

    private Dictionary<string, string> _idAndTitle = new();

    private void Awake()
    {
        createButton.onClick.AddListener(CreateTerm);
        exitButton.onClick.AddListener(WindowAggregator.Close);
        importButton.onClick.AddListener(OpenImportWindow);
    }

    private void OnEnable()
    {
        createButton.image.color = Color.white;
        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        ClearForm();
        courcesDropdown.ClearOptions();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
        }
        courcesDropdown.RefreshShownValue();
    }

    private void OnDestroy()
    {
        createButton.onClick.RemoveListener(CreateTerm);
        exitButton.onClick.RemoveListener(WindowAggregator.Close);
        importButton.onClick.RemoveListener(OpenImportWindow);
    }

    private void CreateTerm()
    {
        if (DatabaseConnector.AddTerm(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text],
             startPointInput.text, termInput.text, descriptionInput.text, timeInput.text))
        {
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
        termInput.text = "";
        descriptionInput.text = "";
        timeInput.text = "";
    }

    private void OpenImportWindow()
    {
        importExcel.Init(_idAndTitle[courcesDropdown.options[courcesDropdown.value].text], courcesDropdown.options[courcesDropdown.value].text);
    }
}
