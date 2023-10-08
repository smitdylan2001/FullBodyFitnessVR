using System.Collections;
using Unity.XR.PXR;
using UnityEngine;

namespace DevDunk.XR
{
    public class BodyTrackingManager : MonoBehaviour
    {
        public static BodyTrackingManager Instance { get; private set; }

        public Transform[] BodyParts;
        public BodyTrackerResult BodyTrackerResult;
        public Quaternion RotationOffset, RotationOffsetDebug;
        Quaternion[] startPos;
        bool startTracking;

        GameObject[] joints;

        WaitForSeconds WaitOneSecond = new WaitForSeconds(1);

        private void Awake()
        {
            Instance = this;

            RotationOffset = Quaternion.identity;

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Material material = go.GetComponent<Renderer>().material;
            material.color = Color.red;

            joints = new GameObject[BodyParts.Length];
            for (int i = 0; i < joints.Length; i++)
            {
                joints[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                joints[i].transform.localScale = Vector3.one/10;

                joints[i].GetComponent<Renderer>().sharedMaterial = material;
            }

            startPos = new Quaternion[BodyParts.Length];

            for (int i = 0; i < startPos.Length; i++)
            {
                startPos[i] = BodyParts[i].rotation;
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                // Set the full-body tracking mode
                PXR_Input.SetSwiftMode(1);
            }
        }

        private IEnumerator Start()
        {
            PXR_Plugin.System.FitnessBandNumberOfConnections += FitnessBandNumberOfConnectionsFuction;
            PXR_Plugin.System.FitnessBandAbnormalCalibrationData += FitnessBandAbnormalCalibrationDataFuction;
            PXR_Plugin.System.FitnessBandElectricQuantity += FitnessBandElectricQuantityFuction;

            // Get the number of motion trackers connected and their IDs
            PxrFitnessBandConnectState bandConnectState = new PxrFitnessBandConnectState();
            PXR_Input.GetFitnessBandConnectState(ref bandConnectState);

            // Detect if the motion tracker has completed calibration (0: uncompleted; 1: completed)
            int calibrated = 0;
            var value = PXR_Input.GetFitnessBandCalibState(ref calibrated);

            // If not calibrated, launch the "PICO Motion Tracker" app for calibration
            if (calibrated == 0)
            {
                PXR_Input.OpenFitnessBandCalibrationAPP();
            }
            while (calibrated == 0)
            {
                yield return WaitOneSecond;
                value = PXR_Input.GetFitnessBandCalibState(ref calibrated);
                Debug.Log(value + " " + calibrated + " not calibrated");
            }
            Debug.Log(value + " " + calibrated + " calibrated");
            // Gets the motion tracker's bettery, value range: [0,5]
            //int battery = 0;
            //PXR_Input.GetFitnessBandBattery(bandConnectState.trackerID[0], ref battery);



            // Set bone length
            BodyTrackingBoneLength boneLength = new BodyTrackingBoneLength();
            boneLength.headLen = 26.1f;
            boneLength.neckLen = 6.1f;
            boneLength.torsoLen = 37.1f;
            boneLength.hipLen = 9.1f;
            boneLength.upperLegLen = 34.1f;
            boneLength.lowerLegLen = 40.1f;
            boneLength.footLen = 14.1f;
            boneLength.shoulderLen = 27.1f;
            boneLength.upperArmLen = 20.1f;
            boneLength.lowerArmLen = 22.1f;
            boneLength.handLen = 21.1f;
            int result = PXR_Input.SetBodyTrackingBoneLength(boneLength);

            // Get the position data of each body joint
            BodyTrackerResult bodyTrackerResult = new BodyTrackerResult();
            PXR_Input.GetBodyTrackingPose(0, ref bodyTrackerResult);

            startTracking = true;
        }

        private void Update()
        {
            if (!startTracking) return;
            GetBodyPose();

            return;
            UpdateBodyPositions();
        }

        void GetBodyPose()
        {
            // Get the position data of each body joint
            BodyTrackerResult = new BodyTrackerResult();
            var status = PXR_Input.GetBodyTrackingPose(0, ref BodyTrackerResult);
        }

        void UpdateBodyPositions()
        {
            for (int i = 0; i < BodyTrackerResult.trackingdata.Length; i++)
            {
                BodyTrackerTransform pose = BodyTrackerResult.trackingdata[i];
                var pos = new Vector3((float)pose.localpose.PosX, (float)pose.localpose.PosY, (float)pose.localpose.PosZ);
                var rot = new Quaternion((float)pose.localpose.RotQx, (float)pose.localpose.RotQy, (float)pose.localpose.RotQz, (float)pose.localpose.RotQw) * startPos[i];

                BodyParts[i].SetPositionAndRotation(pos, rot);

                joints[i].transform.SetPositionAndRotation(pos, rot);
            }
        }

        public void OnDestroy()
        {
            PXR_Plugin.System.FitnessBandNumberOfConnections -= FitnessBandNumberOfConnectionsFuction;
            PXR_Plugin.System.FitnessBandAbnormalCalibrationData -= FitnessBandAbnormalCalibrationDataFuction;
            PXR_Plugin.System.FitnessBandElectricQuantity -= FitnessBandElectricQuantityFuction;
        }

        public void AddRotationOffset()
        {
            RotationOffset *= RotationOffsetDebug;
        }

        private void FitnessBandNumberOfConnectionsFuction(int state, int value)
        {
            Debug.Log("ConFunction " + state + " " + value);
        }

        private void FitnessBandElectricQuantityFuction(int trackerID, int battery)
        {
            Debug.Log("QuanFunction " + trackerID + " " + battery);
        }

        private void FitnessBandAbnormalCalibrationDataFuction(int state, int value)
        {
            Debug.Log("AbnFunction " + state + " " +value);
        }
    }
}