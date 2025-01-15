using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability.Targeting
{
    [CreateAssetMenu(fileName = "New Demo Targeting", menuName = "Debdas/Abilities/Targeting/Demo Targeting")]
    public class DemoTargetingStrategy : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action onTargetingFinished)
        {
            Debug.Log("Demo Targeting called");
        }
    }
}