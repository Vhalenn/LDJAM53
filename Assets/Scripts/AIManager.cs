using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] Mesh cursorMesh;
    [SerializeField] Material cursorMaterial;

    [Header("Controlled")]
    [SerializeField] private Animal[] selectedAgents;

    [Header("Storage")]
    [SerializeField] private Animal[] allAgents;
    [SerializeField] private Base[] allBases;
    [SerializeField] private Matrix4x4[] selectedAgentMatrices;

    void Start()
    {
        allBases = GetComponentsInChildren<Base>();
        allAgents = GetComponentsInChildren<Animal>();
        SetSelection(allAgents);

        for (int i = 0; i < allAgents.Length; i++)
        {
            if (allAgents[i] == null) continue;

            Base appropriatedBase = FindBase(allAgents[i].Type);
            allAgents[i].Init(appropriatedBase);
        }
    }

    private void SetSelection(Animal[] array)
    {
        selectedAgents = array; // TEMP
    }

    private void Update()
    {
        selectedAgentMatrices = new Matrix4x4[selectedAgents.Length];
        for (int i = 0; i < selectedAgents.Length; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgentMatrices[i] = selectedAgents[i].transform.localToWorldMatrix;
        }

        Graphics.DrawMeshInstanced(cursorMesh, 0, cursorMaterial, selectedAgentMatrices);
    }

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

    public void MoveAgentsTo(Ressource ressource)
    {
        for (int i = 0; i < selectedAgents.Length; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgents[i].SetRessourceToGather(ressource);
        }
    }

    public void MoveAgentsTo(Vector3 pos)
    {
        for (int i = 0; i < selectedAgents.Length; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgents[i].SetDestination(pos, randomizePos:true);
        }
    }

}
