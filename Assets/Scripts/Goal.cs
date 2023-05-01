using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Goal
{
    public string name;
    public string description;
    public GoalTrigger pointToReach;
    public int foodAmount;
    [SerializeField] private UnityEvent successEvent;

    [Header("Record")]
    [SerializeField] private bool reachedLocationGoal;
    [SerializeField] private bool reachedFoodGoal;

    public bool locationSuccess => pointToReach == null || reachedLocationGoal;
    public bool foodSuccess => foodAmount <= 0 || reachedFoodGoal;

    private const string SUCCESS_START = "<color=#466b3f><s>";
    private const string SUCCESS_END = "</s></color>";

    public bool Success()
    {
        return locationSuccess && foodSuccess;
    }

    public void ReachedLocation()
    {
        reachedLocationGoal = true;
    }

    public void ReachedFoodGoal()
    {
        reachedFoodGoal = true;
    }

    public string GetGoalText(int foodOwned)
    {
        string goalText = string.Empty;
        //goalText += description + "\n";

        goalText += "-" + (locationSuccess ? SUCCESS_START : string.Empty) + description + (locationSuccess ? SUCCESS_END : string.Empty) + "\n";

        if (foodAmount > 0)
        {
            int remainingFood = Mathf.Max(0, foodAmount - foodOwned);
            goalText += "-" + (foodSuccess ? SUCCESS_START : string.Empty) + $"Gather {remainingFood} apples" + (foodSuccess ? SUCCESS_END : string.Empty);
        }

        return goalText;
    }
}
