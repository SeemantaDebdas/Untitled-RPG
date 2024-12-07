using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "HasAnimationEnded", menuName = "Condition/Animation/Has Animation Ended", order = 1)]
    public class HasAnimationEnded : ConditionSO
    {
        [SerializeField] string animationTag = "";
        [SerializeField] float normalizedTimeThreshold = 0.9f;
        [SerializeField] int layerIndex = 0;
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            CharacterContext characterContext = context as CharacterContext;

            if (characterContext.Animator.GetNormalizedTime(animationTag, layerIndex) > normalizedTimeThreshold)
                return true;
            
            return false;
        }
    }
}
