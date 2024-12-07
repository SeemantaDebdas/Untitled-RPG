using System;
using TMPro;
using UnityEngine;

namespace RPG.Quest.UI
{
    public class QuestDetailsUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questTitle;
        [SerializeField] TextMeshProUGUI questDescription;
        
        [Space]
        [SerializeField] private Transform objectiveContainer;
        [SerializeField] private GameObject objectiveIncompletePrefab, objectiveCompletePrefab;

        private void OnEnable()
        {
            QuestItemUI.OnAnyQuestItemSelected += QuestItemUI_OnAnyQuestItemSelected;
        }

        private void OnDestroy()
        {
            QuestItemUI.OnAnyQuestItemSelected -= QuestItemUI_OnAnyQuestItemSelected;
        }

        private void QuestItemUI_OnAnyQuestItemSelected(QuestStatus questStatus)
        {
            Debug.Log("Quest Details UI: " + questStatus.Quest.name);
            
            questTitle.text = questStatus.Quest.Title;
            ClearObjectiveContainer();

            foreach (QuestSO.Objective objective in questStatus.Quest.Objectives)
            {
                string objectiveText = objective.description;
                
                var prefabToSpawn = questStatus.HasCompletedObjective(objective.reference) ? 
                    objectiveCompletePrefab : objectiveIncompletePrefab;
                
                GameObject objectivePrefabSpawn = Instantiate(prefabToSpawn, objectiveContainer.transform);
                objectivePrefabSpawn.GetComponentInChildren<TextMeshProUGUI>().text = objectiveText;
            }
        }

        private void ClearObjectiveContainer()
        {
            foreach (Transform child in objectiveContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
