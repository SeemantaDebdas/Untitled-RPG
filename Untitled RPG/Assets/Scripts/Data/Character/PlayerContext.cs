using RPG.Core;

namespace RPG.Data
{
    public class PlayerContext : CharacterContext
    {
        public PlayerInput PlayerInput { get; set; }
        public VFXHandler VFXHandler { get; set; }
    }
}
