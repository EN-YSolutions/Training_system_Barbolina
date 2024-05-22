using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTempWindow : BaseWindow
{
    [SerializeField] private Button changeButton;
    [SerializeField] private Button closeButton;
    [Space]
    [SerializeField] private TMP_InputField startPointInput;
    [SerializeField] private TMP_InputField timeInput;
    [SerializeField] private TMP_InputField termInput;
    [SerializeField] private TMP_InputField discriptionInput;
    [SerializeField] private TMP_Dropdown courcesDropdown;

    private TermModel _termModel;
    private Dictionary<string, string> _idAndTitle = new();
    private int _selectCources;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;
        startPointInput.text = termModel.StartPoint.ToString();
        timeInput.text = termModel.Time.ToString();
        termInput.text = termModel.Terminology;
        discriptionInput.text = termModel.Description;


        List<string> idCources = DatabaseConnector.AllCoursesUserAuthor();
        courcesDropdown.ClearOptions();
        _idAndTitle.Clear();

        foreach (string id in idCources)
        {
            _idAndTitle.Add(DatabaseConnector.TitleCourse(id), id);
            courcesDropdown.options.Add(new TMP_Dropdown.OptionData(DatabaseConnector.TitleCourse(id)));
            if (id == termModel.Id)
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
        DatabaseConnector.ChangeTerm(_termModel.Id, _idAndTitle[courcesDropdown.options[courcesDropdown.value].text], startPointInput.text, termInput.text,
             discriptionInput.text, timeInput.text);
        WindowAggregator.Close();
    }
}
