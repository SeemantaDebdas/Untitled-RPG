using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerRangedFireState : PlayerBaseState
    {
        RangedWeaponSO rangedWeapon;
        ProjectileThrower thrower;
        public override void Enter()
        {
            base.Enter();

            thrower = GetComponentInParent<ProjectileThrower>();

            rangedWeapon = weaponHandler.CurrentWeapon as RangedWeaponSO;
            animator.PlayAnimation(rangedWeapon.FireAnimation, 0.1f, rangedWeapon.DrawFireAnimationLayer);

            Weapon currentWeaponInstance = weaponHandler.CurrentWeaponInstance;
            thrower.ThrowObject(rangedWeapon.Projectile, currentWeaponInstance.ShootPoint.position, currentWeaponInstance.ShootPoint.forward);
        }

        public override void Exit()
        {
            base.Exit();

            thrower.ResetForce();
            animator.SetLayerWeightOverTime(1, 0.25f, rangedWeapon.AnimationLayer);
            animator.SetLayerWeightOverTime(0, 0.25f, rangedWeapon.DrawFireAnimationLayer);
        }
    }
}
