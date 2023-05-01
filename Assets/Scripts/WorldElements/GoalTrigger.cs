using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private bool triggered;

    [Header("Multiple agents")]
    [SerializeField] private int count;
    [SerializeField] private int mininmumCount = 0;
    [SerializeField] TextMeshPro countText;

    private void Start()
    {
        if(countText && mininmumCount > 0)
        {
            UpdateCount(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.transform.TryGetComponent(out Animal agent))
        {
            if (!AgentIsValid(agent)) return;

            UpdateCount(count + 1);

            if(count >= mininmumCount)
            {
                Trigger(other.name);
            }
        }
    }

    private void UpdateCount(int newValue)
    {
        count = Mathf.Max(0,newValue);

        if(countText)
        {
            countText.transform.rotation = Quaternion.Euler(90, 0, 0);
            countText.text = $"{newValue}/{mininmumCount}";
        }
    }

    private void Trigger(string name)
    {
        Debug.Log($"<color=green>Triggered {transform.name} by {name} </color>");

        triggered = true;
        if (_event != null) _event.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggered) return;

        if (other.transform.TryGetComponent(out Animal agent))
        {
            if (!AgentIsValid(agent)) return;

            UpdateCount(count - 1);
        }
    }

    private bool AgentIsValid(Animal agent)
    {
        return agent.HasBeenSelected && agent.Type == AnimalType.Horse;
    }

}
