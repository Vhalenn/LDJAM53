using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    [SerializeField] private GameObject toHide;

    void OnEnable()
    {
        toHide.SetActive(false);
    }

}
