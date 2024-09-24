using DG.Tweening;
using RPG.Core;
using System;
using System.Collections.Generic;
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
        [SerializeField] List<WeaponSO> weaponList;

        ///WeaponSO currentWeapon = null;
        //public WeaponSO CurrentWeapon => currentWeapon; 
        public WeaponSO CurrentWeapon { get; private set; } = null;
        int currentWeaponIndex = 0;

        public bool IsSheathed  => CurrentWeapon.IsSheathed; 
        
        public AttackSO CurrentAttack { get; private set; }

        int currentLightAttackIndex = -1, currentHeavyAttackIndex = -1;
        Timer timeBetweenAttacksCounter;

        PlayerInput playerInput;


        private void Awake()
        {
            //EquipWeapon(defaultWeapon);

            for(int i = 0; i < weaponList.Count; i++)
            {
                SpawnWeapon(weaponList[i]);
            }
            EquipWeapon(weaponList[currentWeaponIndex]);

            ResetTimer();
        }

        private void OnEnable()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.OnChangeWeaponPerformed += PlayerInput_OnChangeWeaponPerformed;
        }

        private void OnDisable()
        {
            playerInput.OnChangeWeaponPerformed -= PlayerInput_OnChangeWeaponPerformed;
        }

        #region ATTACK LOGIC

        public AttackSO GetLightAttack()
        {

            //Debug.Log("Get Light Attack Called");
            currentLightAttackIndex = (currentLightAttackIndex + 1) == CurrentWeapon.LightAttackList.Count ? -1 : currentLightAttackIndex;

            CurrentAttack = CurrentWeapon.LightAttackList[++currentLightAttackIndex];

            ResetTimer();
            ResetColliderData(CurrentWeapon);

            SetColliderState(CurrentWeapon, CurrentAttack.ColliderEnableTime, CurrentAttack.ColliderDisableTime);
            return CurrentAttack;
        }


        public AttackSO GetHeavyAttack()
        {
            currentHeavyAttackIndex = (currentHeavyAttackIndex + 1) == CurrentWeapon.HeavyAttackList.Count ? -1 : currentHeavyAttackIndex;

            CurrentAttack = CurrentWeapon.HeavyAttackList[++currentHeavyAttackIndex];

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
        public void EnableCollider(WeaponSO weapon) => weapon.WeaponInstance.EnableCollider();
        public void DisableCollider(WeaponSO weapon) => weapon.WeaponInstance.DisableCollider();

        
        void SetColliderState(WeaponSO weapon, float startTime, float endTime)
        {
            DOVirtual.Float(0, startTime, startTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
            {
                //Debug.Log("Enabling collider");
                EnableCollider(weapon);
                DOVirtual.Float(0, endTime, endTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
                {
                    //Debug.Log("Disabling Collider");
                    DisableCollider(weapon);
                });
            });
        }
        
        void ResetColliderData(WeaponSO weapon) => weapon.WeaponInstance.ResetColliderData();

        public void SpawnWeapon(WeaponSO weapon)
        {
            Transform equipTransform = GetEquipTransform(weapon);

            weapon.SpawnWeapon(equipTransform);

            SheathWeapon(weapon);
        }

        public void EquipWeapon(WeaponSO weapon)
        {
            if (CurrentWeapon == weapon)
                return;

            CurrentWeapon = weapon;
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
            Debug.Log(weapon.name);

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
            if (CurrentWeapon.IsSheathed)
            {
                UnsheathWeapon(CurrentWeapon);
            }
            else
            {
                SheathWeapon(CurrentWeapon);
            }
        }

        public void SheathWeapon(WeaponSO weapon)
        {
            weapon.SheathWeapon(GetSheathTransform(weapon));
            DisableCollider(weapon); 
        }

        public void UnsheathWeapon(WeaponSO weapon)
        {
            weapon.UnsheathWeapon(GetEquipTransform(weapon));
        }


        void PlayerInput_OnChangeWeaponPerformed(float value)
        {
            currentWeaponIndex += (int) value;
            currentWeaponIndex = (int) Mathf.Repeat(currentWeaponIndex, weaponList.Count);
            Debug.Log(currentWeaponIndex);
        }
    }
}
