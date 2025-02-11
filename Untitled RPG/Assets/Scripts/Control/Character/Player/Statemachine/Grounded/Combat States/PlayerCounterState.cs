using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RPG.Combat.Rework;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerCounterState : PlayerBaseState
    {
        [SerializeField] private float parryNominalImpactNormalized = 0.5f;
        [SerializeField] private float rotationSpeed = 50f;
        IEnumerator counterCoroutine;

        private Transform counterTarget = null;

        private Tween moveTween = null;
        
        public override void Enter()
        {
            base.Enter();
            animator.PlayAnimation("Counter", layer: 5, transitionDuration: 0.1f, triggerTime: 0.7f, onAnimationEnd: () =>
            {
                CombatHandler closestTarget = context.TargetHandler.Target;

                if (closestTarget == null)
                {
                    combatHandler.PerformAttack(false);
                }
                else
                {
                    Debug.Log("Performing Attack towards target");
                    combatHandler.CounterAttack(closestTarget);
                }
            });
            
            if(counterCoroutine != null)
                StopCoroutine(counterCoroutine);

            counterCoroutine = Counter();
            StartCoroutine(counterCoroutine);
            
            Debug.Log("IS Countering");

            counterTarget = combatHandler.CounterTarget;
            context.Health.EnableInvulnerability();

            //Time.timeScale = 0.5f;
            
            Vector3 targetPosition = counterTarget.position 
                                     + (transform.position - counterTarget.position).normalized 
                                     * (combatHandler.CounterAttackData.DistanceFromTarget + 0.5f);

            // Stop any existing movement
            moveTween?.Kill();

            // Move to the target position smoothly
            moveTween = context.Transform.DOMove(targetPosition, 0.1f) // Adjust duration as needed
                .SetEase(Ease.OutSine);
        }

        public override void Exit()
        {
            base.Exit();
            context.Health.DisableInvulnerability();
            
            //Time.timeScale = 1f;
        }

        public override void Tick()
        {
            base.Tick();
            
            Vector3 dirToTarget = (counterTarget.transform.position - context.Transform.position).normalized;
            dirToTarget.y = 0;
            dirToTarget.Normalize();
            
            Debug.DrawRay(context.Transform.position, dirToTarget, Color.red);
            
            RotateTowardsDirection(dirToTarget, rotationSpeed);
            
            //Move();
        }

        private IEnumerator Counter()
        {
            // Wait one frame to ensure the new animation state is active.
            yield return new WaitForEndOfFrame();
            
            float parryClipLength = animator.GetCurrentAnimatorStateInfo(0).length;
            float playerNominalImpactTime = parryClipLength * parryNominalImpactNormalized;
            float timeTillPlayerCounter = Time.time + playerNominalImpactTime;
            
            Debug.Log($"Enemy Punch: {combatHandler.TimeTillCounterWindowClose} Player Block: {timeTillPlayerCounter}");
            float speedFactor = combatHandler.TimeTillCounterWindowClose > 0 ? combatHandler.TimeTillCounterWindowClose / timeTillPlayerCounter : 1f;
            animator.SetFloat("counterSpeed", speedFactor);

            animator.SetFloat("counterSpeed", speedFactor);

        }

    }
}
