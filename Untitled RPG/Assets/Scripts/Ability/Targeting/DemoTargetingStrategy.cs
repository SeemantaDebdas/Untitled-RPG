using UnityEngine;

namespace RPG.Ability.Targeting
{
    [CreateAssetMenu(fileName = "New Demo Targeting", menuName = "Abilities/Targeting/Demo Targeting")]
    public class DemoTargetingStrategy : TargetingStrategy
    {
        public override void StartTargeting()
        {
            Debug.Log("Demo Targeting called");
        }
    }
}