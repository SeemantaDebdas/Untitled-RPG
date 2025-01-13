using RPG.Inventory.Model;
using UnityEngine;

namespace RPG.Ability
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Ability")]
    public class AbilitySO : ActionItemSO
    {
        [SerializeField] TargetingStrategy targetingStrategy;

        public override void Use(GameObject target)
        {
            targetingStrategy.StartTargeting();
        }
    }
}
