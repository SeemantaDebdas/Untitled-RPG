using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerAttackState : PlayerBaseState
    {
        AttackSO attack;
        public override void Enter()
        {
            base.Enter();

            animator.SetLayerWeight(weaponHandler.CurrentWeapon.AnimationLayer, 0);

            attack = weaponHandler.GetLightAttack();

            animator.PlayAnimation(attack.AnimationName, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            animator.SetLayerWeight(weaponHandler.CurrentWeapon.AnimationLayer, 1);
        }
    }
}
