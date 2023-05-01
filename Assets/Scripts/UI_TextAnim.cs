using UnityEngine;
using TMPro;

public class UI_TextAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int maxDisplay;

    void OnValidate()
    {
        if(text)
        {
            text.maxVisibleCharacters = maxDisplay;
        }
    }

}
