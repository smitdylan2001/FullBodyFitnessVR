using DevDunk.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct GameItem
{
    public string Name;
    public VideoClip PoseClip;
    public PoseObject Pose;

    public VideoClip HoldClip;
<<<<<<< Updated upstream
=======

    public Texture2D PoseTexture;
    public PoseObject Pose;

    public float HoldTimeNoVideo;
>>>>>>> Stashed changes
}
