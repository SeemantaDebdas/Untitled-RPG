using RPG.Combat.Rework;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLightAttackState : PlayerAttackState
    {

        public override void Enter()
        {
            base.Enter();

            CombatHandler closestTarget = context.TargetHandler.Target;

            if (closestTarget == null)
            {
                combatHandler.PerformAttack(false);
            }
            else
            {
                Debug.Log("Performing Attack towards target");
                combatHandler.PerformAttackTowardsTarget(closestTarget);
            }
            
            //AudioManager.Instance.PlayOneShot(weaponHandler.CurrentWeapon.swooshSound, context.Transform.position);
        }
    }
}
