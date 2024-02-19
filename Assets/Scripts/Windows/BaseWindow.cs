using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public bool IsClosePrevWindow => isClosePrevWindow;

    [SerializeField] protected bool isClosePrevWindow;


    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    protected void ComeBack() => WindowAggregator.Close();
}
