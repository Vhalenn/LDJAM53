using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] GameplayManager gameplayManager;

    [Header("Cursor")]
    [SerializeField] Mesh cursorMesh;
    [SerializeField] Material cursorMaterial;

    [Header("Controlled")]
    [SerializeField] private List<Animal> selectedAgents;
    [SerializeField] private Ressource selectedRessource;

    [Header("Start cutscene")]
    [SerializeField] private Animal pregnantHorse;
    [SerializeField] private GameObject pregnantHorseResting;

    [Header("Storage")]
    [SerializeField] private bool hasMovedHorse;
    public bool HasMovedHorse => hasMovedHorse;

    [SerializeField] private int swapIndexTab = 0;
    [SerializeField] private Animal[] allAgents;
    [SerializeField] private Base[] allBases;
    [SerializeField] private Matrix4x4[] selectedAgentMatrices;

    void Awake()
    {
        hasMovedHorse = false;
        allBases = GetComponentsInChildren<Base>();
        allAgents = GetComponentsInChildren<Animal>();

        for (int i = 0; i < allAgents.Length; i++)
        {
            if (allAgents[i] == null) continue;
            allAgents[i].Init(this);
        }

        for (int i = 0; i < allBases.Length; i++)
        {
            if (allBases[i] == null) continue;
            allBases[i].Init(this);
        }

        ClearSelection();
        Select(allAgents[0]);


        if (pregnantHorseResting)
        {
            pregnantHorseResting.SetActive(false);
        }
    }

    public void RemoveControlOfSummer()
    {
        if(selectedAgents.Contains(pregnantHorse))
        {
            selectedAgents.Remove(pregnantHorse);
        }

        if (pregnantHorse)
        {
            pregnantHorse.Disable();
        }

        if(pregnantHorseResting)
        {
            pregnantHorseResting.SetActive(true);
        }
    }

    private void Update()
    {
        int count = selectedAgents.Count;
        if (selectedRessource) count += 1;

        selectedAgentMatrices = new Matrix4x4[count];
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgentMatrices[i] = selectedAgents[i].transform.localToWorldMatrix;
        }

        // Select the ressource also
        if (selectedRessource)
        {
            selectedAgentMatrices[count - 1] = selectedRessource.transform.localToWorldMatrix;
        }

        Graphics.DrawMeshInstanced(cursorMesh, 0, cursorMaterial, selectedAgentMatrices);
    }

    // GET
    private Base FindBase(AnimalType type)
    {
        for (int i = 0; i < allBases.Length; i++)
        {
            if (allBases[i] == null) continue;

            if (allBases[i].Type == type) return allBases[i];
        }

        // IF FAILED
        Debug.LogError($"Couldn't find the base of type {type}");
        return null;
    }

    public Base GetClosestBase(Vector3 pos)
    {
        float distance = 0;
        float minDistance = 99999;
        Base selectedBase = allBases[0];

        for (int i = 0; i < allBases.Length; i++)
        {
            if (allBases[i] == null) continue;

            distance = Vector3.Distance(pos, allBases[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                selectedBase = allBases[i];
            }
        }

        return selectedBase;
    }

    public void RefreshFoodGoal()
    {
        gameplayManager.UpdateFoodOwned(GetFoodInBases());
    }

    public int GetFoodInBases()
    {
        int amount = 0;

        for (int i = 0; i < allBases.Length; i++)
        {
            amount += allBases[i].NumberOfFood;
        }

        return amount;
    }

    public void ResetAllBases()
    {
        for (int i = 0; i < allBases.Length; i++)
        {
            allBases[i].NumberOfFood = 0;
        }
    }

    public Animal GetNextAgentAlreadySelected(bool tryFromZero = false)
    {
        swapIndexTab++;
        if(swapIndexTab >= allAgents.Length && !tryFromZero)
        {
            return GetNextAgentAlreadySelected(true);
        }

        for (int i = tryFromZero ? 0 : swapIndexTab; i < allAgents.Length; i++)
        {
            swapIndexTab = i;

            if (allAgents[i].HasBeenSelected && allAgents[i].isActiveAndEnabled)
            {
                return allAgents[i];
            }
        }

        if (!tryFromZero) return GetNextAgentAlreadySelected(true);
        else return null;
    }

    public Animal GetSelected()
    {
        if (selectedAgents == null) return null;
        else return selectedAgents[Random.Range(0,selectedAgents.Count)];
    }

    // MOVEMENT
    public void MoveAgentsTo(Ressource ressource)
    {
        selectedRessource = ressource;
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgents[i].SetRessourceToGather(ressource);
        }
    }

    public void MoveAgentsTo(Vector3 pos)
    {
        hasMovedHorse = true;

        selectedRessource = null;
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null || !selectedAgents[i].isActiveAndEnabled) continue;

            selectedAgents[i].SetDestination(pos, randomizePos:true);
        }
    }

    // SELECTION
    private void SetSelection(Animal[] array)
    {
        ClearSelection();
        selectedAgents.AddRange(array); // TEMP
    }

    public void ClearSelection()
    {
        if (selectedAgents != null)
        {
            for(int i = 0; i < selectedAgents.Count; i++)
            {
                if (selectedAgents[i]) selectedAgents[i].Unselect();
            }
        }

        selectedRessource = null;
        selectedAgents = new List<Animal>();


        gameplayManager.GameManager.UIManager.SetSelectionNames(string.Empty);
    }

    public void Select(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Animal animal))
        {
            Select(animal);
        }

    }

    public void Select(Animal animal)
    {
        animal.Select();

        if(!selectedAgents.Contains(animal))
        {
            selectedAgents.Add(animal);
        }

        string selectionNames = string.Empty;
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (i > 0) selectionNames += ", ";
            selectionNames += selectedAgents[i].AgentName;
        }
        gameplayManager.GameManager.UIManager.SetSelectionNames(selectionNames);
    }

}
