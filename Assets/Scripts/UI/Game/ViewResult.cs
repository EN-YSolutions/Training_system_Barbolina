using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ViewResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private Button exitButton;
    public void ShowResult(int countAllQuestion, int allMistake, int percantalResult)
    {
        gameObject.SetActive(true);
        result.text = $"���-�� ��������: {countAllQuestion}\n"
            + $"��� - �� ������: {allMistake}\n"
            + $"����� ���������: {percantalResult}%";

        exitButton.onClick.AddListener(Exit);
    }

    private void Exit()
    {
        exitButton.onClick.RemoveListener(Exit);
        SceneManager.LoadScene(0);
    }

}
