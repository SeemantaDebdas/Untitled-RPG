using DG.Tweening;
using RPG.Combat;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public abstract class PlayerAttackState : PlayerBaseState
    {
        protected AttackSO attack;
        [SerializeField] protected float rotationSpeed = 90f;

        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(attack.AnimationName, 0.1f);
            animator.SetLayerWeightOverTime(0, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);
            
            FaceMovementDirection(CalculateDirection(), rotationSpeed);

            FieldOfView attackFOV = GetComponentInParent<FieldOfView>();

            List<Transform> validTargets = attackFOV.GetValidTargets();

            if (validTargets.Count == 0)
            {
                animator.applyRootMotion = true;
            }
            else
            {
                MoveTowardsTarget(validTargets[0]);
            }
        }

        public override void Exit()
        {
            base.Exit();

            animator.applyRootMotion = false;
            animator.SetLayerWeightOverTime(1, layer: weaponHandler.CurrentWeapon.UnsheathAnimationLayer);
        }

        protected void MoveTowardsTarget(Transform closestTarget)
        {
            Vector3 dirFromTarget = (context.Transform.position - closestTarget.position).normalized;
            context.Transform.LookAt(closestTarget);
            context.Transform.DOMove(closestTarget.position + dirFromTarget, 0.1f);
        }
    }
}
