using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerChangeWeaponState : PlayerBaseState
    {
        [SerializeField] string animationName = "";
        public override void Enter()
        {
            base.Enter();

            animator.SetLayerWeightOverTime(1, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);
            animator.PlayAnimation(animationName, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);

            //weaponHandler.UnsheathWeapon();
        }

        public override void Exit()
        {
            base.Exit();
            //animator.SetLayerWeight(weaponHandler.CurrentWeapon.AnimationLayer, 0);
        }
    }
}
