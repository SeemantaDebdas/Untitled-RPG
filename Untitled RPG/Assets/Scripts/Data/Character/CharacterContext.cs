using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Data
{
    public class CharacterContext : Context
    {
        public Animator Animator { get; set; }
        public CharacterController CharacterController { get; set; }
        public CharacterPhysicsHandler PhysicsHandler { get; set; }
        public WeaponHandler WeaponHandler { get; set; }
        public EnvironmentScanner EnvironmentScanner { get; set; }
        public FieldOfView FieldOfView { get; set; }
        public CombatHandler CombatHandler { get; set; }
        public Interactor Interactor { get; set; }
        public Health Health { get; set; }
    }
}
