using UnityEngine;

namespace RPG.Combat.Rework
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Debdas/Combat/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public float baseDamage;
        public float range;
        public bool canParry;
        public GameObject weaponPrefab;
        public AnimationClip equipAnimation;
        public AnimationClip unequipAnimation;
    }
}
