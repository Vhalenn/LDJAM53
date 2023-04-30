using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AIManager aiManager;

    [Header("Goal")]
    [SerializeField] private int actualGoalIndex;
    [SerializeField] private Goal[] goalArray;

    // TUTORIAL MESSAGES

    [Header("Storage")]
    private bool gameSuccess;
    private Goal actualGoal;

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
            PlacePlayerOn(nextAnimal);
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

    private void CheckGoal()
    {
        actualGoal = goalArray[actualGoalIndex];

        if (actualGoal != null)
        {
            if (actualGoal.Success())
            {
                actualGoalIndex++;

                // Update UI Infos
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
        goalArray[index].reachedLocationGoal = true;
    }
}
