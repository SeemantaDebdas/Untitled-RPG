using UnityEngine;

namespace RPG.Core
{
    public struct DamageData
    {
        public Transform instigator;
        public int damage;
        public Vector3 attackDirection;

        public DamageData(Transform instigator, int damage, Vector3 attackDirection)
        {
            this.instigator = instigator;
            this.damage = damage;
            this.attackDirection = attackDirection;
        }

        public bool IsNull =>  damage == 0 && attackDirection == Vector3.zero;
    }

    public interface IDamageable 
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void Damage(DamageData damageData);
    }
}
