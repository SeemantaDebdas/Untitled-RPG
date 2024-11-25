using RPG.Core;
using RPG.DialogueSystem;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public PlayerInput PlayerInput { get; set; }
        public VFXHandler VFXHandler { get; set; }
        public PlayerConversant PlayerConversant { get; set; }
    }
}
