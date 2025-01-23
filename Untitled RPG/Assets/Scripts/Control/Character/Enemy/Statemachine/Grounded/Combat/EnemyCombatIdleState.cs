using RPG.Combat;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState, IQueueValueSetter<CombatHandler>
    {
        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;
        [SerializeField] float rotationSpeed = 6f;

        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Strafe);

            //AddItem(combatHandler);

            if (attackFOV.GetClosestTarget() != null)
            {
                //if (weaponHandler.CurrentWeapon.IsSheathed)
                    //weaponHandler.PlayCurrentWeaponUnsheathAnimation();
            }

            agent.ResetPath();
        }

        public override void Tick()
        {
            if(EvaluateTransitions())
            {
                return;
            }
            
            agent.SetDestination(attackFOV.GetClosestTarget().position);
            
            FaceDirection(CalculateDirection(), rotationSpeed);

            animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveX, 0, 0.085f, Time.deltaTime);
            animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, 0, 0.085f, Time.deltaTime);
        }

        public void RemoveItem()
        {
            
        }

        public void AddItem(CombatHandler item)
        {
            enemiesInCombatQueue.AddItem(item, this);
        }

        public void ClearEnumerable()
        {
            
        }

        public void SetValue(CombatHandler value)
        {
            
        }
    }
}
