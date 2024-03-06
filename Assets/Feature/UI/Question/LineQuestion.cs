using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineQuestion : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text statisticText;
    [SerializeField] private Button button;

    public string QuestionText { set => questionText.text = value; }
    public string StatisticText { set => statisticText.text = value; }

    public TMP_Text QuestionTMPText => questionText;
    public Button ButtonQuestion => button;
}
