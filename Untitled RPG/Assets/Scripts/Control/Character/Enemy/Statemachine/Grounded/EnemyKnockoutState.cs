using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyKnockoutState : EnemyBaseState
    {
        private DamageData damageData;
        [SerializeField] private float hurtForceDampMultiplier = 0.1f;
        
        public void SetDamageData(DamageData damageData) => this.damageData = damageData;
        public override void Enter()
        {
            base.Enter();
            
            context.Animator.PlayAnimation("Knockdown", 0.1f, layer: 6);
            physicsHandler.AddForce(damageData.AttackDirection * (damageData.Damage * hurtForceDampMultiplier));
        }

        public override void Tick()
        {
            base.Tick();

            Move();
        }
    }
}
