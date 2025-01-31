using RPG.Combat.Rework;
using RPG.Core;
using RPG.Data;
using UnityEngine;

namespace RPG.Control
{
    public class CharacterBaseState : State
    {
        protected CharacterController controller;
        protected CharacterPhysicsHandler physicsHandler;
        protected Animator animator;
        protected FieldOfView fieldOfView;
        protected WeaponHandler weaponHandler;
        protected CombatHandler combatHandler;
        protected EnvironmentScanner environmentScanner;

        protected float slopeSpeedMultiplier = 1f;

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);

            CharacterContext context = statemachine.Context as CharacterContext;
            
            controller = context.CharacterController;
            physicsHandler = context.PhysicsHandler;
            animator = context.Animator;
            fieldOfView = context.FieldOfView;
            weaponHandler = context.WeaponHandler;
            combatHandler = context.CombatHandler;
            environmentScanner = context.EnvironmentScanner;
        }

        protected void HandleMovement(float baseSpeed = 1f)
        {
            float movementSpeed = GetMovementSpeed(baseSpeed);

            if (controller == null)
            {
                Debug.LogWarning("Character Controller cannot be initialized");
                return;
            }
            
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

        protected virtual void FaceDirection(Vector3 movement, float rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }

    }
}
