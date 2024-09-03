using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Data
{
    public class PlayerContext : Context
    {
        public PlayerInput PlayerInput { get; set; }
        public CharacterController CharacterController { get; set; }
        public CharacterPhysicsHandler PhysicsHandler { get; set; }
        public EnvironmentScanner EnvironmentScanner { get; set; }
        //public VFXHandler VFXHandler { get; set; }
        //public CharacterPhysicsHandler PhysicsHandler { get; set; }
        //public CombatHandler CombatHandler { get; set; }
        //public WeaponHandler WeaponHandler { get; set; }
    }
}
