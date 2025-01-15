using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Debdas/Abilities/Ability")]
    public class AbilitySO : ActionItemSO
    {
        [Header("Ability Settings")]
        [field: SerializeField] public float CooldownTime { get; private set; } = 0f;

        [Header("Strategies")]
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] List<FilterStrategy> filterStrategyList;
        [SerializeField] List<EffectStrategy> effectStrategyList;

        public override bool TryUse(GameObject user)
        {
            // Use the ability and track the result
            bool targetsAcquired = false;
            AbilityData data = new(user);

            targetingStrategy.StartTargeting(data, () =>
            {
                targetsAcquired = OnTargetsAcquired(data);
            });

            return targetsAcquired;
        }

        private bool OnTargetsAcquired(AbilityData data)
        {
            // Apply filtering
            foreach (FilterStrategy filterStrategy in filterStrategyList)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            // Apply effects
            foreach (EffectStrategy effectStrategy in effectStrategyList)
            {
                effectStrategy.StartEffect(data);
            }

            // Indicate success
            return data.GetTargets().Count() > 0;
        }
    }
}
