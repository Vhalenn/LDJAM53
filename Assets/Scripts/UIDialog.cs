using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIDialog : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogText;
    Tween fadeTween;

    Coroutine typeText;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void DisplayText(string text)
    {
        fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(1, 0.5f);

        Debug.Log($"DISPLAY DIALOG : {text}");
        if(typeText != null) StopCoroutine(typeText);
        typeText = StartCoroutine(TypingEffect(text));
    }

    private IEnumerator TypingEffect(string text)
    {
        dialogText.maxVisibleCharacters = 0;
        dialogText.text = text;
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < text.Length + 3; i++)
        {
            dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(2f);

        fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(0, 0.5f);
    }
}
