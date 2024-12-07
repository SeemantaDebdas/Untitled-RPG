using RPG.Core;
using RPG.DialogueSystem;
using RPG.Quest;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public InputReader InputReader { get; set; }
        public VFXHandler VFXHandler { get; set; }
        public PlayerConversant PlayerConversant { get; set; }
        public QuestList QuestList { get; set; }
    }
}
