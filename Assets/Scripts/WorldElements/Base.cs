using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private AnimalType type;
    public AnimalType Type => type;

    [SerializeField] private int numberOfFood;
    public int NumberOfFood
    {
        get => numberOfFood;
        set => numberOfFood = value;
    }

    [Header("Elements")]
    [SerializeField] private Transform[] entrance;
    public Transform Entrance => entrance[Random.Range(0,entrance.Length)];

    [Header("Storage")]
    [SerializeField] private AIManager aiManager;

    public void Init(AIManager aiManager)
    {
        this.aiManager = aiManager;
    }

    public void DeliverFood(int amount)
    {
        numberOfFood += amount;

        if (aiManager)
        {
            aiManager.RefreshFoodGoal();
        }
    }
}
