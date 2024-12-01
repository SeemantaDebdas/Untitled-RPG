using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Quest
{
    [System.Serializable]
    public class QuestStatus
    {
        [field: SerializeField] public QuestSO Quest { get; private set; }
        [field: SerializeField] public List<string> CompletedObjectiveList { get; private set; }
    }
}
