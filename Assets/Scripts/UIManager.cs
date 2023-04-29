using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup gameUI;

    [Header("Variables")]
    [SerializeField] private float fadeSpeed;

    // Storage
    private Tween mainMenuTween, gameUITween;

    public void ShowMainMenu()
    {
        SetGameMenuState(false);
    }

    public void ShowGameUI()
    {
        SetGameMenuState(true);
    }

    private void SetGameMenuState(bool state)
    {
        Debug.Log($"SetGameMenuState : {state}");

        GameManager.instance.InMenu = !state;
        SetCanvasGroupState(!state, mainMenu, ref mainMenuTween);
        SetCanvasGroupState(state, gameUI, ref gameUITween);
        SetCursorState(state);
    }

    private void SetCanvasGroupState(bool state, CanvasGroup canvasGroup, ref Tween tween)
    {
        tween.Kill();
        tween = canvasGroup.DOFade(state ? 1 : 0, state ? fadeSpeed * 2 : fadeSpeed);

        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    // VARIOUS
    private void SetCursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.None;
    }

}
