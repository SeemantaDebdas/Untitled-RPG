using RPG.Control;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG
{
    //This is temporary. Replace this by just using HasAnimationEnded and using ScriptableObject variable as layerIndex
    [CreateAssetMenu(fileName = "HasAnimationEndedForWeapon", menuName = "Condition/Animation/Has Animation Ended For Weapon", order = 1)]
    public class HasAnimationEndedForWeapon : ConditionSO
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
            CharacterContext characterContext = context as CharacterContext;

            int layer = characterContext.WeaponHandler.CurrentWeapon.AnimationLayer;

            if (characterContext.Animator.GetNormalizedTime(animationTag, layer) > normalizedTimeThreshold)
                return true;

            return false;
        }
    }
}
