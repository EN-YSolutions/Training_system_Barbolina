using UnityEngine;
using TMPro;

public class TermObject : MonoBehaviour
{
    public DefinitionObject ConnectingDefinition => _connectingDefinitionObject;
    public TermModel Model => _termModel;

    [SerializeField] private TextMeshProUGUI termText;

    private TermModel _termModel;
    private DefinitionObject _connectingDefinitionObject;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;
        termText.text = _termModel.Terminology;
    }

    public void Connecting(DefinitionObject definition)
    {
        if(_connectingDefinitionObject == null)
        {
            _connectingDefinitionObject = definition;
        }
        else
        {
            definition.ConnectTerm.SetDefinition(_connectingDefinitionObject);
            _connectingDefinitionObject = definition;
            definition.Connect(this);
        }
    }

    public void SetDefinition(DefinitionObject definition)
    {
        _connectingDefinitionObject = definition;
        _connectingDefinitionObject.Connect(this);
    }
}
