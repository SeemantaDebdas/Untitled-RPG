using System;
using KevinIglesias;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] UnarmedWeapon unarmedWeapon; 
        [SerializeField] private Weapon defaultWeapon = null;
        private SheathComponentScript sheathScript = null;

        public Weapon CurrentWeapon {get; private set; }
        private Animator animator = null;

        private void Awake()
        {
            sheathScript = GetComponent<SheathComponentScript>();
            animator = GetComponent<Animator>();
            
            if (defaultWeapon == null)
            {
                defaultWeapon = unarmedWeapon;
            }
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.Unequip();
            }

            CurrentWeapon = newWeapon;
            CurrentWeapon.Initialize(newWeapon.name, newWeapon.BaseDamage, newWeapon.Range, newWeapon.CanParry, animator);
        }

        public bool IsCurrentWeaponSheathed()
        {
            if (CurrentWeapon == null)
                return false;
            
            return sheathScript.IsWeaponSheathed(CurrentWeapon.transform);
        }

        public void Attack(bool isHeavy)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.Attack(isHeavy);
            }
        }
        
        public void Attack(CombatHandler target)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.Attack(target);
            }
        }

        public bool CanCombo()
        {
            if (CurrentWeapon == null)
            {
                Debug.LogError("Current Weapon is Null");
                return false;   
            }
            
            return CurrentWeapon.CanCombo();
        }

        public bool HasCurrentAttackFinished()
        {
            return CurrentWeapon.HasCurrentAttackFinished();
        }

        public void PerformAttackTowardsTarget(CombatHandler combatHandler)
        {
            CurrentWeapon?.Attack(combatHandler);
        }

        public void CounterAttack(CombatHandler target)
        {
            CurrentWeapon?.CounterAttack(target);
        }
    }
}
