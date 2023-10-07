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
        public PoseObject Starting, Squad, TPose, RKneeUp, LKneeUp;
        public VideoClip Introduction, StartVideo, SquadVideo, SquadHoldIt, TPoseVideo, RKneeVideo, RKneeHoldIt, LKneeVideo, LKneeHoldIt, Ending;

        private PoseObject activePose;

        IEnumerator Start()
        {
            PlayVideo(Introduction);
            yield return WaitForEndOfClip();

            activePose = Starting;
            PlayVideo(StartVideo);

            yield return WaitForPoseCompleted();

            activePose = Squad;
            PlayVideo(SquadVideo);

            yield return WaitForPoseCompleted();

            PlayVideo(SquadHoldIt);

            yield return new WaitForSeconds(activePose.HoldDuration);

            activePose = TPose;
            PlayVideo(TPoseVideo);

            yield return WaitForPoseCompleted();

            activePose = RKneeUp;
            PlayVideo(RKneeVideo);

            yield return WaitForPoseCompleted();

            PlayVideo(RKneeHoldIt);

            yield return WaitForEndOfClip();

            activePose = LKneeUp;
            PlayVideo(LKneeVideo);

            yield return WaitForPoseCompleted();

            PlayVideo(LKneeHoldIt);

            yield return WaitForEndOfClip();

            PlayVideo(Ending);
            //Reset BasePose (maybe calculate positions based off of changes from base pose for auto recenter??)
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

        IEnumerator WaitForPoseCompleted()
        {
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