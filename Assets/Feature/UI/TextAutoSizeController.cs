using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;
using System.Linq;

public enum ResizePattern
{
    /// <summary>
    /// fast, balanced quality
    /// </summary>
    IgnoreRichText,

    /// <summary>
    /// fast, simple
    /// </summary>
    AllCharacters,
}

[ExecuteAlways]
public class TextAutoSizeController : MonoBehaviour
{
    private const string _tooltipText = "IgnoreRichText - fast, balanced\nqualityAllCharacters - fast, simple";

    [SerializeField, Tooltip(_tooltipText)]
    private ResizePattern _pattern;

    [SerializeField] private Vector2 _size = new Vector2(18, 72);
    [SerializeField] private bool _executeOnUpdate = true;
    [SerializeField] private List<TMP_Text> _labels;
    private int _currentIndex;

    private void Update()
    {
        if (_labels == null || _labels.Count == 0) return;
        if (_executeOnUpdate) Execute();
        OnUpdateCheck();
    }

    public void Init(List<TMP_Text> texts)
    {
        _labels = texts;
    }

    public void Clear()
    {
        _labels.Clear();
        _labels = null;
    }

    public void Execute()
    {
        if (_labels == null || _labels.Count == 0) return;

        int count = _labels.Count;
        int index = 0;
        float maxLength = 0;

        for (int i = 0; i < count; i++)
        {
            float length = _pattern switch
            {
                ResizePattern.IgnoreRichText => _labels[i].GetParsedText().Length,
                ResizePattern.AllCharacters => _labels[i].text.Length,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (length > maxLength)
            {
                maxLength = length;
                index = i;
            }
        }

        if (_currentIndex != index)
        {
            OnChanged(index);
        }
    }

    private void OnChanged(int index)
    {
        if (_currentIndex >= _labels.Count)
            return;
        _labels[_currentIndex].enableAutoSizing = false;
        _currentIndex = index;
        _labels[index].fontSizeMin = _size.x;
        _labels[index].fontSizeMax = _size.y;
        _labels[index].enableAutoSizing = true;
        _labels[index].ForceMeshUpdate();
    }

    private void OnUpdateCheck()
    {
        if (_labels == null || _labels.Count == 0 || _currentIndex >= _labels.Count) return;
        float optimumPointSize = _labels[_currentIndex].fontSize;
        int count = _labels.Count;

        for (int i = 0; i < count; i++)
        {
            if (_currentIndex == i) continue;
            _labels[i].enableAutoSizing = false;
            _labels[i].fontSize = optimumPointSize;
        }

    }

    [ContextMenu("Collect Child Components")]
    public void CollectChildComponents()
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "collect child components");
#endif

        _labels = GetComponentsInChildren<TMP_Text>(true).ToList();
    }

    private void Reset() => CollectChildComponents();

    [ContextMenu("Validate values")]
    private void OnValidate() => OnChanged(_currentIndex);
}

