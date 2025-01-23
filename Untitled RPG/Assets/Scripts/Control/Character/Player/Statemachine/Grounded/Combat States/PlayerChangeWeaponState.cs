using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerChangeWeaponState : CharacterBaseState
    {
        [SerializeField] ScriptableString sheathAnimation, unsheathAnimation;

        WeaponSO previousWeaponData, currentWeaponData;
        public override void Enter()
        {
            base.Enter();
            //
            // if (weaponHandler == null)
            //     Debug.Log("Weapon Handler not set.", gameObject);
            //
            // currentWeaponData = weaponHandler.CurrentWeapon.WeaponData;
            //
            // Debug.Log("Current Weapon: " + currentWeaponData.name);
            //
            // if (weaponHandler.PreviousWeapon != null)
            // {
            //     previousWeaponData = weaponHandler.PreviousWeapon.WeaponData;
            //     Debug.Log("Previous Weapon: " + previousWeaponData.name);
            //     //sheath the previous weapon
            //     animator.PlayAnimation(sheathAnimation.Value, layer: previousWeaponData.AnimationLayer);
            // }
            //
            // animator.SetLayerWeightOverTime(1, layer: currentWeaponData.AnimationLayer);
            // animator.PlayAnimation(unsheathAnimation.Value, layer: currentWeaponData.AnimationLayer);
            
            animator.PlayAnimation(unsheathAnimation.Value, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            // if (previousWeaponData != null)
            // {
            //     animator.SetLayerWeightOverTime(0, layer: previousWeaponData.AnimationLayer);
            // }
            
        }
    }
}
