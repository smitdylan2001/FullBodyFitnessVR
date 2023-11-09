using TMPro;
using Unity.XR.PXR;
using UnityEngine;
using DevDunk.XR;
using DevDunk.Movement.Utilities;

namespace DevDunk.Movement
{
    public class ExcersizeManager : MonoBehaviour
    {
        public static ExcersizeManager Instance { get; private set; }

        public float TotalDistance, TotalRotation;
        [SerializeField] private TMP_Text poseText;

        BodyTrackerResult activePose, TPose;
        bool trackingActive;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!trackingActive) return;

            TotalDistance = TotalRotation = 0;

            for (int i = 0; i < activePose.trackingdata.Length; i++)
            {
                if (i < BodyTrackingManager.Instance.BodyTrackerResult.trackingdata.Length)
                {
                    var currentPose = BodyTrackingManager.Instance.BodyTrackerResult.trackingdata[i].localpose;
                    var activePose = this.activePose.trackingdata[i].localpose;

                    Vector3 currentPos = new Vector3((float)currentPose.PosX, (float)currentPose.PosY, (float)currentPose.PosZ);
                    Quaternion currentRot = new Quaternion((float)currentPose.RotQx, (float)currentPose.RotQy, (float)currentPose.RotQz, (float)currentPose.RotQw);
                    Vector3 activePos = new Vector3((float)activePose.PosX, (float)activePose.PosY, (float)activePose.PosZ);
                    Quaternion activeRot = new Quaternion((float)activePose.RotQx, (float)activePose.RotQy, (float)activePose.RotQz, (float)activePose.RotQw);

                    TotalDistance += Vector3.Distance(currentPos, activePos);
                    TotalRotation += Quaternion.Angle(currentRot, activeRot);
                }
            }
            //poseText.text = $"Distance: {TotalDistance}\nRotation: {TotalRotation}";
        }

        public void SetActivePose(BodyTrackerResult pose)
        {
            activePose = pose;
            trackingActive = true;
        }


        double time;
        public void SaveCurrentPose()
        {
            return;
            if (Time.timeAsDouble - time < 1) return;

            activePose = BodyTrackingManager.Instance.BodyTrackerResult;
            trackingActive = true;

            JSONTools.SaveIntoJson(activePose);

            time = Time.timeAsDouble;
        }
    }
}