using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class CharacterHurtState : CharacterBaseState
    {
        [SerializeField] private int hurtLayer = 6;
        [SerializeField] float hurtForceDampMultiplier = 0.5f;
        [SerializeField] UnityEvent onEnter;
        [SerializeField] private UnityEvent<int> onHurt;
        
        DamageData damageData;
        
        public void SetDamageData(DamageData damageData) => this.damageData = damageData;
        
        public override void Enter()
        {
            base.Enter();

            //agent.enabled = false;

            //animator.SetLayerWeightOverTime(1, layer: 5);
            //animator.PlayAnimation(GetHurtAnimation(), 0.1f, layer: 5);
            animator.PlayAnimation("Hurt", 0.1f, layer: hurtLayer);
            physicsHandler.AddForce(damageData.AttackDirection * (damageData.Damage * hurtForceDampMultiplier));
            //Debug.Log($"Attack Direction: {damageData.attackDirection} /n Damage Data: {damageData.damage}");

            //physicsHandler.AddForce(GetDamageForce());

            onEnter?.Invoke();
            onHurt?.Invoke(damageData.Damage);
            //Debug.Break();
        }

        public override void Tick()
        {
            base.Tick();

            Move();
        }
    }
}
