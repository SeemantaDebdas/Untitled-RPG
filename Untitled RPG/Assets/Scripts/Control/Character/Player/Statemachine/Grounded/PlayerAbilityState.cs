using UnityEngine;

namespace RPG.Control
{
    public class PlayerAbilityState : PlayerBaseState
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entered PlayerAbilityState");
        }
    }
}
