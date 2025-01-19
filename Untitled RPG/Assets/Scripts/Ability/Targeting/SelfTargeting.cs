using System;
using UnityEngine;

namespace RPG.Ability.Targeting
{
    [CreateAssetMenu(fileName = "New Self Targeting", menuName = "Debdas/Abilities/Targeting/Self Targeting")]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action onTargetingFinished)
        {
            data.SetTargets(new[] { data.GetUser() });
            onTargetingFinished?.Invoke();
        }
    }
}
