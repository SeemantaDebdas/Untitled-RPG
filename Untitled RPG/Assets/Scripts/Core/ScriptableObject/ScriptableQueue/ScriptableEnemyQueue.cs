using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewEnemyQueue", menuName = "Debdas/Variables/Queue/Enemy Queue")]
    public class ScriptableEnemyQueue : ScriptableQueue<IStatemachine>
    {
    }
}
