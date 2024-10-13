using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState, IQueueValueSetter<CombatHandler>
    {
        [SerializeField] string animationName = string.Empty;
        [SerializeField] string moveXParam = "moveX", moveYParam = "moveY";

        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;


        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName);

            AddItem(combatHandler);
        }

        public override void Tick()
        {
            base.Tick();

            animator.SetFloat(moveXParam, 0, 0.085f, Time.deltaTime);
            animator.SetFloat(moveYParam, 0, 0.085f, Time.deltaTime);
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
