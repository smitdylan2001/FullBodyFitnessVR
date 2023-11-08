using TagSelector;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [TagSelector] public string requiredTag;
    public UnityEvent Event;

    private void OnTriggerEnter(Collider collision)
    {
        if (requiredTag.Length == 0 || collision.CompareTag(requiredTag))
        {
            Event.Invoke();
        }
    }
}
