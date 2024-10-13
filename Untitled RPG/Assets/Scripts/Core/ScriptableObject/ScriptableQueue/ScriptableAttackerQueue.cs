using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "NewAttackerQueue", menuName = "Variables/Queue/Attacker Queue")]
    public class ScriptableAttackerQueue : ScriptableQueue<CombatHandler>
    {
    }
}
