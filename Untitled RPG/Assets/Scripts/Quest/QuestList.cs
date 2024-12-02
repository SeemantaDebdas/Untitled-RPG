using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quest
{
    public class QuestList : MonoBehaviour
    {
        public List<QuestStatus> QuestStatusList { get; private set; } = new();
        public event Action OnQuestListUpdated;

        private void OnEnable()
        {
            QuestCompletion.OnAnyObjectiveComplete += CompleteObjective;
        }

        private void OnDestroy()
        {
            QuestCompletion.OnAnyObjectiveComplete -= CompleteObjective;
        }

        public void AddQuest(QuestStatus quest)
        {
            if(QuestStatusList.Contains(quest))
                return;
            
            Debug.Log("Quest Added");
            
            QuestStatusList.Add(quest);       
            OnQuestListUpdated?.Invoke();
        }

        public void CompleteObjective(QuestSO quest, string objectiveName)
        {
            QuestStatus questStatus = GetStatus(quest);

            questStatus?.CompleteObjective(objectiveName);
            
            OnQuestListUpdated?.Invoke();
        }

        private QuestStatus GetStatus(QuestSO quest)
        {
            return QuestStatusList.FirstOrDefault(questStatus => questStatus.Quest == quest);
        }
    }
}
