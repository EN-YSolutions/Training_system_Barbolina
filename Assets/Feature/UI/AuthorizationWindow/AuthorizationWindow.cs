using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthorizationWindow : BaseWindow
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button authorizationButton;
    [SerializeField] private Button exitButton;
    [Space]
    [SerializeField] private MainMenuWindow mainMenu;

    private void Start()
    {
        authorizationButton.onClick.AddListener(TryOpen);
        exitButton.onClick.AddListener(Exit);

        if (DatabaseConnector.IdNowUser != "")
            WindowAggregator.Open(mainMenu);
    }

    private void OnEnable()
    {
        nameInput.Select();
        nameInput.text = "";
        passwordInput.Select();
        passwordInput.text = "";
    }

    private void OnDestroy()
    {
        authorizationButton.onClick.RemoveListener(TryOpen);
        exitButton.onClick.RemoveListener(Exit);
    }

    private void TryOpen()
    {
        if (DatabaseConnector.Verification(nameInput.text, passwordInput.text))
            WindowAggregator.Open(mainMenu);
    }

    private void Exit() => Application.Quit();
}
