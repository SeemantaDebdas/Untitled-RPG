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

            animator.PlayAnimation(animationName, layer: weaponHandler.CurrentWeapon.AnimationLayer);
        }

        public override void Exit()
        {
            base.Exit();

            animator.SetLayerWeight(weaponHandler.CurrentWeapon.AnimationLayer, 0);
        }
    }
}
