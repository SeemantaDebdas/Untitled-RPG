using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLightAttackState : PlayerAttackState
    {

        public override void Enter()
        {
            base.Enter();

            Transform closestTarget = GetClosestTarget();

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

        private Transform GetClosestTarget()
        {
            if(context.TargetHandler.Target == null)
                return fieldOfView.GetClosestTarget();
            
            return context.TargetHandler.Target;
        }
    }
}
