using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Quest.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questTitle;
        private Button button;
        
        private QuestStatus questStatus;

        public static event Action<QuestStatus> OnAnyQuestItemSelected;
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            button.onClick.AddListener(() => OnAnyQuestItemSelected?.Invoke(questStatus));
        }
        
        public void Setup(QuestStatus questStatus)
        {
            this.questStatus = questStatus;
            questTitle.text = questStatus.Quest.name;
        }

        public void Select()
        {
            OnAnyQuestItemSelected?.Invoke(questStatus);
        }
    }
}
