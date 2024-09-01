using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "HasAnimationEnded", menuName = "Condition/Has Animation Ended", order = 1)]
    public class HasAnimationEnded : ConditionSO
    {
        [SerializeField] string animationTag = "";
        [SerializeField] float normalizedTimeThreshold = 0.9f;
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            if (context.Animator.GetNormalizedTime(animationTag) > normalizedTimeThreshold)
                return true;
            
            return false;
        }
    }
}
