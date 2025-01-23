namespace RPG.Control
{
    public class PlayerLightAttackState : PlayerAttackState
    {

        public override void Enter()
        {
            base.Enter();

            combatHandler.PerformAttack(false);


            //AudioManager.Instance.PlayOneShot(weaponHandler.CurrentWeapon.swooshSound, context.Transform.position);
        }  
    }
}
