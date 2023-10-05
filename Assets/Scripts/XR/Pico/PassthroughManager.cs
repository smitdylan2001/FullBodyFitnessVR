using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.UIElements;

namespace DevDunk.XR
{
    public class PassthroughManager : MonoBehaviour
    {
        // Enable seethrough
        void Awake()
        {
            PXR_MixedReality.EnableVideoSeeThrough(true);
            PXR_Manager.SpatialTrackingStateUpdate += PXR_Manager_SpatialTrackingStateUpdate;

            //PXR_MixedReality.CreateAnchorEntity(Vector3 position, Quaternion rotation, out ulong taskId);
        }



        // Re-enable seethrough after the app resumes
        void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                PXR_MixedReality.EnableVideoSeeThrough(true);
            }
        }

        private void PXR_Manager_SpatialTrackingStateUpdate(PxrEventSpatialTrackingStateUpdate obj)
        {
            if (obj.state != PxrSpatialTrackingState.Valid)
            {
                Debug.LogWarning("Pico tracking state is " + obj.state);
                return;
            }
        }
    }
}