using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Health Effect", menuName = "Debdas/Abilities/Effect/Health Effect")]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] int healthChange = 0;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            foreach (GameObject target in data.GetTargets())
            {
                if (target.TryGetComponent(out Health health))
                {
                    if (healthChange < 0)
                    {
                        //negating the health change since Damage also subtracts from current health
                        health.Damage(new(data.GetUser().transform, -healthChange, Vector3.up, target.transform.position));
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }
            
            onFinished?.Invoke();
        }
    }
}
