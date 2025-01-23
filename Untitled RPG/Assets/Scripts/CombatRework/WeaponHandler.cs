using System;
using KevinIglesias;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat.Rework
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private Weapon defaultWeapon = null;
        private SheathComponentScript sheathScript = null;

        public Weapon CurrentWeapon {get; private set; }

        private void Awake()
        {
            EquipWeapon(defaultWeapon);
            sheathScript = GetComponent<SheathComponentScript>();
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.Unequip();
            }

            CurrentWeapon = newWeapon;
            CurrentWeapon.Initialize(
                newWeapon.WeaponName,
                newWeapon.BaseDamage,
                newWeapon.Range,
                newWeapon.CanParry,
                GetComponent<Animator>()
            );

            CurrentWeapon.Sheath(); 
        }

        public void UnsheathWeapon()
        {
            CurrentWeapon?.Unsheath();
            //playing the animation should unsheath the weapon automatically beacuse of sheath unsheath script
        }

        public void SheathWeapon()
        {
            CurrentWeapon?.Sheath();
            //playing the animation should unsheath the weapon automatically beacuse of sheath unsheath script
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

        public bool CanCombo()
        {
            return CurrentWeapon.CanCombo();
        }
    }
}
