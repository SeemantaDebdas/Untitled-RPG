using RPG.Combat;
using RPG.Core;
using RPG.Data;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class EnemyHurtState : EnemyBaseState
    {
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
            animator.PlayAnimation("Hurt", 0.1f, layer: 6);
            //Debug.Log($"Attack Direction: {damageData.attackDirection} /n Damage Data: {damageData.damage}");

            //physicsHandler.AddForce(GetDamageForce());

            onEnter?.Invoke();
            onHurt?.Invoke(damageData.Damage);
            //Debug.Break();
        }

        public override void Exit() 
        { 
            base.Exit();

            //agent.enabled = true;
            //animator.SetLayerWeightOverTime(0, layer: 5);
        }

        public override void Tick()
        {
            base.Tick();

            //Move();
        }
        
        string GetHurtAnimation()
        {
            float angle = Vector3.Angle(context.Transform.forward, damageData.AttackDirection);
            Debug.Log("Hurt angle: " + angle);

            string hurtAnimation;
            if (Mathf.Abs(angle) >= 0 && Mathf.Abs(angle) <= 30)
            {
                hurtAnimation = CharacterAnimationData.Instance.Hurt.Back;
            }
            else if (angle < -30 && angle > -160)
            {
                hurtAnimation = CharacterAnimationData.Instance.Hurt.Left;
            }
            else if (angle > 30 && angle < 160)
            {
                hurtAnimation = CharacterAnimationData.Instance.Hurt.Right;
            }
            else
            {
                hurtAnimation = CharacterAnimationData.Instance.Hurt.Front;
            }

            return hurtAnimation;
        }

        Vector3 GetDamageForce()
        {
            Vector3 dirFromAttacker = (context.Transform.position - damageData.Instigator.position).normalized;
            return dirFromAttacker * (damageData.Damage * hurtForceDampMultiplier);
        }
        
    }
}
