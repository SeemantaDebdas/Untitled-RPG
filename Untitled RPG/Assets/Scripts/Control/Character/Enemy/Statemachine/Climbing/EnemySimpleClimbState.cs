using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemySimpleClimbState : EnemyBaseState
    {
        [SerializeField] float rotationSpeed = 180f;
        [SerializeField] float changedColliderRadius = 0.15f;

        [SerializeField] List<CanClimbObject> climbConditions;

        string animationName = "";
        Vector3 matchPosition;
        Quaternion matchRotation;

        CanClimbObject selectedClimbObjectData;
        float initialColliderRadius;

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Enemy Climb State");

            for (int i = 0; i < climbConditions.Count; i++)
            {
                if (climbConditions[i].Evaluate(context))
                {
                    selectedClimbObjectData = climbConditions[i];
                    break;
                }
            }

            //DOVirtual.Float(0, selectedClimbObjectData.MatchStartTime, selectedClimbObjectData.MatchStartTime, (v) => { controller.enabled = false; });;
            initialColliderRadius = controller.radius;
            controller.radius = changedColliderRadius;

            animationName = selectedClimbObjectData.AnimationName;


            EnvironmentScanner scanner = context.EnvironmentScanner;

            RaycastHit? objHit = scanner.GetObjectInfront(context.Transform.position);
            matchPosition = GetMatchPosition(objHit);

            matchRotation = Quaternion.LookRotation(-objHit.Value.normal);

            animator.applyRootMotion = true;

            animator.PlayAnimation(animationName, 0.1f);
            
        }

        public override void Exit()
        {
            base.Exit();

            controller.radius = initialColliderRadius;
            controller.enabled = true;
            animator.applyRootMotion = false;
            agent.ResetPath();
        }


        private Vector3 GetMatchPosition(RaycastHit? objHit)
        {
            Vector3 matchPosition;

            matchPosition = context.EnvironmentScanner.GetObjectBelow(objHit.Value.point).Value.point;

            //z offset
            matchPosition += -objHit.Value.normal * selectedClimbObjectData.MatchPositionOffset.z;

            //x offset
            Vector3 rotatedNormal = Quaternion.AngleAxis(90, Vector3.up) * -objHit.Value.normal;
            matchPosition += rotatedNormal * selectedClimbObjectData.MatchPositionOffset.x;

            //y offset
            matchPosition += Vector3.up * selectedClimbObjectData.MatchPositionOffset.y;

            return matchPosition;
        }

        public override void Tick()
        {
            base.Tick();

            if (context.Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                context.Animator.CustomMatchTarget(
                            matchPosition,
                            matchRotation,
                            selectedClimbObjectData.AvatarTarget,
                            new MatchTargetWeightMask(Vector3.one, 0),
                            selectedClimbObjectData.MatchStartTime,
                            selectedClimbObjectData.MatchEndTime);
            }

            context.Transform.rotation = Quaternion.RotateTowards(context.Transform.rotation, matchRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
