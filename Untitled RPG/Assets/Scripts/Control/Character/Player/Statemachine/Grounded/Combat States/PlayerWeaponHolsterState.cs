using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerWeaponHolsterState : PlayerBaseState
    {
        [SerializeField] ScriptableString animationName;

        public override void Enter()
        {
            base.Enter();

            animator.PlayAnimation(animationName.Value, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            //animator.SetLayerWeightOverTime(0, layer: weaponHandler.CurrentWeapon.WeaponData.AnimationLayer);
        }
    }
}
