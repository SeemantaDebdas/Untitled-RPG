using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Debdas/Quest")]
    public class QuestSO : ScriptableObject
    {
        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }
        [field: SerializeField] public List<Objective> Objectives { get; private set; } = new();

        public string Title
        {
            get => name;
        }

        public bool HasObjective(string objectiveRef, out Objective matchedObjective)
        {
            matchedObjective = null;
            foreach (var objective in Objectives)
            {
                if (objective.reference == objectiveRef)
                {
                    matchedObjective = objective;
                    return true;
                }
            }

            return false;
        }
    }
}
