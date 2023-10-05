using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

namespace DevDunk.Movement
{
    [System.Serializable]
    public struct PoseData
    {
        public string JointName;
        [HideInInspector] public BodyTrackerRole JointID;
        public Vector3 Position, Rotation;
        [Range(0.0f, 1.0f)] public float PosWeight, RotWeight;
    }
}