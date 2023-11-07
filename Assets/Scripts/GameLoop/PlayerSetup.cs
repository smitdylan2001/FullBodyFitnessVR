using DevDunk.Tools;
using System.Collections;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.InputSystem.InputAction;

namespace DevDunk.GameSystems
{
    public class PlayerSetup : MonoBehaviour
    {
        public Transform PointTransform, HostVideoObject, MirroredPosition;
        public PXR_Hand HandInteractions;
        public bool firstStage = true, useHandTracking;

        public float interactioDelay = 3, videoHeight = 1, personHeight = 1;

        public GameObject WorkoutObject;

        bool interactionEnabled, initiatedButton, doneInteraction;
        float triggerValue;
        Transform camTransform;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            camTransform = Camera.main.transform;

            yield return new WaitForSeconds(interactioDelay);

            interactionEnabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (interactionEnabled && triggerValue >= 0.5f && Physics.Raycast(PointTransform.position, PointTransform.forward, out RaycastHit rch))
            {
                var button = rch.transform.GetComponent<CollideEvent>();
                if (!doneInteraction && button)
                {
                    button.Event.Invoke();
                    doneInteraction = true;
                    return;
                }
                else
                {
                    return;
                    Vector3 pos = rch.point + (firstStage ? new Vector3(0, videoHeight, 0) : new Vector3(0, personHeight, 0));
                    Quaternion rot = Quaternion.LookRotation(pos - camTransform.position);
                    if (firstStage) HostVideoObject.SetPositionAndRotation(pos, rot);
                    else MirroredPosition.SetPositionAndRotation(pos, rot);
                }
            }else if (triggerValue <= 0.5f)
            {
                doneInteraction = false;
            }
        }

        [ContextMenu("GoToNextStage")]
        public void GoToNextStage()
        {
            StartWorkout(GameObject.Find("ContinueButton"));
            return;

            if (firstStage) firstStage = false;
            else StartWorkout(GameObject.Find("ContinueButton"));
        }

        public void GoToNextStage(GameObject button)
        {
            StartWorkout(button);
            return;

            if (firstStage) firstStage = false;
            else StartWorkout(button);
        }

        void StartWorkout(GameObject button)
        {
            GameLoop.StartGamePlay = true;
            PointTransform.gameObject.SetActive(false);
            this.enabled = false;
            interactionEnabled = false;

            if (button) button.SetActive(false);
        }

        public void TriggerInput(CallbackContext context)
        {
            triggerValue = context.ReadValue<float>();
        }
    }
}