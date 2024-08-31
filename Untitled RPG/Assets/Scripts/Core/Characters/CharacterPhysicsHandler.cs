using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class CharacterPhysicsHandler : MonoBehaviour
    {
        [SerializeField] float defaultGravityMultiplier = 2f;
        float currentGravityMultiplier;

        [SerializeField] float accelerationRate = 5f, decelerationRate = 4f;
        [SerializeField] float drag = 0.3f;

        [Range(-10f, -1f)]
        [SerializeField] float defaultYVelocityLimit = -10f;
        float yVelocityLimit;
        [SerializeField] string debugText = "Debug";

        CharacterController controller;
        float velocityY = 0;

        Vector3 addedForce = default;
        Vector3 dampingVelocity = default;
        public Vector3 Movement => addedForce + Vector3.up * velocityY;

        #region Calculate Velocity

        Vector3 currentPosition = default, previousPosition = default;
        public Vector3 TargetVelocity { get; private set; } = default;
        public Vector3 DampedVelocity { get; private set; } = default;

        #endregion Calculate Velocity

        #region Calculate Delta Velocity

        public Vector3 DeltaVelocity { get; private set; } = default;
        Vector3 previousVelocity;

        #endregion Calculate Delta Velocity

        NavMeshAgent agent = null;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();

            currentGravityMultiplier = defaultGravityMultiplier;

            yVelocityLimit = defaultYVelocityLimit;
        }

        private void Update()
        {
            addedForce = Vector3.SmoothDamp(addedForce, Vector3.zero, ref dampingVelocity, drag * Time.deltaTime);

            debugText = Movement.magnitude.ToString();

            if (agent != null && addedForce == Vector3.zero)
            {
                agent.enabled = true;
            }

            if (velocityY < 0f && controller.isGrounded)
            {
                velocityY = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                velocityY += Physics.gravity.y * currentGravityMultiplier * Time.deltaTime;

                if (velocityY < yVelocityLimit)
                    velocityY = yVelocityLimit;
            }

            CalculateVelocity();
        }

        private void FixedUpdate()
        {
            CalculateDeltaVelocity();
        }

        void CalculateDeltaVelocity()
        {
            DeltaVelocity = controller.velocity - previousVelocity;
            previousVelocity = controller.velocity;
        }

        public void AddForce(Vector3 force)
        {
            addedForce = force;
            if (agent != null)
            {
                agent.enabled = false;
            }
        }

        public void JumpByForce(float jumpForce)
        {
            velocityY += jumpForce;
        }

        public void JumpByHeight(float jumpHeight)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        public void ResetVelocity()
        {
            ResetHorizontalVelocity();
            ResetVerticalVelocity();
            controller.SimpleMove(Vector3.zero);
        }

        public void ResetHorizontalVelocity()
        {
            addedForce = Vector3.zero;
        }

        public void ResetVerticalVelocity()
        {
            velocityY = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravityMultiplier">0 <= gravityMultiplier <= 2</param>
        public void SetGravityMultiplier(float gravityMultiplier)
        {
            currentGravityMultiplier = Mathf.Clamp(gravityMultiplier, 0f, 2f);
        }

        public void ResetGravityMultiplier()
        {
            currentGravityMultiplier = defaultGravityMultiplier;
        }

        /// <summary>
        /// Set negative value
        /// </summary>
        /// <param name="limit"> -10 <= limit <= 0</param>
        public void SetYVelocityLimit(float limit)
        {
            yVelocityLimit = Mathf.Clamp(limit, -10, 0f);
        }

        public void ResetYVelocityLimit()
        {
            yVelocityLimit = defaultYVelocityLimit;
        }

        void CalculateVelocity()
        {
            currentPosition = transform.position;
            TargetVelocity = (currentPosition - previousPosition) / Time.deltaTime;

            if (TargetVelocity.magnitude > DampedVelocity.magnitude)
            {
                //accelerate
                DampedVelocity = Vector3.MoveTowards(DampedVelocity, TargetVelocity, accelerationRate * Time.deltaTime);
            }
            else
            {
                //decelerate
                DampedVelocity = Vector3.MoveTowards(DampedVelocity, TargetVelocity, decelerationRate * Time.deltaTime);
            }

            previousPosition = currentPosition;
        }
    }
}
