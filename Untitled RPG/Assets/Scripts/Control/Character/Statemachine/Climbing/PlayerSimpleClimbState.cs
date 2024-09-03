using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerSimpleClimbState : PlayerBaseState
    {
        [SerializeField] float rotationSpeed = 180f;

        [SerializeField] List<CanClimbObject> climbConditions;

        string animationName = "";
        Vector3 matchPosition;
        Quaternion matchRotation;

        CanClimbObject selectedClimbObjectData;

        public override void Enter()
        {
            base.Enter();

            for (int i = 0; i < climbConditions.Count; i++)
            {
                if (climbConditions[i].Evaluate(context))
                {
                    selectedClimbObjectData = climbConditions[i];
                    break;
                }
            }

            animationName = selectedClimbObjectData.AnimationName;


            EnvironmentScanner scanner = context.EnvironmentScanner;

            RaycastHit? objHit = scanner.GetObjectInfront(context.Transform.position);
            matchPosition = GetMatchPosition(objHit);

            matchRotation = Quaternion.LookRotation(-objHit.Value.normal);

            animator.applyRootMotion = true;
            controller.enabled = false;

            PlayAnimation(animationName, 0.1f);
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

        public override void Exit()
        {
            base.Exit();

            controller.enabled = true;
            animator.applyRootMotion = false;
        }

        public override void Tick()
        {
            base.Tick();

            if(context.Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                context.Animator.CustomMatchTarget(
                            matchPosition,
                            matchRotation,
                            selectedClimbObjectData.AvatarTarget,
                            new MatchTargetWeightMask(Vector3.one, 0),
                            0.11f,
                            0.17f);
            }

            context.Transform.rotation = Quaternion.RotateTowards(context.Transform.rotation, matchRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
