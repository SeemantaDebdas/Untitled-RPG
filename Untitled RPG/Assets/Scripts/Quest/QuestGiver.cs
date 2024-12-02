using UnityEngine;

namespace RPG.Quest
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] QuestSO quest;

        public void GiveQuest(Component caller, object data)
        {
            QuestStatus questStatus = new QuestStatus(quest);
            caller.GetComponent<QuestList>().AddQuest(questStatus);
        }
    }
}
