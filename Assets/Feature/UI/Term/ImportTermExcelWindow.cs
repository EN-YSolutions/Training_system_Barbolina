using Crosstales.FB;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImportTermExcelWindow : BaseWindow
{
    [SerializeField] private Button importButton;
    [SerializeField] private TextMeshProUGUI textExplanation;

    private string _path;
    private string _id;

    public void Init(string id, string nameCources)
    {
        _id = id;
        textExplanation.text = nameCources + textExplanation.text;
    }

    private void Start()
    {
        importButton.onClick.AddListener(ImportNewQuastions);
    }

    private void OnDestroy()
    {
        importButton.onClick.RemoveListener(ImportNewQuastions);
    }

    private void ImportNewQuastions()
    {
        _path = FileBrowser.Instance.OpenSingleFile("Âûבונטעו פאיכ Excel", string.Empty, string.Empty, "xlsx", "xls");

        if (_path == "")
            return;

        ExcelPackage excelPackage = new ExcelPackage(new FileInfo(_path));
        ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.First();
        ExcelCellAddress end = workSheet.Dimension.End;
        ExcelCellAddress start = workSheet.Dimension.Start;

        for (int i = workSheet.Dimension.Start.Row; i <= end.Row; i++)
        {
            List<string> question = new();
            for (int col = start.Column; col <= end.Column; col++)
            {
                question.Add(workSheet.Cells[i, col].Text);
            }
            DatabaseConnector.AddTerm(_id, question[0], question[2], question[3],question[1]);
        }

        gameObject.SetActive(false);
    }
}
