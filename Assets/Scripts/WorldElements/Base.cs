using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private AnimalType type;
    public AnimalType Type => type;

    [SerializeField] private int numberOfFood;

    [Header("Elements")]
    [SerializeField] private Transform[] entrance;
    public Transform Entrance => entrance[Random.Range(0,entrance.Length)];

    public void DeliverFood(int amount)
    {
        numberOfFood += amount;
    }
}
