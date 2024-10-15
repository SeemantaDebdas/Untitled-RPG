using DG.Tweening;
using RPG.Core;
using RPG.Data;
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
        //[SerializeField] WeaponSO defaultWeapon = null;
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] List<WeaponSO> weaponList;

        Dictionary<WeaponSO, Weapon> weaponCache = new();
        public Weapon CurrentWeapon { get; private set; } = null;
        public Weapon PreviousWeapon { get; private set; } = null;
        int currentWeaponIndex = 0;

        public bool IsSheathed  => CurrentWeapon.IsSheathed; 
        
        public AttackSO CurrentAttack { get; private set; }

        int currentLightAttackIndex = -1, currentHeavyAttackIndex = -1;
        AutoTimer timeBetweenAttacksCounter;
        Animator animator;

        PlayerInput playerInput;


        private void Awake()
        {
            animator = GetComponent<Animator>();

            for(int i = 0; i < weaponList.Count; i++)
            {
                Weapon weaponInstance = SpawnAndCacheWeapon(weaponList[i]);
                SheathWeapon(weaponInstance);
            }
            EquipWeapon(weaponCache[weaponList[currentWeaponIndex]]);

            ResetTimer();
        }

        private void OnEnable()
        {
            playerInput = GetComponent<PlayerInput>();

            if(playerInput)
                playerInput.OnChangeWeaponPerformed += PlayerInput_OnChangeWeaponPerformed;
        }

        private void OnDisable()
        {
            if(playerInput) 
                playerInput.OnChangeWeaponPerformed -= PlayerInput_OnChangeWeaponPerformed;
        }

        #region ATTACK LOGIC

        public AttackSO GetLightAttack()
        {
            WeaponSO currentWeaponData = CurrentWeapon.WeaponData;
            //Debug.Log("Get Light Attack Called");
            currentLightAttackIndex = (currentLightAttackIndex + 1) == currentWeaponData.LightAttackList.Count ? -1 : currentLightAttackIndex;

            CurrentAttack = currentWeaponData.LightAttackList[++currentLightAttackIndex];

            ResetTimer();
            ResetColliderData(currentWeaponData);

            SetColliderState(CurrentWeapon, CurrentAttack.ColliderEnableTime, CurrentAttack.ColliderDisableTime);
            return CurrentAttack;
        }


        public AttackSO GetHeavyAttack()
        {
            WeaponSO currentWeaponData = CurrentWeapon.WeaponData;

            currentHeavyAttackIndex = (currentHeavyAttackIndex + 1) == currentWeaponData.HeavyAttackList.Count ? -1 : currentHeavyAttackIndex;

            CurrentAttack = currentWeaponData.HeavyAttackList[++currentHeavyAttackIndex];

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

        #region Colliders

        public void EnableCollider(Weapon weapon) => weapon.EnableCollider();
        public void DisableCollider(Weapon weapon) => weapon.DisableCollider();

        void SetColliderState(Weapon weapon, float startTime, float endTime)
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
        
        void ResetColliderData(WeaponSO weapon) => weaponCache[weapon].ResetColliderData();

        #endregion

        Weapon SpawnAndCacheWeapon(WeaponSO weaponSO)
        {
            Transform equipTransform = GetSheathTransform(weaponSO);
            Weapon weaponInstance = weaponSO.SpawnWeapon(equipTransform);
            weaponCache[weaponSO] = weaponInstance;

            return weaponInstance;
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (CurrentWeapon == weapon)
                return;

            PreviousWeapon = CurrentWeapon;
            CurrentWeapon = weapon;

            if(PreviousWeapon == null)
                return;

            if (PreviousWeapon.IsSheathed)
                return;

            PlayWeaponSheathAnimation(PreviousWeapon);

            PlayCurrentWeaponUnsheathAnimation();
        }

        public void PlayCurrentWeaponUnsheathAnimation()
        {
            if (!CurrentWeapon.IsSheathed)
                return;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(CharacterAnimationData.Instance.Unsheath))
                return;

            WeaponSO currentWeaponData = CurrentWeapon.WeaponData;
            animator.SetLayerWeightOverTime(1, layer: currentWeaponData.AnimationLayer);
            animator.PlayAnimation(CharacterAnimationData.Instance.Unsheath, layer: currentWeaponData.AnimationLayer);
        }

        public void PlayCurrentWeaponSheathAnimation()
        {
            PlayWeaponSheathAnimation(CurrentWeapon);
        }

        public void PlayWeaponSheathAnimation(Weapon weapon)
        {
            if (weapon.IsSheathed) 
                return;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName(CharacterAnimationData.Instance.Sheath))
                return;

            animator.PlayAnimation(CharacterAnimationData.Instance.Sheath, layer: weapon.WeaponData.AnimationLayer,
                                    onAnimationEnd: () =>
                                    {
                                        animator.SetLayerWeightOverTime(0, layer: weapon.WeaponData.AnimationLayer);
                                    },
                                    triggerTime: 0.9f);
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
            //Debug.Log(weapon.name);

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
        public void ToggleWeaponState(WeaponSO weaponData) // not the best way to handle things especaially since we need to pass the weapon via animator. Do it dynamically by using normalized time and scripting logic
        {
            Weapon weapon = weaponCache[weaponData];

            if (weapon.IsSheathed)
            {
                UnsheathWeapon(weapon);
            }
            else
            {
                SheathWeapon(weapon);
            }
        }

        public void SheathWeapon(Weapon weapon)
        {
            if (weapon == null || weapon.IsSheathed)
            {
                if (weapon.IsSheathed)
                    Debug.Log("weapon is sheathed");
                return;
            }

            weapon.SheathWeapon(GetSheathTransform(weapon.WeaponData));
            DisableCollider(weapon); 
        }

        public void UnsheathWeapon(Weapon weapon)
        {
            weapon.UnsheathWeapon(GetEquipTransform(weapon.WeaponData));
        }

        void PlayerInput_OnChangeWeaponPerformed(float value)
        {
            currentWeaponIndex += (int) value;
            currentWeaponIndex = (int) Mathf.Repeat(currentWeaponIndex, weaponList.Count);

            Debug.Log(weaponList[currentWeaponIndex].name);

            WeaponSO weaponSO = weaponList[currentWeaponIndex];
            EquipWeapon(weaponCache[weaponSO]);
        }
    }
}
