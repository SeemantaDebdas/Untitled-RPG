using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerRangedFireState : PlayerBaseState
    {
        RangedWeaponSO rangedWeaponData;
        ProjectileThrower thrower;
        public override void Enter()
        {
            base.Enter();

            thrower = GetComponentInParent<ProjectileThrower>();

            rangedWeaponData = weaponHandler.CurrentWeapon.WeaponData as RangedWeaponSO;
            animator.PlayAnimation(rangedWeaponData.FireAnimation, 0.1f, rangedWeaponData.DrawFireAnimationLayer);

            Weapon currentWeaponInstance = weaponHandler.CurrentWeapon;
            thrower.ThrowObject(rangedWeaponData.Projectile, currentWeaponInstance.ShootPoint.position, currentWeaponInstance.ShootPoint.forward);
        }

        public override void Exit()
        {
            base.Exit();

            thrower.ResetForce();
            animator.SetLayerWeightOverTime(1, 0.25f, rangedWeaponData.AnimationLayer);
            animator.SetLayerWeightOverTime(0, 0.25f, rangedWeaponData.DrawFireAnimationLayer);
        }
    }
}
