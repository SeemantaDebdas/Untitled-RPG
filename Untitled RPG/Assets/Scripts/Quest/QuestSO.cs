using UnityEngine;

namespace RPG.Quest
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] private string[] objectives;
    }
}
