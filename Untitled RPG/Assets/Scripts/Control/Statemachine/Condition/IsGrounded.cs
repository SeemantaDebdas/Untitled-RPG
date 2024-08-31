using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsGrounded", menuName = "Condition/Is Grounded", order = 1)]
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
            return (context as PlayerContext).CharacterController.isGrounded;

            if(Physics.Raycast(context.Transform.position, Vector3.down,raycastDistance, groundLayer))
            {
                return true;
            }

            return false;
        }
    }
}
