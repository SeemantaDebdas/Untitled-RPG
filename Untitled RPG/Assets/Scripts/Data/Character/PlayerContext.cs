using RPG.Combat.Rework;
using RPG.Core;
using RPG.DialogueSystem;
using RPG.Inventory;
using RPG.Quest;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public InputReader InputReader { get; set; }
        public VFXHandler VFXHandler { get; set; }
        public PlayerConversant PlayerConversant { get; set; }
        public QuestList QuestList { get; set; }
        public TargetHandler TargetHandler { get; set; }
        public GridInventoryController Inventory { get; set; }
    }
}
