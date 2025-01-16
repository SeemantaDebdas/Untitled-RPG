using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Delay Composite Effect", menuName = "Debdas/Abilities/Effect/Delay Composite Effect")]
    public class DelayCompositeEffect : EffectStrategy
    {
        [SerializeField] private float delay;
        [SerializeField] List<EffectStrategy> effectStrategyList;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            Timing.RunCoroutine(DelayedEffect(data, onFinished));
        }

        private IEnumerator<float> DelayedEffect(AbilityData data, Action onFinished = null)
        {
            yield return Timing.WaitForSeconds(delay);
            
            int remainingEffects = effectStrategyList.Count;

            foreach (EffectStrategy effectStrategy in effectStrategyList)
            {
                effectStrategy.StartEffect(data, () =>
                {
                    remainingEffects--;

                    // Check if all effects are finished
                    if (remainingEffects == 0)
                    {
                        onFinished?.Invoke();
                    }
                });
            }
        }
    }
}
