using FMODUnity;
using UnityEngine;
using RPG.Audio;
using FMOD.Studio;

namespace RPG.Core
{
    public enum FootstepLocation
    {
        LEFT, RIGHT
    }
    public class FootstepHandler : MonoBehaviour
    {
        [Header("AUDIO")]
        //[SerializeField] EventReference audioClip;
        [SerializeField] EventReference footstepAudio;

        [Header("PARTICLES")]
        [SerializeField] GameObject dustParticle;
        [SerializeField] Transform leftFootTransform, rightFootTransform;

        //VFXHandler vfxHandler = null;

        EventInstance walkAudioInstance, runAudioInstance;

        private void Awake()
        {
            //vfxHandler = GetComponent<VFXHandler>();    
        }

        public void PlayWalkAudio()
        { 
            if(!walkAudioInstance.isValid())
            {
                //Debug.Log("Walk audio instance not created. Creating.");
                walkAudioInstance = AudioManager.Instance.CreateEventInstance(footstepAudio);
            }

            walkAudioInstance.setParameterByName("WalkRunParam", 0);
            //AudioManager.Instance.PlayOneShot(walkAudio, transform.position);

            walkAudioInstance.start();
        }

        public void PlayRunAudio()
        {
            if (!runAudioInstance.isValid())
            {
                //Debug.Log("Run audio instance not created. Creating.");
                runAudioInstance = AudioManager.Instance.CreateEventInstance(footstepAudio);
            }

            runAudioInstance.setParameterByName("WalkRunParam", 1);
            //AudioManager.Instance.PlayOneShot(walkAudio, transform.position);

            runAudioInstance.start();
        }

        //public void PlaySkidAudio()
        //{
        //    AudioManager.Instance.PlayOneShot(skidAudio, transform.position);
        //}

        public void PlayDustParticles(FootstepLocation location)
        {
            Vector3 footstepPosition = location == FootstepLocation.LEFT? leftFootTransform.position : rightFootTransform.position;
            //vfxHandler.PlayVFXAtPosition(dustParticle, footstepPosition);
        }
    }
}
