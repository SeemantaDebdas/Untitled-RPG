using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Data
{
    public class CharacterAnimationData : MonoBehaviour
    {
        [field: Header("Variables")]

        [field: SerializeField] public string Idle { get; private set; }
        [field: SerializeField] public string Walk { get; private set; }
        [field: SerializeField] public string Run { get; private set; }
        [field: SerializeField] public string Jump { get; private set; }
        [field: SerializeField] public string Fall { get; private set; }
        [field: SerializeField] public string Strafe { get; private set; }
        [field: SerializeField] public string LandIdle { get; private set; }
        [field: SerializeField] public string LandMove { get; private set; }
        [field: SerializeField] public string Sheath { get; private set; }
        [field: SerializeField] public string Unsheath { get; private set; }


        [field: Header("Parameters")]
        [field: SerializeField] public string MoveX { get; private set; }
        [field: SerializeField] public string MoveY { get; private set; }

        public static CharacterAnimationData Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
}
