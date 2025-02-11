using RPG.Combat.Rework;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAttackState : EnemyBaseState, IListValueSetter<Transform>
    {
        [SerializeField] float speed = 1.876f;
        [SerializeField] float speedMultiplier = 0.75f;
        [SerializeField] float rotationSpeed = 6f;

        [SerializeField] ScriptableEnemyList enemiesInAttackList;
        bool isAttacking = false;

        public override void Enter()
        {
            base.Enter();

            //agent.ResetPath();
            //
            //Debug.Log("Entered Enemy Attack State", context.Transform);
            
            animator.PlayAnimation("Strafe_Run");

            if(enemiesInAttackList != null )
            {
                enemiesInAttackList.AddItem(context.Transform, this);
            }

            //animator.SetFloatValueOverTime(CharacterAnimationData.Instance.Locomotion.MoveX, 0);

            isAttacking = false;

            //animator.SetLayerWeightOverTime(1, layer: 4);
        }

        public override void Exit()
        {
            base.Exit();

            if (enemiesInAttackList != null)
            {
                enemiesInAttackList.RemoveItem(context.Transform, this);
            }

            animator.SetLayerWeightOverTime(0, layer: 4);
            
            //agent.ResetPath();
        }

        public override void Tick()
        {
            if (EvaluateTransitions())
            {
                //Debug.Log("Conditions not met", context.Transform);
                return;
            }

            Transform closestTarget = chaseFov.GetClosestTarget();

            float distanceToEnemy = Vector3.Distance(context.Transform.position, closestTarget.position);

            if (isAttacking)
                return;
            
            if(distanceToEnemy > 1f)
            {
                //Move closer to attack
                agent.SetDestination(closestTarget.position);

                HandleMovement(speed * speedMultiplier);
                FaceMovementDirection(CalculateDirection(), rotationSpeed);

                //animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, controller.velocity.magnitude);
                Vector3 localVelocity = context.Transform.InverseTransformDirection(controller.velocity);
                float localVelocityX = Mathf.Abs(localVelocity.x) < 0.15f ? 0 : localVelocity.x;
                float localVelocityZ = Mathf.Abs(localVelocity.z) < 0.15f ? 0 : localVelocity.z;
        
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveX, localVelocityX, 0.25f, Time.deltaTime);
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, localVelocityZ, 0.25f, Time.deltaTime);
            }
            else
            {
                isAttacking = true;
                agent.ResetPath();
                combatHandler.PerformAttack(closestTarget.GetComponent<CombatHandler>());
                //closestTarget.GetComponent<RPG.Combat.Rework.CombatHandler>().NotifyAttack();
                //animator.PlayAnimation(weaponHandler.GetLightAttack().AnimationName, 0.1f, 4);
            }
        }
    }
}
