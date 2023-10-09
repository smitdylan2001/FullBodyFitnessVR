using DevDunk.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DevDunk.GameSystems
{
    public class GameLoop : MonoBehaviour
    {
        public float posIncrementMultiplier = 1, rotIncrementMultiplier = 1;
        public VideoPlayer VideoPlayer;

        public GameItem[] gameItems;

        private PoseObject activePose;
        public bool StartGamePlay;
        WaitForSeconds waitSecond = new WaitForSeconds(1);

        IEnumerator Start()
        {
            if (!StartGamePlay) yield return waitSecond;

            foreach (var item in gameItems)
            {
                if(item.PoseClip)
                {
                    PlayVideo(item.PoseClip);
                }

                if (item.Pose)
                {
                    yield return WaitForPoseCompleted(item.Pose);
                }
                else if(item.PoseClip)
                {
                    yield return WaitForEndOfClip();
                }

                if (item.HoldClip)
                {
                    yield return WaitForEndOfClip();
                }
            }
        }

        void PlayVideo(VideoClip clip)
        {
            VideoPlayer.Stop();
            VideoPlayer.clip = clip;
            VideoPlayer.Play();
        }

        void SetActivePose(PoseObject pose)
        {
            activePose = pose;
            ExcersizeManager.Instance.SetActivePose(pose.result);
        }

        IEnumerator WaitForEndOfClip()
        {
            long frameCount = (long)VideoPlayer.frameCount;
            while (VideoPlayer.frame < frameCount)
            {
                yield return null;
            }
        }

        IEnumerator WaitForPoseCompleted(PoseObject pose)
        {
            SetActivePose(pose);

            float timer = 0;
            float posMargin = activePose.PositionMargin;
            float rotMargin = activePose.RotationMargin;
            while (ExcersizeManager.Instance.TotalDistance > posMargin
                && ExcersizeManager.Instance.TotalRotation > rotMargin) //Check distance
            {
                posMargin += timer + (Time.deltaTime * posIncrementMultiplier);
                rotMargin += timer + (Time.deltaTime * rotIncrementMultiplier);
                yield return null;
            }
        }
    }
}