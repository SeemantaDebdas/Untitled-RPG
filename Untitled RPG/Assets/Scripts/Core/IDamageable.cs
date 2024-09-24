using UnityEngine;

namespace RPG.Core
{
    public struct DamageData
    {
        public int damage;
        public Vector3 attackDirection;

        public DamageData(int damage, Vector3 attackDirection)
        {
            this.damage = damage;
            this.attackDirection = attackDirection;
        }
    }

    public interface IDamageable 
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void Damage(DamageData damageData);
    }
}
