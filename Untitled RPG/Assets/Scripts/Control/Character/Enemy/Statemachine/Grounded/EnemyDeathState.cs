using RPG.Core;
using RPG.Data;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Control
{
    public class EnemyDeathState : EnemyBaseState
    {
        [SerializeField] float hurtForceDampMultiplier = 0.5f;
        [SerializeField] private UnityEvent onEnter, onDeathAnimationEnd;
        [SerializeField] private ConditionSO onDeathAnimationEndCondition;
        DamageData damageData;
        public void SetDamageData(DamageData damageData) => this.damageData = damageData;
        
        public override void Enter()
        {
            base.Enter();

            //animator.PlayAnimation(CharacterAnimationData.Instance.Hurt.Death, 0.1f);
            onEnter?.Invoke();
            
            //physicsHandler.AddForce(GetDamageForce());
        }

        // public override void Tick()
        // {
        //     base.Tick();
        //
        //     //condition being. On Death Animation End.
        //     if (onDeathAnimationEndCondition.Evaluate(context))
        //     {
        //         //Debug.Log("On Death Animation End");
        //         onDeathAnimationEnd?.Invoke();
        //     }
        //
        //     Move();
        // }

        Vector3 GetDamageForce()
        {
            Vector3 dirFromAttacker = (context.Transform.position - damageData.Instigator.position).normalized;
            return dirFromAttacker * (damageData.Damage * hurtForceDampMultiplier);
        }
    }
}
