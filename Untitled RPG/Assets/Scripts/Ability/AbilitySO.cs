using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Debdas/Abilities/Ability")]
    public class AbilitySO : ActionItemSO
    {
        [field: Header("Ability Settings")]
        [field: SerializeField] public float CooldownTime { get; private set; } = 0f;
        [field: SerializeField] public int ManaCost { get; private set; } = 0;
        
        [Header("Ability Events")]
        [SerializeField] ScriptableEvent onAbiltyStarted = null;
        [SerializeField] ScriptableEvent onAbiltyFinished = null;

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
            foreach (FilterStrategy filterStrategy in filterStrategyList)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }
            
            if (!data.GetTargets().Any()) 
                return false;
            
            Debug.Log("Ability Stated");
            onAbiltyStarted.Raise(data.GetUser().transform, this);

            // Track effect completion
            int remainingEffects = effectStrategyList.Count;

            foreach (EffectStrategy effectStrategy in effectStrategyList)
            {
                effectStrategy.StartEffect(data, () =>
                {
                    remainingEffects--;

                    // Check if all effects are finished
                    if (remainingEffects == 0)
                    {
                        Debug.Log("Ability Ended");
                        onAbiltyFinished.Raise(data.GetUser().transform, this);
                    }
                });
            }

            return true;
        }
    }
}
