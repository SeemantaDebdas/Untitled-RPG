using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quest
{
    [System.Serializable]
    public class QuestStatus
    {
        public QuestSO Quest { get; private set; }
        public List<QuestSO.Objective> CompletedObjectiveList { get; private set; } = new();
        
        public QuestStatus(QuestSO quest)
        {
            Quest = quest;
        }

        public void CompleteObjective(string objectiveRef)
        {
            if (!Quest.HasObjective(objectiveRef, out var objective))
                return;
            
            if (CompletedObjectiveList.Contains(objective))
                return;
            
            CompletedObjectiveList.Add(objective);
        }

        public bool HasCompletedObjective(string objectiveRef)
        {
            foreach (QuestSO.Objective objective in CompletedObjectiveList)
            {
                if (objective.reference == objectiveRef)
                    return true;
            }

            return false;
        }
    }
}
