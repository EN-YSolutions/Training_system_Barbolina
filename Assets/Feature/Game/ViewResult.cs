using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ViewResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    public void ShowResult(int countAllQuestion, int allMistake, int percantalResult)
    {
        gameObject.SetActive(true);
        result.text = $"���-�� ��������: {countAllQuestion}\n"
            + $"��� - �� ������: {allMistake}\n"
            + $"����� ���������: {percantalResult}%";
    }
}
