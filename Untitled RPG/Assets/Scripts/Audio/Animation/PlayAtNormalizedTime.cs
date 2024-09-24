using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{

    [System.Serializable]
    public class AnimationAudioData
    {
        public EventReference sound;
        public float normalizedTimeThreshold;
        [HideInInspector] public bool hasPlayed;
    }

    public class PlayAtNormalizedTime : StateMachineBehaviour
    {
        [SerializeField] List<AnimationAudioData> audioDataList;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < audioDataList.Count; i++)
            {
                audioDataList[i].hasPlayed = false;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < audioDataList.Count; i++)
            {
                float normalizedTimeThreshold = audioDataList[i].normalizedTimeThreshold;

                if (!audioDataList[i].hasPlayed && stateInfo.normalizedTime >= normalizedTimeThreshold)
                {
                    audioDataList[i].hasPlayed = true;
                    AudioManager.Instance.PlayOneShot(audioDataList[i].sound, animator.transform.position);
                }
            }
        }
    }
}
