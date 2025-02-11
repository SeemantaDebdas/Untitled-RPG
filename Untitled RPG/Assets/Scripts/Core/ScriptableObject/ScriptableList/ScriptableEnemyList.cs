using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewEnemyList", menuName = "Debdas/Variables/List/EnemyList")]
    public class ScriptableEnemyList : ScriptableList<Transform>//move IStatemachine to Core layer since it can be exported to other 
    {
    }
}
