using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public struct VFXData
    {
        public GameObject VFXPrefab;
        public float VFXTime;
    }

    [CreateAssetMenu(fileName = "New Attack", menuName = "Combat/Attack")]
    public class AttackSO : ScriptableObject
    {
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public float ComboTime { get ; private set; }
        [field: SerializeField] public float Force { get; private set; }
        [field: SerializeField] public List<VFXData> VFXDataList { get; private set; }

        [field: Header("COLLIDER")]
        [field: SerializeField] public float ColliderEnableTime { get; private set; }
        [field: SerializeField] public float ColliderDisableTime { get; private set; }  
    }
}
