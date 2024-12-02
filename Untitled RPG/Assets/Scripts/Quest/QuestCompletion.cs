using System;
using UnityEngine;

namespace RPG.Quest
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private QuestSO quest;
        [SerializeField] private string objective;

        public static event Action<QuestSO, string> OnAnyObjectiveComplete;
        
        public void CompleteObjective()
        {
            OnAnyObjectiveComplete?.Invoke(quest, objective);
        }
    }
}
