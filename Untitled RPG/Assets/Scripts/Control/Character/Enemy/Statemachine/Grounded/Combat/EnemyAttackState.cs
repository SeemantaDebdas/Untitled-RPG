using RPG.Combat;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAttackState : EnemyBaseState, IListValueSetter<IStatemachine>
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
            Debug.Log("Entered Enemy Attack State", context.Transform);

            if(enemiesInAttackList != null )
            {
                AddItem(statemachine);
            }

            animator.SetFloatValueOverTime(CharacterAnimationData.Instance.MoveX, 0);

            isAttacking = false;

            animator.SetLayerWeightOverTime(1, layer: 4);
        }

        public override void Exit()
        {
            base.Exit();

            if (enemiesInAttackList != null)
            {
                RemoveItem(statemachine);
            }

            animator.SetLayerWeightOverTime(0, layer: 4);
            
            //agent.ResetPath();
        }

        public override void Tick()
        {
            if (EvaluateTransitions())
            {
                Debug.Log("Coniditons not met", context.Transform);
                return;
            }

            Transform closestTarget = attackFOV.GetClosestTarget();

            float distanceToEnemy = Vector3.Distance(context.Transform.position, closestTarget.position);

            if(distanceToEnemy > 1.5f)
            {
                //Move closer to attack
                agent.SetDestination(closestTarget.position);

                HandleMovement(speed * speedMultiplier);
                FaceDirection(CalculateDirection(), rotationSpeed);

                animator.SetFloat(CharacterAnimationData.Instance.MoveY, controller.velocity.magnitude);
            }
            else if(!isAttacking)
            {
                isAttacking = true;
                animator.PlayAnimation(weaponHandler.GetLightAttack().AnimationName, 0.1f, 4);
            }
        }

        #region Scriptable List Operations

        public void AddItem(IStatemachine item)
        {
            enemiesInAttackList.AddItem(item, this);
        }

        public void ClearEnumerable(){}

        public void RemoveItem(IStatemachine item)
        {
            enemiesInAttackList.RemoveItem(item, this);
        }

        public void SetValue(IStatemachine value){}

        #endregion


    }
}
