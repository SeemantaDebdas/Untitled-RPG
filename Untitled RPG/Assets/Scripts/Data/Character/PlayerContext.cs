using RPG.Core;
using RPG.DialogueSystem;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public InputReader InputReader { get; set; }
        public VFXHandler VFXHandler { get; set; }
        public PlayerConversant PlayerConversant { get; set; }
    }
}
