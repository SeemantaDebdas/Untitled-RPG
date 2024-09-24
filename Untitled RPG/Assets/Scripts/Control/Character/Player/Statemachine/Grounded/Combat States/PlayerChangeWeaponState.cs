using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerChangeWeaponState : PlayerBaseState
    {
        [SerializeField] string sheathAnimation = string.Empty, unsheathAnimation = string.Empty;

        WeaponSO previousWeapon, currentWeapon;
        public override void Enter()
        {
            base.Enter();

            previousWeapon = weaponHandler.PreviousWeapon;
            currentWeapon = weaponHandler.CurrentWeapon;

            Debug.Log("Current Weapon: " + currentWeapon.name);

            if (previousWeapon != null)
            {
                Debug.Log("Previous Weapon: " + previousWeapon.name);
                //sheath the previous weapon
                animator.PlayAnimation(sheathAnimation, layer: previousWeapon.AnimationLayer);
            }

            animator.SetLayerWeightOverTime(1, layer: currentWeapon.AnimationLayer);
            animator.PlayAnimation(unsheathAnimation, layer: currentWeapon.AnimationLayer);

        }

        public override void Exit()
        {
            base.Exit();

            if (previousWeapon != null)
            {
                animator.SetLayerWeightOverTime(0, layer: previousWeapon.AnimationLayer);
            }
            
        }
    }
}
