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
        }
    }
}