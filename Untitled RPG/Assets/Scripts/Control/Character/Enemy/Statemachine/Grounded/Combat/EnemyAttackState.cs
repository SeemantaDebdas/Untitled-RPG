using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAttackState : EnemyBaseState, IListValueSetter<IStatemachine>
    {
        [SerializeField] float speed = 1.876f;
        [SerializeField] float speedMultiplier = 0.75f;
        [SerializeField] float rotationSpeed = 6f;
        [SerializeField] AttackSO attack;

        [SerializeField] ScriptableString moveXParam, moveYParam;
        
        [SerializeField] ScriptableEnemyList enemiesInAttackList;
        bool isAttacking = false;

        public override void Enter()
        {
            base.Enter();

            agent.ResetPath();  

            if(enemiesInAttackList != null )
            {
                AddItem(statemachine);
            }

            animator.SetFloat(moveXParam.Value, 0);

            isAttacking = false;
        }

        public override void Exit()
        {
            base.Exit();

            if (enemiesInAttackList != null)
            {
                RemoveItem(statemachine);
            }
        }

        public override void Tick()
        {
            if (EvaluateTransitions())
                return;

            Transform closestTarget = attackFOV.GetClosestTarget();

            float distanceToEnemy = Vector3.Distance(context.Transform.position, closestTarget.position);

            if(distanceToEnemy > 1.5f)
            {
                //Move closer to attack
                agent.SetDestination(closestTarget.position);

                HandleMovement(speed * speedMultiplier);
                FaceDirection(CalculateDirection(), rotationSpeed);

                animator.SetFloat(moveYParam.Value, controller.velocity.magnitude);
            }
            else if(!isAttacking)
            {
                isAttacking = true;
                animator.PlayAnimation(attack.AnimationName, 0.1f);
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
