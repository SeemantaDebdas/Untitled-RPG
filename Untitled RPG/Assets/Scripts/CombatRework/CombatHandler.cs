using System;
using System.Collections;
using MoreMountains.Feedbacks;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat.Rework
{
    public class CombatHandler : MonoBehaviour
    {
        [SerializeField] private WeaponHandler weaponHandler;
        [SerializeField] private UnityEvent onAttack;
        [SerializeField] private UnityEvent onAttackHit;

        [Header("Polish")] 
        [SerializeField] private MMF_Player attackHitFeedback;

        public bool IsStunned { get; private set; }
        public Transform CounterTarget { get; private set; } = null;
        public float TimeTillCounterWindowClose { get; private set; } = -1f;
        public AttackData CounterAttackData{get; private set;}

        private IEnumerator counterAttackCoroutine;

        private void OnEnable()
        {
            weaponHandler.OnHit += WeaponHandler_OnHit;
        }

        private void OnDisable()
        {
            weaponHandler.OnHit -= WeaponHandler_OnHit;
        }

        void WeaponHandler_OnHit(IDamageable damageable, AttackData attackData)
        {
            onAttackHit?.Invoke();

            if (attackHitFeedback != null)
            {
                //Debug.Log(attackData.Damage * 0.001f);
               attackHitFeedback.GetFeedbackOfType<MMF_FreezeFrame>().FreezeFrameDuration = attackData.Damage * 0.001f;
               attackHitFeedback.PlayFeedbacks();
            }
        }

        public void PerformAttack(bool isHeavy)
        {
            // Additional logic for targeting and validating attack
            weaponHandler.Attack(isHeavy);
        }

        public void PerformAttack(CombatHandler target)
        {
            weaponHandler.Attack(target);
        }
        
        public void CounterAttack(CombatHandler target)
        {
            weaponHandler.CounterAttack(target);
        }

        public bool CanCounter()
        {
            return CounterTarget != null && TimeTillCounterWindowClose > 0f;
        }

        public bool CanCombo()
        {
            return weaponHandler.CanCombo();
        }
        
        public void Attack()
        {
            onAttack?.Invoke();
        }

        public void PerformAttackTowardsTarget(CombatHandler closestTarget)
        {
            weaponHandler.PerformAttackTowardsTarget(closestTarget);
        }

        public void NotifyAttack(Transform attacker, UnarmedAttackData attack, float timeTillImpact)
        {
            if (counterAttackCoroutine != null)
            {
                StopCoroutine(counterAttackCoroutine);
            }
            
            CounterTarget = attacker; 
            TimeTillCounterWindowClose = timeTillImpact;
            CounterAttackData = attack;
            
            counterAttackCoroutine = ResetCounterTargetAfterTime(timeTillImpact - Time.time);
            StartCoroutine(counterAttackCoroutine);
            
            //Debug.Log($"Current Time : {Time.time} || Time Till Impact: {timeTillImpact}");
        }

        public void SetStunned(bool isStunned)
        {
            IsStunned = isStunned;
        }

        IEnumerator ResetCounterTargetAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            
            //Debug.Log($"Resetting Counter Target: {Time.time}");
            CounterTarget = null;
            TimeTillCounterWindowClose = -1f;
            CounterAttackData = null;
        }
    }
}
