using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsGrounded", menuName = "Condition/Detection/Is Grounded", order = 1)]
    public class IsGrounded : ConditionSO
    {
        [SerializeField] float raycastDistance = 1.0f;
        [SerializeField] LayerMask groundLayer;
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            if (context is CharacterContext characterContext)
            {
                return characterContext.PhysicsHandler.IsGrounded();
            }

            if (Physics.Raycast(context.Transform.position, Vector3.down,raycastDistance, groundLayer))
            {
                return true;
            }

            return false;
        }
    }
}
