using DevDunk.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace DevDunk.GameSystems
{
    public class GameLoop : MonoBehaviour
    {
        public float posIncrementMultiplier = 1, rotIncrementMultiplier = 1;
        public VideoPlayer VideoPlayer;
        public float MinPoseTime = 2;
        public GameObject ProgressIndicator, completeIndicator;
        public AudioSource winAudio;
        public GameItem[] gameItems;
        private PoseObject activePose;
        public static bool StartGamePlay;
        WaitForSeconds waitSecond = new WaitForSeconds(1);

        Material progressMat;
        public float progress;
        public bool doneTPose;

        int shaderID;

        private void Awake()
        {
            StartGamePlay = false;
            VideoPlayer.gameObject.SetActive(false);
            progressMat = ProgressIndicator.GetComponent<Renderer>().sharedMaterial;
            ProgressIndicator.SetActive(false);
            shaderID = Shader.PropertyToID("_Progress");
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.sampleRate = 48000;
            AudioSettings.Reset(config);
        }

        IEnumerator Start()
        {
            while (!StartGamePlay) yield return waitSecond;
            VideoPlayer.gameObject.SetActive(true);

            foreach (var item in gameItems)
            {
                progress = 0;

                if (item.PoseClip)
                {
                    PlayVideo(item.PoseClip);
                    //Debug.Log("Play Video 1: " + item.PoseClip.name);
                }

                if (item.Pose)
                {
                    yield return WaitForPoseCompleted(item.Pose);
                    //Debug.Log("Done Pose" + item.Name);
                }
                else if(item.PoseClip)
                {
                    yield return WaitForEndOfClip();
                    //Debug.Log("Waited for clip1 ");
                }
                ProgressIndicator.SetActive(true);
                if (item.HoldClip)
                {
                    PlayVideo(item.HoldClip);
                    //Debug.Log("Play Video 2: " + item.HoldClip.name);
                    yield return WaitForEndOfClip();
                    TriggerComplete();
                }
                else if(item.Pose)
                {
                    yield return WaitFor5Seconds(item.HoldTimeNoVideo);
                    TriggerComplete();
                }
                else
                {
                    ProgressIndicator.SetActive(false);
                }

                
            }
        }

        private void Update()
        {
            progressMat.SetFloat(shaderID, progress);
        }

        private void TriggerComplete()
        {
            completeIndicator.SetActive(true);
            winAudio.Play();
            StartCoroutine(DisableObject());
        }

        IEnumerator DisableObject()
        {
            yield return waitSecond;
            ProgressIndicator.SetActive(false);
            completeIndicator.SetActive(false);
        }

        void PlayVideo(VideoClip clip)
        {
            VideoPlayer.Stop();
            VideoPlayer.clip = clip;
            //VideoPlayer.Prepare();
            //while (!VideoPlayer.isPrepared) yield return null;
            VideoPlayer.Play();
        }

        void SetActivePose(PoseObject pose)
        {
            activePose = pose;
            ExcersizeManager.Instance.SetActivePose(activePose.result);
        }

        IEnumerator WaitForEndOfClip()
        {
            //yield return new WaitForSeconds((float)VideoPlayer.clip.length);
            float time = 0;
            float duration = (float)VideoPlayer.clip.length;
            while (time < duration)
            {
                progress = time / duration;
                yield return null;
                time += Time.deltaTime;
            }

            //long frameCount = (long)VideoPlayer.frameCount;
            //while (VideoPlayer.frame < frameCount)
            //{
                
            //    yield return null;
            //}
        }

        IEnumerator WaitForPoseCompleted(PoseObject pose)
        {
            if (pose != null)
            {
                SetActivePose(pose);

                float timer = 0;
                float posMargin = activePose.PositionMargin;
                float rotMargin = activePose.RotationMargin;
                var deltaTime = Time.deltaTime;
                while ((ExcersizeManager.Instance.TotalDistance > posMargin
                    && ExcersizeManager.Instance.TotalRotation > rotMargin) || timer < MinPoseTime) //Check distance
                {
                    posMargin += (deltaTime * posIncrementMultiplier);
                    rotMargin += (deltaTime * rotIncrementMultiplier);
                    timer += deltaTime;
                    yield return null;
                }

                while(VideoPlayer.isPlaying) { yield return null; }
            }
            else
            {
                Debug.LogWarning("Your pose is empty yo ");
            }
        }


        IEnumerator WaitFor5Seconds(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                progress = time / duration;
                yield return null;
                time += Time.deltaTime;
            }
        }

        [ContextMenu("SET TO 6 SECONDS")]
        public void SetToFive()
        {
            for (int i = 0; i < gameItems.Length; i++)
            {
                gameItems[i].HoldTimeNoVideo = 5;
            }
        }
    }
}