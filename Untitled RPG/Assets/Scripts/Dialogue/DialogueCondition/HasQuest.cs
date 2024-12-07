using RPG.Data;
using RPG.Quest;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "HasQuest", menuName = "Condition/Dialogue/Has Quest", order = 1)]
    public class HasQuest : DialogueConditionSO
    {
        [SerializeField] private QuestSO quest;
        public override void Initialize(Context context)
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            bool result = (context as PlayerContext).QuestList.HasQuest(quest);
            Debug.Log(name + " has quest: " + result);
            
            return result;
        }

        public override void Reset()
        {

        }
    }
}
