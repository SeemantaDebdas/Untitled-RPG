using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Debdas/Abilities/Ability")]
    public class AbilitySO : ActionItemSO
    {
        [field: Header("Ability Settings")]
        [field: SerializeField] public float CooldownTime { get; private set; } = 0f;
        [field: SerializeField] public int ManaCost { get; private set; } = 0;
        [field: SerializeField] public int AbilityDuration { get; private set; } = 0;
        
        [Header("Ability Events")]
        [SerializeField] ScriptableEvent onAbilityStarted = null;
        [SerializeField] ScriptableEvent onAbilityInitiated = null;
        [SerializeField] ScriptableEvent onAbilityFinished = null;

        [Header("Strategies")]
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] List<FilterStrategy> filterList;
        [SerializeField] List<EffectStrategy> stateDependentEffectList;
        [SerializeField] List<EffectStrategy> stateIndependentEffectList;

        public override bool TryUse(GameObject user)
        {
            // Use the ability and track the result
            bool targetsAcquired = false;
            AbilityData data = new(user, AbilityDuration);

            targetingStrategy.StartTargeting(data, () =>
            {
                targetsAcquired = OnTargetsAcquired(data);
            });

            return targetsAcquired;
        }

        private bool OnTargetsAcquired(AbilityData data)
        {
            foreach (FilterStrategy filterStrategy in filterList)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }
            
            if (!data.GetTargets().Any()) 
                return false;
            
            Debug.Log("Ability Stated");
            onAbilityStarted.Raise(data.GetUser().transform, this);

            // Track effect completion
            int remainingEffects = stateDependentEffectList.Count + stateIndependentEffectList.Count;
            int remainingStateDependentEffects = stateDependentEffectList.Count;

            foreach (EffectStrategy effectStrategy in stateDependentEffectList)
            {
                effectStrategy.StartEffect(data, () =>
                {
                    remainingEffects = DecreaseRemainingEffects(data, remainingEffects);

                    remainingStateDependentEffects--;

                    if (remainingStateDependentEffects == 0)
                    {
                        onAbilityInitiated.Raise(data.GetUser().transform, this);
                    }
                });
            }
            
            foreach (EffectStrategy effectStrategy in stateIndependentEffectList)
            {
                effectStrategy.StartEffect(data, () =>
                {
                    remainingEffects = DecreaseRemainingEffects(data, remainingEffects);
                });
            }
            
            return true;
        }

        private int DecreaseRemainingEffects(AbilityData data, int remainingEffects)
        {
            remainingEffects--;

            // Check if all effects are finished
            if (remainingEffects == 0)
            {
                onAbilityFinished.Raise(data.GetUser().transform, this);
            }

            return remainingEffects;
        }
    }
}
