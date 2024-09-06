using RPG.Core;
using UnityEngine.AI;

namespace RPG.Data
{
    public class EnemyContext : CharacterContext
    {
        public NavMeshAgent Agent { get; set; }
        public Path Path { get; set; }
    }
}
