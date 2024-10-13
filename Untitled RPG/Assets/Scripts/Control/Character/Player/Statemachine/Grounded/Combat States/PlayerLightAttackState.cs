namespace RPG.Control
{
    public class PlayerLightAttackState : PlayerAttackState
    {

        public override void Enter()
        {
            attack = weaponHandler.GetLightAttack();
            base.Enter();


            //AudioManager.Instance.PlayOneShot(weaponHandler.CurrentWeapon.swooshSound, context.Transform.position);
        }  
    }
}
