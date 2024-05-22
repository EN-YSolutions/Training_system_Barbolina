using UnityEngine;
using TMPro;

public class TermObject : MonoBehaviour
{
    public TermModel ConnectingModel;
    public TermModel Model => _termModel;

    [SerializeField] private TextMeshProUGUI termText;

    private TermModel _termModel;

    public void Init(TermModel termModel)
    {
        _termModel = termModel;
        termText.text = _termModel.Terminology;
    }
}
