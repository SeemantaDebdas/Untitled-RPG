using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewEnemyList", menuName = "Variables/List/EnemyList")]
    public class ScriptableEnemyList : ScriptableList<IStatemachine>//move IStatemachine to Core layer since it can be exported to other 
    {
    }
}
