using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private Animal[] allAgents;

    [Header("Controlled")]
    [SerializeField] private Animal[] selectedAgents;

    void Start()
    {
        allAgents = GetComponentsInChildren<Animal>();
        selectedAgents = allAgents;
    }

    public void MoveAgentsTo(Vector3 pos)
    {
        for (int i = 0; i < selectedAgents.Length; i++)
        {
            if (selectedAgents[i] == null) continue;

            selectedAgents[i].SetDestination(pos);
        }
    }

}
