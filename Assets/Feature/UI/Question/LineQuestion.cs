using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineQuestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI queestionText;
    [SerializeField] private TextMeshProUGUI statisticText;
    [SerializeField] private Button button;

    public string QuestionText { set => queestionText.text = value; }
    public string StatisticText { set => statisticText.text = value; }

    public Button ButtonQuestion => button;
}
