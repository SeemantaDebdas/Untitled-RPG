using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Combat/Ranged Weapon")]
    public class RangedWeaponSO : WeaponSO
    {
        [field: Header("RANGED PROPERTIES")]
        [field: SerializeField] public Rigidbody Projectile { get; private set; }
        [field: SerializeField] public int DrawFireAnimationLayer { get; private set; }
        [field: SerializeField] public string DrawAnimation { get; private set; }
        [field: SerializeField] public string FireAnimation { get; private set; }
    }
}
