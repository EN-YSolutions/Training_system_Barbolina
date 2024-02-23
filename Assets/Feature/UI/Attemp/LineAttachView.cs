using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineAttachView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public string Text { set { text.text = value; } }
}
