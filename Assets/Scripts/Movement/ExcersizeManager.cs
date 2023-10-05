using TMPro;
using Unity.XR.PXR;
using UnityEngine;
using DevDunk.XR;
using DevDunk.Movement.Utilities;

namespace DevDunk.Movement
{
    public class ExcersizeManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text poseText;

        BodyTrackerResult activePose;
        float totalDistance, totalRotation;
        bool trackingActive;

        private void Update()
        {
            if (!trackingActive) return;

            totalDistance = totalRotation = 0;

            for (int i = 0; i < activePose.trackingdata.Length; i++)
            {
                var currentPose = BodyTrackingManager.Instance.BodyTrackerResult.trackingdata[i].localpose;
                var activePose = this.activePose.trackingdata[i].localpose;

                Vector3 currentPos = new Vector3((float)currentPose.PosX, (float)currentPose.PosY, (float)currentPose.PosZ);
                Quaternion currentRot = new Quaternion((float)currentPose.RotQx, (float)currentPose.RotQy, (float)currentPose.RotQz, (float)currentPose.RotQw);
                Vector3 activePos = new Vector3((float)activePose.PosX, (float)activePose.PosY, (float)activePose.PosZ);
                Quaternion activeRot = new Quaternion((float)activePose.RotQx, (float)activePose.RotQy, (float)activePose.RotQz, (float)activePose.RotQw);

                totalDistance += Vector3.Distance(currentPos, activePos);
                totalRotation += Quaternion.Angle(currentRot, activeRot);
            }
            poseText.text = $"Distance: {totalDistance}\nRotation: {totalRotation}";
        }

        public void SaveCurrentPose()
        {
            JSONTools.V3SaveIntoJson(transform.position);

            activePose = BodyTrackingManager.Instance.BodyTrackerResult;
            trackingActive = true;

            JSONTools.SaveIntoJson(activePose);
        }
    }
}