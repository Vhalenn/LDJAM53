using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        Debug.Log($"<color=green>Triggered {transform.name} by {other.name} </color>");

        triggered = true;
        if (_event != null) _event.Invoke();
    }

}
