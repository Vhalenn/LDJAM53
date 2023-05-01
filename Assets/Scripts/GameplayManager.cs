using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AIManager aiManager;

    [Header("Goal")]
    [SerializeField] private int actualGoalIndex;
    [SerializeField] private Goal[] goalArray;
    [SerializeField] private int foodOwned;

    // TUTORIAL MESSAGES

    [Header("Storage")]
    private bool gameSuccess;
    private Goal actualGoal;

    private void Start()
    {
        UpdateGoal();
    }

    private void Update()
    {
        if (gameSuccess) return;

        if(actualGoalIndex >= goalArray.Length)
        {
            // GAME SUCCESS !!!
            Success();
            return;
        }

        CheckGoal();
        CheckShortcuts();
    }

    private void CheckShortcuts()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Animal nextAnimal = aiManager.GetNextAgentAlreadySelected();

            if(nextAnimal != null)
            {
                aiManager.ClearSelection();
                aiManager.Select(nextAnimal);
                PlacePlayerOn(nextAnimal);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlacePlayerOnSelection();
        }
    }

    public void PlacePlayerOnSelection()
    {
        PlacePlayerOn(aiManager.GetSelected());
    }
    private void PlacePlayerOn(Animal selectedAnimal)
    {
        if (selectedAnimal)
        {
            playerController.transform.position = selectedAnimal.transform.position;
        }
    }

    // GOAL
    private void Success()
    {
        gameSuccess = true;
    }
    public void UpdateFoodOwned(int foodOwned)
    {
        this.foodOwned = foodOwned;

        if (foodOwned > actualGoal.foodAmount)
        {
            actualGoal.ReachedFoodGoal();
            CheckGoal();
        }

        UpdateGoal();
    }

    private void UpdateGoal()
    {
        actualGoal = goalArray[actualGoalIndex];

        // Update UI Infos
        gameManager.UIManager.UpdateGoalText(actualGoal.GetGoalText(foodOwned));
    }

    private void CheckGoal()
    {
        if(actualGoalIndex >= goalArray.Length)
        {
            Success();
            return;
        }

        actualGoal = goalArray[actualGoalIndex];

        if (actualGoal != null)
        {
            if (actualGoal.Success())
            {
                actualGoal.SuccessEvent();

                // RESET
                if(actualGoal.foodAmount > 0)
                {
                    aiManager.ResetAllBases();
                }

                // GO TO NEW GOAL
                Debug.Log($"<color=green>Goal {actualGoalIndex} is a success !</color>");
                gameManager.UIManager.GoalSuccess();
                actualGoalIndex++;

                // CHANGE THE GOAL
                if (actualGoalIndex >= goalArray.Length)
                {
                    Success();
                    return;
                }
                UpdateGoal();
            }

        }
        else
        {
            actualGoalIndex++;
        }
    }

    // CALLED BY TRIGGER EVENT
    public void ValidateGoalPos(int index)
    {
        goalArray[index].ReachedLocation();

        if(actualGoal == null) actualGoal = goalArray[actualGoalIndex];
        gameManager.UIManager.UpdateGoalText(actualGoal.GetGoalText(foodOwned));
    }

    public void DialogEvent(string text)
    {
        gameManager.UIManager.ShowUIDialog(text);
    }
}
