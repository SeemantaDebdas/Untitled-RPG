using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerWeaponHolsterState : PlayerBaseState
    {
        [SerializeField] string animationName = string.Empty;

        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);
        }

        public override void Exit()
        {
            base.Exit();

            animator.SetLayerWeightOverTime(0, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);
        }
    }
}