using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(WeaponCollider))]
    public class Weapon : MonoBehaviour
    {
        WeaponCollider weaponCollider = null;
        

        private void Awake()
        {
            weaponCollider = GetComponent<WeaponCollider>();
        }

        public void SheathWeapon(Transform parent)
        {
            transform.SetParent(parent);

            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public void UnsheathWeapon(Transform parent)
        {
            transform.SetParent(parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            EnableVisual();
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
