using DG.Tweening;
using RPG.Combat;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerLightAttackState : PlayerAttackState
    {

        public override void Enter()
        {
            attack = weaponHandler.GetLightAttack();
            base.Enter();
        }  
    }
}
