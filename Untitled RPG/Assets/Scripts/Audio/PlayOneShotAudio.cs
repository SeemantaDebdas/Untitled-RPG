using FMODUnity;
using UnityEngine;

namespace RPG.Audio
{
    public class PlayOneShotAudio : MonoBehaviour
    {
        [SerializeField] EventReference audioClip;
        public void PlayAudio()
        {
            AudioManager.Instance.PlayOneShot(audioClip, transform.position);
        }
    }
}
