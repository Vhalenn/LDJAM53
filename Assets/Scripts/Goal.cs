using UnityEngine;

[System.Serializable]
public class Goal
{
    public string name;
    public string description;
    public GoalTrigger pointToReach;
    public int foodAmount;

    [Header("Record")]
    public bool reachedLocationGoal;
    public bool reachedFoodGoal;

    public bool Success()
    {
        // Destination
        bool locationGoal = pointToReach == null || reachedLocationGoal;

        // Food
        bool foodGoal = foodAmount <= 0 || reachedFoodGoal;

        return locationGoal && foodGoal;
    }
}
