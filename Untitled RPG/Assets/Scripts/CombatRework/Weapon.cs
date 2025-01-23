using UnityEngine;

namespace RPG.Combat.Rework
{
    public abstract class Weapon : MonoBehaviour
    {
        public string WeaponName { get; private set; }
        public float BaseDamage { get; private set; }
        public float Range { get; private set; }
        public bool CanParry { get; private set; }
        public bool IsEquipped { get; protected set; }
        public bool IsSheathed { get; private set; }

        protected Animator animator;

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
        public abstract bool CanCombo();
    }

}
