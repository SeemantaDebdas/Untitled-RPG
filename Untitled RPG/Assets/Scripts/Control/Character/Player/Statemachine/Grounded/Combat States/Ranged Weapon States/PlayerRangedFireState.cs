using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerRangedFireState : PlayerBaseState
    {
        RangedWeaponSO rangedWeapon;
        public override void Enter()
        {
            base.Enter();
            rangedWeapon = weaponHandler.CurrentWeapon as RangedWeaponSO;
            animator.PlayAnimation(rangedWeapon.FireAnimation, 0.1f, rangedWeapon.DrawFireAnimationLayer);
        }

        public override void Exit()
        {
            base.Exit();

            animator.SetLayerWeightOverTime(1, 0.25f, rangedWeapon.UnsheathAnimationLayer);
            animator.SetLayerWeightOverTime(0, 0.25f, rangedWeapon.DrawFireAnimationLayer);
        }
    }
}
