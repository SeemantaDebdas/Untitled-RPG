using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] public int AnimationLayer { get; private set; } = 4;
        public string WeaponName { get; private set; }
        public float BaseDamage { get; private set; }
        public float Range { get; private set; }
        public bool CanParry { get; private set; }
        public bool IsEquipped { get; protected set; }
        public bool IsSheathed { get; private set; }

        protected Animator animator;

        public Action<IDamageable, AttackData> OnHit; 

        public virtual void Initialize(string name, float damage, float range, bool canParry, Animator anim)
        {
            WeaponName = name;
            BaseDamage = damage;
            Range = range;
            CanParry = canParry;
            animator = anim;

            Sheath(); // By default, sheath the weapon
        }

        public void Sheath()
        {
            IsSheathed = true;
        }

        public void Unsheath()
        {
            IsSheathed = false;
        }

        public abstract void Equip();
        public abstract void Unequip();
        public abstract void Attack(bool isHeavy);
        public abstract void Attack(CombatHandler target);
        public abstract void CounterAttack(CombatHandler target);   
        //public abstract void SnapAttack(Transform target);
        public abstract bool CanCombo();

        public bool HasCurrentAttackFinished()
        {
            float normalizeTime = animator.GetNormalizedTime("Attack", AnimationLayer);

            return normalizeTime > 0.9f;
        }

    }

}
