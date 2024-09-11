using DG.Tweening;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] Transform sheathLocation, unsheathLocation;
        [SerializeField] WeaponSO defaultWeapon = null;
        [SerializeField] float timeBetweenAttacks = 2f;

        WeaponSO currentWeapon = null;
        public WeaponSO CurrentWeapon { get => currentWeapon; }
        Weapon currentWeaponInstance = null;
        
        bool isSheathed = false;
        public bool IsSheathed { get => isSheathed; }   
        
        public AttackSO CurrentAttack { get; private set; }

        int currentAttackIndex = -1;
        Timer timeBetweenAttacksCounter;


        private void Awake()
        {
            EquipWeapon(defaultWeapon);
            ResetTimer();
        }

#region ATTACK LOGIC

        public AttackSO GetLightAttack()
        {

            Debug.Log("Get Light Attack Called");
            currentAttackIndex = (currentAttackIndex + 1) == currentWeapon.LightAttackList.Count ? -1 : currentAttackIndex;

            CurrentAttack = currentWeapon.LightAttackList[++currentAttackIndex];

            ResetTimer();
            ResetColliderData();

            SetColliderState(CurrentAttack.ColliderEnableTime, CurrentAttack.ColliderDisableTime);
            return CurrentAttack;
        }


        public AttackSO GetHeavyAttack()
        {
            CurrentAttack = currentWeapon.HeavyAttackList[0];
            return CurrentAttack;
        }

        void ResetTimer()
        {
            timeBetweenAttacksCounter?.StopTimer(); // Stop the old timer before resetting it

            timeBetweenAttacksCounter = new(timeBetweenAttacks, () =>
            {
                Debug.Log("Timer Reset");
                currentAttackIndex = -1;
            });
        }

#endregion ATTACK LOGIC
        public void EnableCollider() => currentWeaponInstance.EnableCollider();
        public void DisableCollider() => currentWeaponInstance.DisableCollider();

        
        void SetColliderState(float startTime, float endTime)
        {
            DOVirtual.Float(0, startTime, startTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
            {
                EnableCollider();
                DOVirtual.Float(0, endTime, endTime, value => { }).SetEase(Ease.Linear).OnComplete(() =>
                {
                    DisableCollider();
                });
            });
        }
        
        void ResetColliderData() => currentWeaponInstance.ResetColliderData();

        public void EquipWeapon(WeaponSO weapon)
        {
            //sheathedWeapon.SetActive(false);
            //unsheathedWeapon.SetActive(true);

            if(currentWeapon == weapon) 
                return;

            currentWeapon = weapon;

            if(currentWeaponInstance != null)
            {
                Destroy(currentWeaponInstance);
            }

            currentWeaponInstance = Instantiate(weapon.WeaponPrefab, sheathLocation.position, sheathLocation.rotation);
            SheathWeapon();
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

            currentWeaponInstance.SheathWeapon(sheathLocation);
            
            DisableCollider(); 
        }

        public void UnsheathWeapon()
        {
            if (!isSheathed || currentWeaponInstance == null) return;

            currentWeaponInstance.UnsheathWeapon(unsheathLocation);

            isSheathed = false;
        }
    }
}
