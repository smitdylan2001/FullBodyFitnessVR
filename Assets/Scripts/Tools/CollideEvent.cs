using TagSelector;
using UnityEngine;
using UnityEngine.Events;

namespace DevDunk.Tools
{
    public class CollideEvent : MonoBehaviour
    {
        [TagSelector] public string requiredTag;
        public UnityEvent Event;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.collider.tag);
            if(requiredTag.Length == 0 || collision.collider.CompareTag(requiredTag))
            {
                Event.Invoke();
            }
        }
    }
}