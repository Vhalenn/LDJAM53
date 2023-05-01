using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup gameUI;

    [Header("Game")]
    [SerializeField] private UIGoal uiGoal;
    [SerializeField] private UIDialog uiDialog;
    [SerializeField] private TextMeshProUGUI startButtonText;
    [SerializeField] private TextMeshProUGUI selectionNames;

    [Header("Variables")]
    [SerializeField] private float fadeSpeed;

    [Header("Storage")]
    [SerializeField] private bool alreadyPlayed;
    private Tween mainMenuTween, gameUITween;

    public void ShowMainMenu()
    {
        SetGameMenuState(false);
    }

    public void ShowGameUI()
    {
        SetGameMenuState(true);
    }

    public void SetSelectionNames(string value)
    {
        if(selectionNames) selectionNames.text = value;
    }

    private void SetGameMenuState(bool state)
    {
        Debug.Log($"SetGameMenuState : {state}");

        if(state)
        {
            alreadyPlayed = true;
        }

        if(!state && alreadyPlayed && startButtonText)
        {
            startButtonText.text = "Resume";
        }

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

    public void UpdateGoalText(string text)
    {
        uiGoal.UpdateText(text);
    }

    public void ShowUIDialog(string text)
    {
        uiDialog.DisplayText(text);
    }

    public void GoalSuccess()
    {
        uiGoal.GoalSuccess();
    }

    // VARIOUS
    private void SetCursorState(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.None;
    }

}
