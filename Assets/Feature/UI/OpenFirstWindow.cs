using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFirstWindow : MonoBehaviour
{
    [SerializeField] BaseWindow firstWindow;

    private void Awake()
    {
        WindowAggregator.Open(firstWindow);
    }
}
