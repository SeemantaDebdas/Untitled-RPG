using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestList questList;
        [SerializeField] QuestItemUI questItemUIPrefab = null;
        
        private void OnEnable()
        {
            questList.OnQuestListUpdated += RegenerateQuestList;
        }

        private void OnDestroy()
        {
            questList.OnQuestListUpdated -= RegenerateQuestList;
        }

        private void Start()
        {
            RegenerateQuestList();
        }

        void RegenerateQuestList()
        {
            ClearQuestList();
            BuildQuestList();
        }

        private void BuildQuestList()
        {
            Debug.Log("Building quest list");
            
            if (questList.QuestStatusList.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < questList.QuestStatusList.Count; i++)
            {
                QuestStatus questStatus = questList.QuestStatusList[i];
                
                QuestItemUI questItemUI = Instantiate(questItemUIPrefab, transform);
                questItemUI.Setup(questStatus);

                if (i == 0)
                {
                    //if first element then select it so that it's details populate the QuestDetailPanel
                    questItemUI.Select();
                }
            }
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
