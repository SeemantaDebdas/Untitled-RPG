using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "Has Interactable", menuName = "Condition/Detection/Has Interactable", order = 1)]
    public class HasInteractable : ConditionSO 
    {
        public override void Initialize(Context context)
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            return ((CharacterContext)context).Interactor.HasInteractable();
        }

        public override void Reset()
        {
            
        }
    }
}
