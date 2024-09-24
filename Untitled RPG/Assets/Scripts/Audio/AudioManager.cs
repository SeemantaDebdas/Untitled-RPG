using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

namespace RPG.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] EventReference ambianceWindSound;
        [SerializeField] EventReference music;


        public static AudioManager Instance { get; private set; }

        List<EventInstance> eventInstanceList = new List<EventInstance>();

        EventInstance ambianceInstance, musicInstance;

        void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Debug.LogError("More than one Audio Manager Instance found");
                Destroy(this);
                return;
            }

            Instance = this;

            eventInstanceList = new();
        }

        private void Start()
        {
            InitializeAmbiance(ambianceWindSound);
            InitializeMusic(music);
        }

        void OnDestroy()
        {
            CleanUpEventInstances();
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPosition)
        {
            RuntimeManager.PlayOneShot(sound, worldPosition);
        }

        public EventInstance CreateEventInstance(EventReference soundReference)
        {
            EventInstance instance = RuntimeManager.CreateInstance(soundReference);
            eventInstanceList.Add(instance);
            return instance;
        }

        void InitializeAmbiance(EventReference ambianceSound)
        {
            if (ambianceSound.IsNull)
                return;

            ambianceInstance = CreateEventInstance(ambianceSound);
            ambianceInstance.start();
        }
        void InitializeMusic(EventReference music)
        {
            if (music.IsNull)
                return;

            musicInstance = CreateEventInstance(music);
            musicInstance.start();
        }

        void CleanUpEventInstances()
        {
            //stop and release created instances
            foreach(EventInstance instance in eventInstanceList)
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
            }
        }
    }
}
