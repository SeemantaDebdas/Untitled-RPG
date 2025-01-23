using DG.Tweening;
using RPG.Combat;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public abstract class PlayerAttackState : PlayerBaseState
    {
        protected AttackData attack;
        [SerializeField] protected float rotationSpeed = 90f;

        public override void Enter()
        {
            base.Enter();

            // animator.SetLayerWeightOverTime(1, layer: 4);
            // animator.PlayAnimation(attack.AnimationName, 0.1f, 4);
            // //animator.SetLayerWeightOverTime(0, layer: weaponHandler.CurrentWeapon.WeaponData.AnimationLayer);
            //
            // FaceDirection(CalculateDirection(), rotationSpeed);
            //
            // FieldOfView attackFOV = GetComponentInParent<FieldOfView>();
            //
            // List<Transform> validTargets = attackFOV.GetValidTargets();
            //
            // if (validTargets.Count == 0)
            // {
            //     animator.applyRootMotion = true;
            // }
            // else
            // {
            //     MoveTowardsTarget(validTargets[0]);
            // }
            //
            //
            // for (int i = 0; i < attack.VFXDataList.Count; i++)
            // {
            //     VFXData data = attack.VFXDataList[i];
            //
            //     AutoTimer timeTillVFXPlays = new(data.VFXTime, () =>
            //     {
            //         if (data.VFXPrefab != null)
            //             context.VFXHandler.PlayVFX(data.VFXPrefab, 1f);
            //     });
            // }
        }

        public override void Exit()
        {
            base.Exit();

            // animator.applyRootMotion = false;
            // //animator.SetLayerWeightOverTime(1, layer: weaponHandler.CurrentWeapon.WeaponData.AnimationLayer);
            // animator.SetLayerWeightOverTime(0, layer: 4);
        }

        protected void MoveTowardsTarget(Transform closestTarget)
        {
            Vector3 dirFromTarget = (context.Transform.position - closestTarget.position).normalized;
            context.Transform.LookAt(closestTarget);
            context.Transform.DOMove(closestTarget.position + dirFromTarget, 0.1f);
        }
    }
}
