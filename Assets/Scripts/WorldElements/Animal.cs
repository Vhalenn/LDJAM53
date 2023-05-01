using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] private string agentName;
    public string AgentName => agentName;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform ditheringPattern;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AnimalType type;
    public AnimalType Type => type;
    [SerializeField] private Color gatherColor, moveColor;

    [Header("Selected")]
    [SerializeField] private bool isSelected;
    [SerializeField] private bool hasBeenSelected;
    public bool HasBeenSelected => hasBeenSelected;

    [Header("Storage")]
    [SerializeField] private AIManager aiManager;
    [SerializeField] private NavMeshPath path;
    [SerializeField] private bool isOnOffMeshLink;
    [SerializeField] private Color gizmoColor;

    [Header("Gathering Storage")]
    [SerializeField] private Ressource gatheringObject;
    [SerializeField] private bool goingToBase => amountOfFood > 0;
    [SerializeField] private int amountOfFood;

    public void Init(AIManager aiManager)
    {
        this.aiManager = aiManager;

        if(animator)
        {
            animator.SetFloat("offset", Random.value);
        }

        if (ditheringPattern)
        {
            ditheringPattern.localRotation = Quaternion.Euler(new Vector3(0, Random.value * 360, 0));
            ditheringPattern.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        isOnOffMeshLink = agent.isOnOffMeshLink;

        if (animator && agent)
        {
            animator.SetFloat("speed", agent.velocity.magnitude);
        }

        if(audioSource && agent)
        {
            audioSource.volume = Mathf.Clamp01(agent.velocity.magnitude);
        }

        GatheringCheck();
    }
    public void Select()
    {
        hasBeenSelected = true;
        isSelected = true;
        ditheringPattern.gameObject.SetActive(true);
    }
    public void Unselect()
    {
        isSelected = false;
        hasBeenSelected = true;
    }

    public void Disable()
    {
        agent.enabled = false;
        gameObject.SetActive(false);
    }

    private void GatheringCheck()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) // Reached Destination
        {
            if (gatheringObject != null)
            {
                if (goingToBase) // Reached base
                {
                    Base baseObject = aiManager.GetClosestBase(transform.position);
                    // Drop ressources in base
                    if (baseObject)
                    {
                        baseObject.DeliverFood(amountOfFood);
                        amountOfFood = 0;
                    }

                    // Go back to ressource
                    if (gatheringObject)
                    {
                        if (gatheringObject.AvailableRessource)
                        {
                            SetRessourceToGather(gatheringObject);
                        }
                        else
                        {
                            gatheringObject = null;
                        }
                    }
                }
                else // Reached ressource
                {
                    // Get the ressources from the base
                    if (gatheringObject && gatheringObject.AvailableRessource)
                    {
                        gatheringObject.GetRessource(out amountOfFood);
                        if(amountOfFood == 0) gatheringObject = null;
                    }

                    // Go to base
                    Base baseObject = aiManager.GetClosestBase(transform.position);
                    if (baseObject) SetDestination(baseObject.Entrance.position, true);
                }
            }
        }
    }

    public void SetDestination(Vector3 pos, bool gatheringRessource = false, bool randomizePos = false)
    {
        if(!gatheringRessource)
        {
            this.gatheringObject = null;
        }

        if (randomizePos)
        {
            pos += Random.insideUnitSphere * agent.stoppingDistance * 0.5f;
        }

        gizmoColor = gatheringRessource ? gatherColor : moveColor;

        if (agent)
        {
            agent.SetDestination(pos);
            StartCoroutine(DrawPath());
        }
    }

    public void SetRessourceToGather(Ressource ressourceObject)
    {
        if( ressourceObject == null ||
            (goingToBase == true && ressourceObject == gatheringObject) || 
            !ressourceObject.AvailableRessource)
        {
            return;
        }

        gatheringObject = ressourceObject;
        SetDestination(ressourceObject.transform.position, gatheringRessource:true);
    }

    private IEnumerator DrawPath()
    {
        path = new NavMeshPath();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        path = agent.path;

        if(path != null && path.corners != null)
        {
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            line.endColor = gizmoColor;
            line.startColor = gizmoColor;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = gizmoColor;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }

}
