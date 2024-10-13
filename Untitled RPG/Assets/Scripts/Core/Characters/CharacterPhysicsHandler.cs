using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class CharacterPhysicsHandler : MonoBehaviour
    {
        [SerializeField] float defaultGravityMultiplier = 2f;
        float currentGravityMultiplier;

        [SerializeField] float drag = 0.3f;


        [Range(-10f, -1f)]
        [SerializeField] float defaultYVelocityLimit = -10f;
        float yVelocityLimit;
        [SerializeField] string debugText = "Debug";
        
        [Header("Ground Detection")]
        [SerializeField] float groundedYVelocityCap = 0.2f;
        [SerializeField] float raycastDownDistance = 1.0f;
        [SerializeField] LayerMask groundLayer;

        CharacterController controller;
        float velocityY = 0;

        Vector3 addedForce = default;
        Vector3 dampingVelocity = default;
        public Vector3 Movement => addedForce + Vector3.up * velocityY;

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

            if (IsGrounded())
            { 
                velocityY = groundedYVelocityCap;
            }
            else
            {
                velocityY += Physics.gravity.y * currentGravityMultiplier * Time.deltaTime;

                if (velocityY < yVelocityLimit)
                    velocityY = yVelocityLimit;
            }

        }


        public void AddForce(Vector3 force)
        {
            addedForce = force;
            if (agent != null)
            {
                agent.enabled = false;
            }
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

        public bool IsGrounded()
        {
            //return controller.isGrounded;

            if (Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, raycastDownDistance, groundLayer))
            {
                return true;
            }

            return false;
        }
    }
}
