using UnityEngine;

public class BaseWindow : MonoBehaviour
{
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
