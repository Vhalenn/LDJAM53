using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIGoal : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI goalSuccessText;

    private Color HIDDEN = new Color(1, 1, 1, 0);
    private Color SHOWN = new Color(1, 1, 1, 1);

    private void Start()
    {
        goalSuccessText.color = HIDDEN;
    }

    public void UpdateText(string text)
    {
        goalText.text = text;
    }

    public void GoalSuccess()
    {
        if (goalSuccessText == null) return;

        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(goalSuccessText.DOColor(SHOWN, 0.25f));
        fadeSequence.AppendInterval(2f);
        fadeSequence.Append(goalSuccessText.DOColor(HIDDEN, 0.5f));
    }

}
