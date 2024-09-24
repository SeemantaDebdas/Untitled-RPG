using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class PlayOnEnter : StateMachineBehaviour
    {
        [SerializeField] List<EventReference> soundClips;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < soundClips.Count; i++)
            {
                AudioManager.Instance.PlayOneShot(soundClips[i], animator.transform.position);
            }

        }
    }
}
