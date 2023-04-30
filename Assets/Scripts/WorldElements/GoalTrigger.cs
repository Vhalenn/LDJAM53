using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;
    [SerializeField] private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
        if (_event != null) _event.Invoke();
    }

}
