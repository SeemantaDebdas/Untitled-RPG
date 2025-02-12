using RPG.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RPG.Control
{
    public class EnemyStunnedState : EnemyBaseState
    {
        [SerializeField] private float hurtForceDampMultiplier = 0.1f;
        [SerializeField] private ParticleSystem stunParticles, stunLoopParticles;
        [SerializeField] EnemyKnockoutState knockoutState;
        
        DamageData damageData;
        
        public void SetDamageData(DamageData damageData)
        {
            this.damageData = damageData;
        }
        
        public override void Enter()
        {
            base.Enter();
            
            animator.PlayAnimation("Hurt", 0.1f, layer: 6, () =>
            {
                animator.PlayAnimation("Stunned", 0.1f, layer: 6);
                stunLoopParticles.gameObject.SetActive(true);
            });
            
            stunParticles.gameObject.SetActive(true);

            combatHandler.SetStunned(true);
            physicsHandler.AddForce(damageData.AttackDirection * (damageData.Damage * hurtForceDampMultiplier));
            
            SubscribeToHurtEvent();
            SubscribeToStunnedEvent();
        }

        public override void Exit()
        {
            base.Exit();
            
            combatHandler.SetStunned(false);
            
            animator.Play("EMPTY", layer: 6);
            
            stunParticles.gameObject.SetActive(false);
            stunLoopParticles.gameObject.SetActive(false);
            
            UnsubscribeToHurtEvent();
            UnsubscribeToStunnedEvent();
        }

        public override void Tick()
        {
            base.Tick();
        
            Move();
        }

        protected override void CharacterDamageHandler_OnHurt(DamageData damageData)
        {
            SwitchToKnockoutState(damageData);
        }

        protected override void CharacterDamageHandler_OnStunned(DamageData damageData)
        {
            SwitchToKnockoutState(damageData);
        }
        private void SwitchToKnockoutState(DamageData damageData)
        {
            knockoutState.SetDamageData(damageData);
            SwitchState(knockoutState);
        }

    }
}
