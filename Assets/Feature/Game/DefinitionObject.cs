using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DefinitionObject : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField] private TextMeshProUGUI definitionText;

    private TermModel _termModel;
    private Vector3 _newPosition;
    private TermObject _termObject;
    private Vector3 _startPosition;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;
        definitionText.text = _termModel.Description;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _newPosition = Input.mousePosition;

        _newPosition.z = 0;
        transform.position = _newPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);


        foreach (RaycastResult result in results)
        {
            if(result.gameObject.TryGetComponent<TermObject>(out _termObject))
            {
                _termObject.ConnectingModel = _termModel;
                transform.position = result.gameObject.transform.position;
                break;
            }
        }
    }

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = _startPosition;
    }
}
