using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public bool triggerOnce;
    private bool hasTriggered;
    private bool hasExited;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnce && hasTriggered)
            {
                return;
            }
            hasTriggered = true;
            OnPlayerEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerOnce && hasExited)
            {
                return;
            }
            hasExited = true;
            OnPlayerExit.Invoke();
        }
    }
}
