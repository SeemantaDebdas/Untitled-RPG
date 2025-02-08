using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Rework
{
    [System.Serializable]
    public struct VFXData
    {
        public GameObject VFXPrefab;
        public float VFXTime;
    }

    [CreateAssetMenu(fileName = "New Attack", menuName = "Debdas/Combat/Attack")]
    public class AttackData : ScriptableObject
    {
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public float ComboTime { get ; private set; }
        [field: SerializeField] public float Force { get; private set; }
        [field: SerializeField] public float DistanceFromTarget { get; private set; } = 0.75f;
        [field: SerializeField] public List<VFXData> VFXDataList { get; private set; }

        [field: Header("COLLIDER")]
        [field: SerializeField] public float ImpactStartTime { get; private set; }
        [field: SerializeField] public float ImpactEndTime { get; private set; }  
    }
}
