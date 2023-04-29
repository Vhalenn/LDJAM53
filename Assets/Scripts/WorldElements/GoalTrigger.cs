using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _event;

    private void OnTriggerEnter(Collider other)
    {
        if(_event != null) _event.Invoke();
    }

}
