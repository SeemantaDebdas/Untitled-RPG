using RPG.Combat;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyCombatIdleState : EnemyBaseState, IQueueValueSetter<CombatHandler>
    {
        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;


        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(CharacterAnimationData.Instance.Strafe);

            AddItem(combatHandler);

            if (attackFOV.GetClosestTarget() != null)
            {
                if (weaponHandler.CurrentWeapon.IsSheathed)
                    weaponHandler.PlayCurrentWeaponUnsheathAnimation();
            }
        }

        public override void Tick()
        {
            base.Tick();

            animator.SetFloat(CharacterAnimationData.Instance.MoveX, 0, 0.085f, Time.deltaTime);
            animator.SetFloat(CharacterAnimationData.Instance.MoveY, 0, 0.085f, Time.deltaTime);
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
