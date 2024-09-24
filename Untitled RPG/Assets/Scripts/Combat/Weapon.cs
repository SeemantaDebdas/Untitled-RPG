using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(WeaponCollider))]
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public Transform ShootPoint { get; private set; }   
        WeaponCollider weaponCollider = null;

        public bool IsSheathed { get; private set; } = true;
        

        private void Awake()
        {
            weaponCollider = GetComponent<WeaponCollider>();
        }

        public void SheathWeapon(Transform parent)
        {
            transform.SetParent(parent);

            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            IsSheathed = true;
        }

        public void UnsheathWeapon(Transform parent)
        {
            transform.SetParent(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            EnableVisual();

            IsSheathed = false;
        }

        public void EnableCollider() => weaponCollider.EnableCollider();

        public void DisableCollider() => weaponCollider.DisableCollider();

        public void ResetColliderData()
        {
            weaponCollider.ResetColliderData();
        }

        void DisableVisual()
        {
            //dissolveShaderController.Dissolve();
        }


        void EnableVisual()
        {
            //dissolveShaderController.UnDissolve();
        }

    }
}
