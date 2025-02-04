using RPG.Control;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    //This is temporary. Replace this by just using HasAnimationEnded and using ScriptableObject variable as layerIndex
    [CreateAssetMenu(fileName = "HasAnimationEndedForWeapon", menuName = "Condition/Animation/Has Animation Ended For Weapon", order = 1)]
    public class HasAnimationEndedForWeapon : ConditionSO
    {
        public override void Initialize(Context context)
        {

        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            CharacterContext characterContext = context as CharacterContext;

            return characterContext.WeaponHandler.HasCurrentAttackFinished();
        }
    }
}
