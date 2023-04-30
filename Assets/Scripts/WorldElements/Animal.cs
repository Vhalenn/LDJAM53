using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform ditheringPattern;
    [SerializeField] private bool visible;
    public bool Visible { get => visible; set => visible = value; }
    [SerializeField] private AnimalType type;
    public AnimalType Type => type;

    [Header("Selected")]
    [SerializeField] private bool isSelected;
    [SerializeField] private bool hasBeenSelected;

    [Header("Storage")]
    [SerializeField] private AIManager aiController;
    [SerializeField] private NavMeshPath path;
    [SerializeField] private bool isOnOffMeshLink;
    [SerializeField] private Color gizmoColor;

    [Header("Gathering Storage")]
    [SerializeField] private Ressource gatheringObject;
    [SerializeField] private Base baseObject;
    [SerializeField] private bool goingToBase;
    [SerializeField] private int amountOfFood;

    public void Init(Base baseObject)
    {
        this.baseObject = baseObject;

        if (ditheringPattern)
        {
            ditheringPattern.localRotation = Quaternion.Euler(new Vector3(0, Random.value * 360, 0));
            ditheringPattern.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        isOnOffMeshLink = agent.isOnOffMeshLink;

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

    private void GatheringCheck()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) // Reached Destination
        {
            if (gatheringObject != null)
            {
                if (goingToBase) // Reached base
                {
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
                            goingToBase = false;
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
                    goingToBase = true;
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

        gizmoColor = gatheringRessource ? Color.yellow : Color.green;

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
        goingToBase = false;
        SetDestination(ressourceObject.transform.position, gatheringRessource:true);
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
            Gizmos.color = gizmoColor;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }

}
