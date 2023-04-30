using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] Mesh cursorMesh;
    [SerializeField] Material cursorMaterial;

    [Header("Controlled")]
    [SerializeField] private List<Animal> selectedAgents;

    [Header("Storage")]
    [SerializeField] private Animal[] allAgents;
    [SerializeField] private Base[] allBases;
    [SerializeField] private Matrix4x4[] selectedAgentMatrices;

    void Awake()
    {
        allBases = GetComponentsInChildren<Base>();
        allAgents = GetComponentsInChildren<Animal>();

        for (int i = 0; i < allAgents.Length; i++)
        {
            if (allAgents[i] == null) continue;

            Base appropriatedBase = FindBase(allAgents[i].Type);
            allAgents[i].Init(appropriatedBase);
        }

        ClearSelection();
        Select(allAgents[0]);
    }

    private void Update()
    {
        selectedAgentMatrices = new Matrix4x4[selectedAgents.Count];
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgentMatrices[i] = selectedAgents[i].transform.localToWorldMatrix;
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

    public Animal GetSelected()
    {
        if (selectedAgents == null) return null;
        else return selectedAgents[0];
    }

    // MOVEMENT
    public void MoveAgentsTo(Ressource ressource)
    {
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgents[i].SetRessourceToGather(ressource);
        }
    }

    public void MoveAgentsTo(Vector3 pos)
    {
        for (int i = 0; i < selectedAgents.Count; i++)
        {
            if (selectedAgents[i] == null) continue;

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
        if(selectedAgents != null)
        {
            for(int i = 0; i < selectedAgents.Count; i++)
            {
                if (selectedAgents[i]) selectedAgents[i].Unselect();
            }
        }

        selectedAgents = new List<Animal>();
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
    }

}
