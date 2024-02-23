using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBack : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(Exit);
    }
    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(Exit);
    }

    private void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
