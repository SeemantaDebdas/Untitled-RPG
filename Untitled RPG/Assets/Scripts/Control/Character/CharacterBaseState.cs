using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class CharacterBaseState : State
    {
        protected CharacterController controller;
        protected CharacterPhysicsHandler physicsHandler;
        protected Animator animator;

        protected float slopeSpeedMultiplier = 1f;

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);
        }

        protected void HandleMovement(float baseSpeed = 1f)
        {
            float movementSpeed = GetMovementSpeed(baseSpeed);

            Move(movementSpeed * CalculateDirection());
        }

        protected void Move()
        {
            controller.Move(physicsHandler.Movement * Time.deltaTime);
        }

        protected void Move(Vector3 motion)
        {
            controller.Move((physicsHandler.Movement + motion) * Time.deltaTime);
        }

        protected float GetMovementSpeed(float baseSpeed)
        {
            float movementSpeed = baseSpeed * slopeSpeedMultiplier;
            return movementSpeed;
        }

        protected virtual Vector3 CalculateDirection() { return Vector3.zero; }

        protected virtual void FaceMovementDirection(Vector3 movement, float rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }

    }
}
