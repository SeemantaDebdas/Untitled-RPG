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
            Debug.Log("Complete Objective called on Quest UI");
            
            QuestStatus questStatus = GetStatus(quest);

            questStatus?.CompleteObjective(objectiveName);
            
            OnQuestListUpdated?.Invoke();
        }

        public bool HasQuest(QuestSO quest)
        {
            foreach (QuestStatus questStatus in QuestStatusList)
            {
                if (questStatus.Quest == quest)
                {
                    Debug.Log("Has Quest: " + quest.name);
                    return true;
                }
            }
            
            Debug.Log("Does not have quest: " + quest.name);

            return false;
        }

        private QuestStatus GetStatus(QuestSO quest)
        {
            return QuestStatusList.FirstOrDefault(questStatus => questStatus.Quest == quest);
        }
    }
}
