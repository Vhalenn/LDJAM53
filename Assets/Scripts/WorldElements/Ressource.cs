using UnityEngine;

public enum RessourceType
{
    Apples,
}

public class Ressource : MonoBehaviour
{
    [SerializeField] private RessourceType type;
    [SerializeField] private Vector2Int gatheredAmount = new Vector2Int(3, 7);
    [SerializeField] private int maxQuantity = 50;

    [Header("Elements")]
    [SerializeField] private GameObject model;
    [SerializeField] private SphereCollider hitbox;

    [Header("Storage")]
    [SerializeField] private int actualQuantity = 50;

    public bool AvailableRessource { get => actualQuantity > 0; }

    public RessourceType GetRessource(out int amount)
    {
        amount = Random.Range(gatheredAmount.x, gatheredAmount.y);

        amount = Mathf.Min(amount, actualQuantity);
        actualQuantity -= amount;

        if(!AvailableRessource)
        {
            model.SetActive(false);
            hitbox.enabled = false;
        }

        return type;
    }
}
