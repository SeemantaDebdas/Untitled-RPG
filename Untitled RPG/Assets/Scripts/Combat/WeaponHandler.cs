using DG.Tweening;
using RPG.Core;
using System;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponHandler : MonoBehaviour
    {

        [Header("SHEATH LOCATIONS")]
        [SerializeField] Transform rightBack;
        [SerializeField] Transform leftBack;

        [Header("UNSHEATH LOCATIONS")]
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;

        [Space]
        [SerializeField] WeaponSO defaultWeapon = null;
        [SerializeField] float timeBetweenAttacks = 2f;

        WeaponSO currentWeapon = null;
        public WeaponSO CurrentWeapon => currentWeapon; 

        Weapon currentWeaponInstance = null;
        public Weapon CurrentWeaponInstance => currentWeaponInstance;
        
        bool isSheathed = false;
        public bool IsSheathed  => isSheathed; 
        
        public AttackSO CurrentAttack { get; private set; }

        int currentLightAttackIndex = -1, currentHeavyAttackIndex = -1;
        Timer timeBetweenAttacksCounter;


        private void Awake()
        {
            EquipWeapon(defaultWeapon);
            ResetTimer();
        }

#region ATTACK LOGIC

        public AttackSO GetLightAttack()
        {

            //Debug.Log("Get Light Attack Called");
            currentLightAttackIndex = (currentLightAttackIndex + 1) == currentWeapon.LightAttackList.Count ? -1 : currentLightAttackIndex;

            CurrentAttack = currentWeapon.LightAttackList[++currentLightAttackIndex];

            ResetTimer();
            ResetColliderData();

            SetColliderState(CurrentAttack.ColliderEnableTime, CurrentAttack.ColliderDisableTime);
            return CurrentAttack;
        }


        public AttackSO GetHeavyAttack()
        {
            currentHeavyAttackIndex = (currentHeavyAttackIndex + 1) == currentWeapon.HeavyAttackList.Count ? -1 : currentHeavyAttackIndex;

            CurrentAttack = currentWeapon.HeavyAttackList[++currentHeavyAttackIndex];

            return CurrentAttack;
        }

        void ResetTimer()
        {
            timeBetweenAttacksCounter?.StopTimer(); // Stop the old timer before resetting it

            timeBetweenAttacksCounter = new(timeBetweenAttacks, () =>
            {
                //Debug.Log("Timer Reset");
                currentLightAttackIndex = -1;
            });
        }

#endregion ATTACK LOGIC
        public void EnableCollider() => currentWeaponInstance.EnableCollider();
        public void DisableCollider() => currentWeaponInstance.DisableCollider();

        
        void SetColliderState(float startTime, float endTime)
        {
            DOVirtual.Float(0, startTime, startTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
            {
                //Debug.Log("Enabling collider");
                EnableCollider();
                DOVirtual.Float(0, endTime, endTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //Debug.Log("Disabling Collider");
                    DisableCollider();
                });
            });
        }
        
        void ResetColliderData() => currentWeaponInstance.ResetColliderData();

        public void EquipWeapon(WeaponSO weapon)
        {
            //sheathedWeapon.SetActive(false);
            //unsheathedWeapon.SetActive(true);

            Transform equipTransform = GetEquipTransform(weapon);

            if(currentWeapon == weapon) 
                return;

            currentWeapon = weapon;

            if(currentWeaponInstance != null)
            {
                Destroy(currentWeaponInstance);
            }

            currentWeaponInstance = Instantiate(weapon.WeaponPrefab, equipTransform.position, equipTransform.rotation);
            SheathWeapon();
        }

        Transform GetEquipTransform (WeaponSO weapon)
        {
            return weapon.WeaponUnsheathLocation switch
            {
                WeaponUnsheathLocation.LEFT_HAND => leftHand,
                WeaponUnsheathLocation.RIGHT_HAND => rightHand,
                _ => null,
            };
        }

        Transform GetSheathTransform(WeaponSO weapon)
        {
            return weapon.WeaponUnsheathLocation switch
            {
                WeaponUnsheathLocation.LEFT_HAND => leftBack,
                WeaponUnsheathLocation.RIGHT_HAND => rightBack,
                _ => null,
            };
        }

        /// <summary>
        /// This function should be called from Equpping/Holstering Animation Event
        /// </summary>
        public void ToggleWeaponState()
        {
            if(currentWeaponInstance == null)
                return;

            if (isSheathed)
            {
                UnsheathWeapon();
            }
            else
            {
                SheathWeapon();
            }
        }

        public void SheathWeapon()
        {
            if (isSheathed || currentWeaponInstance == null) return;

            isSheathed = true;

            currentWeaponInstance.SheathWeapon(GetSheathTransform(currentWeapon));
            
            DisableCollider(); 
        }

        public void UnsheathWeapon()
        {
            if (!isSheathed || currentWeaponInstance == null) return;

            currentWeaponInstance.UnsheathWeapon(GetEquipTransform(currentWeapon));

            isSheathed = false;
        }
    }
}
