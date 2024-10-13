using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewEnemyQueue", menuName = "Variables/Queue/Enemy Queue")]
    public class ScriptableEnemyQueue : ScriptableQueue<IStatemachine>
    {
    }
}
