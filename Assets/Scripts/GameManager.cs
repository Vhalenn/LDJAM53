using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private bool startWithMenu;

    private void Start()
    {
        if(startWithMenu || !Application.isEditor)
        {
            uiManager.ShowMainMenu();
        }
    }

}
