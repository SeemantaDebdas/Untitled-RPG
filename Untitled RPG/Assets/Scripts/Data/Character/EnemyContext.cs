using RPG.Core;
using UnityEngine.AI;

namespace RPG.Data
{
    public class EnemyContext : CharacterContext
    {
        public NavMeshAgent Agent { get; set; }
        public Path Path { get; set; }
        public FieldOfView ChaseFOV { get; set; }
        public FieldOfView AttackFOV { get; set; }  
        public FieldOfView AvoidanceFOV { get; set; }
        public CharacterDamageHandler CharacterDamageHandler { get; set; }
    }
}
