using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerJumpState : PlayerBaseState
    {
        [SerializeField] float rotationSpeed = 20f;
        [SerializeField] float jumpForce = 10f;

        Vector3 momentum = Vector3.zero;
        public override void Enter()
        {
            base.Enter();

            momentum = controller.velocity;
            momentum.y = 0f;

            physicsHandler.ResetVelocity();
            Jump();

            animator.PlayAnimation(CharacterAnimationData.Instance.Locomotion.Jump);

        }

        public override void Exit()
        {
            base.Exit();

            context.PhysicsHandler.ResetHorizontalVelocity();
        }

        public override void Tick()
        {

            HandleMovement();

            if (InputReader.MoveInput.magnitude > 0.1f)
            {
                FaceDirection(CalculateDirection(), rotationSpeed);
            }

            //context.AnimationData.JumpParameter = context.Controller.velocity.y / maxVelocityY;
            //context.Animator.SetFloat(Parameters.velocityY, context.Controller.velocity.y / maxVelocityY, FAST_FADE, Time.deltaTime);

            //float xzVelocityMagnitude = (context.ReusableData.CurrentJumpForce.x + context.ReusableData.CurrentJumpForce.z) / 2;

            //context.Animator.SetFloat(Parameters.velocityXZ, xzVelocityMagnitude, FAST_FADE, Time.deltaTime);
            base.Tick();
        }

        void Jump()
        {
            momentum.y = jumpForce;

            //Vector3 jumpDirection = context.Transform.forward;

            //// Normalize the horizontal component of playerForward
            //Vector3 horizontalForward = new Vector3(jumpDirection.x, 0, jumpDirection.z).normalized;

            //// Ensures Directional Jump 
            //jumpVelocity.x *= horizontalForward.x;
            //jumpVelocity.z *= horizontalForward.z;


            context.PhysicsHandler.AddForce(momentum);
        }

    }

}
