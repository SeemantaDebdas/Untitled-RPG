using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerChangeWeaponState : CharacterBaseState
    {
        [SerializeField] ScriptableString sheathAnimation, unsheathAnimation;

        WeaponSO previousWeapon, currentWeapon;
        public override void Enter()
        {
            base.Enter();

            if (weaponHandler == null)
                Debug.Log("Weapon Handler not set.", gameObject);

            previousWeapon = weaponHandler.PreviousWeapon;
            currentWeapon = weaponHandler.CurrentWeapon;

            Debug.Log("Current Weapon: " + currentWeapon.name);

            if (previousWeapon != null)
            {
                Debug.Log("Previous Weapon: " + previousWeapon.name);
                //sheath the previous weapon
                animator.PlayAnimation(sheathAnimation.Value, layer: previousWeapon.AnimationLayer);
            }

            animator.SetLayerWeightOverTime(1, layer: currentWeapon.AnimationLayer);
            animator.PlayAnimation(unsheathAnimation.Value, layer: currentWeapon.AnimationLayer);

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
