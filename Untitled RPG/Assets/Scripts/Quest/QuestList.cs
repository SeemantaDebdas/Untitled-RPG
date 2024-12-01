using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Quest
{
    public class QuestList : MonoBehaviour
    {
        [field: SerializeField] public List<QuestStatus> QuestStatusList { get; private set; }
    }
}
