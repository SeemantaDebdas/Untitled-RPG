using UnityEngine;

namespace RPG.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
    public class QuestSO : ScriptableObject
    {
        [field: SerializeField] public string[] Objectives { get; private set; }

        public string Title
        {
            get => name;
        }
    }
}
