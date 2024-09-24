using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public enum WeaponUnsheathLocation { LEFT_HAND, RIGHT_HAND }

    [CreateAssetMenu(fileName = "New Weapon", menuName = "Combat/Weapon")]
    public class WeaponSO : ScriptableObject
    {
        [field: SerializeField] public Weapon WeaponPrefab { get; private set; }
        [field: SerializeField] public int AnimationLayer { get; private set; } = 0;
        [field: SerializeField] public List<AttackSO> LightAttackList { get; private set; } 
        [field: SerializeField] public List<AttackSO> HeavyAttackList { get; private set; }
        [field: SerializeField] public WeaponUnsheathLocation WeaponUnsheathLocation { get; private set; }

        [Header("SFX")]
        [field: SerializeField] public EventReference swooshSound;
    }
}
