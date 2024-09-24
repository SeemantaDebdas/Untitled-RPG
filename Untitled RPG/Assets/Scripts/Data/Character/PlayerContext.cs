using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public PlayerInput PlayerInput { get; set; }
        public VFXHandler VFXHandler { get; set; }
        //public CharacterPhysicsHandler PhysicsHandler { get; set; }
        //public CombatHandler CombatHandler { get; set; }
        //public WeaponHandler WeaponHandler { get; set; }
    }
}
