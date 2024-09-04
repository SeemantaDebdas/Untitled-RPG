using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Data
{
    public class EnemyContext : Context
    {
        public NavMeshAgent Agent { get; set; }
        public CharacterController CharacterController { get; set; }
        public CharacterPhysicsHandler PhysicsHandler { get; set; }
        public Path Path { get; set; }
    }
}
