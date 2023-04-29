using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool visible;
    public bool Visible { get => visible; set => visible = value; }

    [Header("Storage")]
    [SerializeField] private NavMeshPath path;
    [SerializeField] private bool isOnOffMeshLink;

    private void Update()
    {
        isOnOffMeshLink = agent.isOnOffMeshLink;
    }

    public void SetDestination(Vector3 pos)
    {
        agent.SetDestination(pos);
        StartCoroutine(DrawPath());
    }

    private IEnumerator DrawPath()
    {
        path = new NavMeshPath();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        path = agent.path;
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }

}
