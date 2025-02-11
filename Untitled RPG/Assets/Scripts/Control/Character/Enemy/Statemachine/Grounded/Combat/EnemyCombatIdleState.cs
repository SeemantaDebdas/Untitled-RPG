using RPG.Combat.Rework;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState, IQueueValueSetter<CombatHandler>
    {
        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;
        [SerializeField] private float rotationSpeed = 6f;
        [SerializeField] float speed = 1.876f;
        [SerializeField] float speedMultiplier = 0.75f;
        
        private EnemyGridManager enemyGridManager = null;

        //Vector3 localGridPosition = Vector3.zero;
        EnemyGridManager.GridSlot slot = null;
        
        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation("Strafe_Walk");
            
            //Debug.Log("Entered Enemy Combat Idle State");

            Transform closestTarget = chaseFov.GetClosestTarget();
            enemyGridManager = closestTarget.GetComponent<EnemyGridManager>();

            slot = enemyGridManager.RequestGridPosition(context.Transform, 1);
            enemiesInCombatQueue.AddItem(combatHandler, this);
            
            fieldOfView.SetAngle(360f);
            
            agent.ResetPath();
        }

        public override void Exit()
        {
            base.Exit();
            
            enemyGridManager.UnoccupyGridPositionForEnemy(context.Transform, 1);
        }

        public override void Tick()
        {
            if (EvaluateTransitions()) return;

            Vector3 destination = chaseFov.GetClosestTarget().position + slot.Position;
            float distance = Vector3.Distance(destination, context.Transform.position);
    
            if (distance < 0.25f)
            {
                agent.isStopped = true;
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveX, 0, 0.085f, Time.deltaTime);
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, 0, 0.085f, Time.deltaTime);
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(destination);
                HandleMovement(speed * speedMultiplier);

                Vector3 localVelocity = context.Transform.InverseTransformDirection(controller.velocity);
                float localVelocityX = Mathf.Abs(localVelocity.x) < 0.15f ? 0 : localVelocity.x;
                float localVelocityZ = Mathf.Abs(localVelocity.z) < 0.15f ? 0 : localVelocity.z;
        
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveX, localVelocityX, 0.25f, Time.deltaTime);
                animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, localVelocityZ, 0.25f, Time.deltaTime);
            }

            FaceMovementDirection(CalculateDirectionToTarget(), rotationSpeed);
        }


        Vector3 CalculateDirectionToTarget()
        {
            Vector3 targetPos = chaseFov.GetClosestTarget().position;
            Vector3 direction = (targetPos - context.Transform.position);
            direction.y = 0;
            direction.Normalize();

            return direction;
        }

        public void RemoveItem()
        {
            
        }

        public void AddItem(CombatHandler item)
        {

        }

        public void ClearEnumerable()
        {
            
        }

        public void SetValue(CombatHandler value)
        {
            
        }
    }
}
