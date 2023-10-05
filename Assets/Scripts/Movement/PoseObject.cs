using DevDunk.Movement.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.XR.PXR;
using UnityEngine;

namespace DevDunk.Movement
{
    [CreateAssetMenu(fileName = "Pose", menuName = "Pose", order = 1)]
    public class PoseObject : ScriptableObject
    {
        public string PoseName;
        public float HoldDuration, PositionMargin, RotationMargin;
        [NonReorderable] public PoseData[] PoseData;

        [Header("Convertion")]
        public string JsonString;
        public bool ConvertNow;
        public BodyTrackerResult result;


        void OnValidate()
        {
            int count = System.Enum.GetValues(typeof(BodyTrackerRole)).Length - 4;
            if (PoseData.Length != count)
            {
                PoseData = new PoseData[count];

                for (int i = 0; i < count; i++)
                {
                    PoseData[i].JointID = (BodyTrackerRole)i;
                    PoseData[i].JointName = PoseData[i].JointID.ToString();
                    PoseData[i].PosWeight = PoseData[i].RotWeight = 1;
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (PoseData[i].JointName != PoseData[i].JointID.ToString())
                        PoseData[i].JointName = PoseData[i].JointID.ToString();
                }
            }

            if(ConvertNow && JsonString.Length > 0)
            {
                result = JSONTools.ReadFromJson(JsonString);

                for (int i = 0; i < PoseData.Length; i++)
                {
                    var pose = result.trackingdata[i].localpose;
                    PoseData[i].Position = new Vector3((float)pose.PosX, (float)pose.PosY, (float)pose.PosZ);
                    PoseData[i].Rotation = new Quaternion((float)pose.RotQx, (float)pose.RotQy, (float)pose.RotQz, (float)pose.RotQw).eulerAngles;
                }

                ConvertNow = false;
                JsonString = string.Empty;
            }
        }
    }
}