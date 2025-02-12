using System;
using UnityEngine;

namespace RPG.Core
{
    public struct DamageData
    {
        public Transform Instigator { get; private set; }
        public int Damage { get; private set; }
        public Vector3 AttackDirection{ get; private set; }
        public Vector3 HitPoint { get; private set; }
        public string HitAnimation { get; private set; }

        public DamageData(Transform instigator, int damage, Vector3 attackDirection, Vector3 hitPoint, string hitAnimation = "")
        {
            Instigator = instigator;
            Damage = damage;
            AttackDirection = attackDirection;
            HitPoint = hitPoint;
            HitAnimation = hitAnimation;
        }

        public bool IsNull =>  Damage == 0 && AttackDirection == Vector3.zero;
    }

    public interface IDamageable 
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void Damage(DamageData damageData);
    }
}
