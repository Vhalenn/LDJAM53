using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIManager uiManager;
    public UIManager UIManager => uiManager;

    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private bool startWithMenu;
    [SerializeField] private bool inMenu;
    public bool InMenu { get => inMenu; set => inMenu = value; }

    private void Start()
    {
        if(startWithMenu || !Application.isEditor)
        {
            ShowMainMenu();
        }
        else
        {
            ShowGame();
        }

        Cursor.lockState = CursorLockMode.Confined;
        gameplayManager.PlacePlayerOnSelection();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InMenu) ShowGame();
            else ShowMainMenu();
        }
    }

    // MENU / GAME STATE
    public void ShowMainMenu()
    {
        uiManager.ShowMainMenu();
    }

    public void ShowGame()
    {
        uiManager.ShowGameUI();
    }

}
