using UnityEngine;

namespace RPG.Control
{
    public class PlayerWalkState : PlayerBaseState
    {
        [SerializeField] float baseSpeed = 1f;
        [SerializeField] float rotationSpeed = 6f;

        public override void Enter()
        {
            base.Enter();

            //PlayAnimation(animationName);
        }

        public override void Tick()
        {
            base.Tick();
            HandleMovement(baseSpeed);
            FaceMovementDirection(CalculateDirection(), rotationSpeed);
        }
    }
}
