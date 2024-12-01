using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestList questList;
        [SerializeField] QuestItemUI questItemUIPrefab = null;

        private void Start()
        {
            ClearQuestList();

            for (int i = 0; i < questList.QuestStatusList.Count; i++)
            {
                QuestStatus questStatus = questList.QuestStatusList[i];
                QuestSO quest = questStatus.Quest;
                
                QuestItemUI questItemUI = Instantiate(questItemUIPrefab, transform);
                questItemUI.Setup(questStatus);

                if (i == 0)
                {
                    //if first element then select it so that it's details populate the QuestDetailPanel
                    questItemUI.Select();
                }
            }
        }

        private void OnEnable()
        {
            Debug.Log("Enabled QuestListUI");
        }

        private void ClearQuestList()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
