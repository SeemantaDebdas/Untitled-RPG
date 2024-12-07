using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsComboTimeReached", menuName = "Condition/Combat/Is Combo Time Reached", order = 1)]
    public class IsComboTimeReached : ConditionSO
    {
        [SerializeField] string attackTag = string.Empty;
        [SerializeField] int layer = 0;
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            CharacterContext characterContext = (CharacterContext)context;
            
            float comboTime = characterContext.WeaponHandler.CurrentAttack.ComboTime;
            float normalizedTime = characterContext.Animator.GetNormalizedTime(attackTag, layer);

            return normalizedTime > comboTime;
        }
    }
}
