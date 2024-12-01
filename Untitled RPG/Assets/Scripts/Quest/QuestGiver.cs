using UnityEngine;

namespace RPG.Quest
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] QuestSO quest;

        public void GiveQuest()
        {
            Debug.Log("Giving Quest: " + quest.Title);
        }
    }
}
