using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "IsVelocityAboveThreshold", menuName = "Condition/Is Velocity Above Threshold", order = 2)]
    public class IsVelocityAboveThreshold : ConditionSO
    {
        [SerializeField] float threshold = 1.0f;
        [SerializeField] private ScriptableFloat thresholdFloat;
        [SerializeField] private float thresholdOffset = 0.1f;
        [SerializeField] bool onlyYVelocity = false;
        public override void Initialize(Context context)
        {
            
        }

        public override void Reset()
        {
            
        }

        protected override bool ProcessCondition(Context context)
        {
            Vector3 velocity = (context as CharacterContext).CharacterController.velocity;
            
            if (onlyYVelocity)
            {
                return velocity.y > threshold;

                //float yVelocity = (context as PlayerContext).PhysicsHandler.Movement.y;

                //return yVelocity > threshold;
            }

            if(thresholdFloat != null)
                return velocity.magnitude > threshold - thresholdOffset;
            
            return velocity.magnitude > threshold;
        }
    }
}
