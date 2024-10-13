namespace RPG.Control
{
    public class PlayerHeavyAttackState : PlayerAttackState
    {
        public override void Enter()
        {
            attack = weaponHandler.GetHeavyAttack();
            
            base.Enter();
        }
    }
}
