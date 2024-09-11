using FMODUnity;
using UnityEngine;
using RPG.Audio;

namespace RPG.Core
{
    public enum FootstepLocation
    {
        LEFT, RIGHT
    }
    public class FootstepHandler : MonoBehaviour
    {
        [Header("AUDIO")]
        [SerializeField] EventReference audioClip;
        [SerializeField] EventReference walkAudio, runAudio, skidAudio;

        [Header("PARTICLES")]
        [SerializeField] GameObject dustParticle;
        [SerializeField] Transform leftFootTransform, rightFootTransform;

        //VFXHandler vfxHandler = null;

        private void Awake()
        {
            //vfxHandler = GetComponent<VFXHandler>();    
        }

        public void PlayAudio()
        {
            AudioManager.Instance.PlayOneShot(audioClip, transform.position);
        }

        public void PlayWalkAudio()
        {
            AudioManager.Instance.PlayOneShot(walkAudio, transform.position);
        }

        public void PlayRunAudio()
        {
            AudioManager.Instance.PlayOneShot(runAudio, transform.position);
        }

        public void PlaySkidAudio()
        {
            AudioManager.Instance.PlayOneShot(skidAudio, transform.position);
        }

        public void PlayDustParticles(FootstepLocation location)
        {
            Vector3 footstepPosition = location == FootstepLocation.LEFT? leftFootTransform.position : rightFootTransform.position;
            //vfxHandler.PlayVFXAtPosition(dustParticle, footstepPosition);
        }
    }
}
