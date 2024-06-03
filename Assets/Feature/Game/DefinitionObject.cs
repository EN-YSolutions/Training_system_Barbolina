using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DefinitionObject : MonoBehaviour, IDragHandler, IDropHandler, IEndDragHandler
{
    public TermModel Model => _termModel;
    public TermObject ConnectTerm => _termObject;


    [SerializeField] private TextMeshProUGUI definitionText;
    [SerializeField] private TermObject _termObject;

    private TermObject _temp;
    private TermModel _termModel;
    private Vector3 _newPosition;
    private Vector3 _startPosition;
    private Vector3 _connectingPosition;

    private bool _isClick = false;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;
        definitionText.text = _termModel.Description;
    }

    public void Connect(TermObject termObject)
    {
        _isClick = false;
        _termObject = termObject;
        _connectingPosition = _termObject.gameObject.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isClick = true;
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
            if (result.gameObject.TryGetComponent<TermObject>(out _temp))
            {
                _temp.Connecting(this);
                return;
            }
        }
        _isClick = false;
    }

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = _startPosition;
    }

    private void Update()
    {
        if (!_isClick)
            transform.position = _connectingPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isClick = false;
    }
}
