using RPG.Core;
using RPG.Data;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyCombatCirclingState : EnemyBaseState
    {
        [SerializeField] float moveSpeed = 0.9f;
        [SerializeField] float rotationSpeed = 6f;
        [SerializeField] float obstacleAvoidRadius = 0.5f;
        [SerializeField] float targetDistanceOffsetFromPlayer = 0.5f;
        [SerializeField] float minimumDistanceFromPlayer = 2f;
        [SerializeField] float velocityReachTime = 5f;


        int circleDirectionMultiplier = 1;

        [SerializeField] ScriptableString animationName;

        Vector3 targetVelocity = Vector3.zero;
        Vector3 currentVelocity = Vector3.zero;
        Transform closestTarget;

        public override void Enter()
        {
            base.Enter();

            circleDirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;

            if(!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName.Value))
            {
                //coming from attack state
                animator.PlayAnimation(animationName.Value);
            }
        }

        public override void Tick()
        {
            if(EvaluateTransitions())
            {
                return;
            }
            
            closestTarget = attackFOV.GetClosestTarget();

            Vector3 moveDir = Vector3.Cross(Vector3.up, CalculateDirection());
            targetVelocity = circleDirectionMultiplier * moveSpeed * moveDir;
            targetVelocity.y = 0;

            
            if (!IsObstacleBehind())
            {
                //we have to handle this in some other way. maybe through conditions. If there's and enemy with higher priority on right and it's moving, then start moving
            }
            MaintainDistanceFromPlayer();

            CheckObstacleWithFOV();


            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, velocityReachTime * Time.deltaTime);

            Move(currentVelocity);

            FaceMovementDirection(CalculateDirection(), rotationSpeed);

            HandleAnimation();
        }

        private void CheckForObstacle()
        {
            //Modify the move Direction to account for obstacles.
            //------

            //-------

            //--------

            Vector3 raycastDirection = targetVelocity; //storing it since this will get changed after raycasting

            Vector3 origin = context.Transform.position + Vector3.up * controller.height / 2;

            if (Physics.SphereCast(origin, obstacleAvoidRadius, raycastDirection * 2f, out RaycastHit sideHitInfo))
            {
                if(Vector3.Distance(context.Transform.position, closestTarget.position) <= minimumDistanceFromPlayer)
                {
                    //it's better to change direction then
                    return;
                }

                //Go closer to player if there's a player with higher priority on your move direction.
                //Go closer to player if there's an obstacle on your move direction
                if (!sideHitInfo.collider.TryGetComponent(out NavMeshAgent otherAgent) || otherAgent.avoidancePriority < agent.avoidancePriority)
                {
                    Debug.Log(sideHitInfo.collider.name, gameObject);
                    targetVelocity = CalculateDirection();
                    //might want to add additional checks to see if there's something infront
                    //or if there's an enemy already infront of us
                    //or if we're too close to the player
                    //then just switch directions and head back
                }
            }
            else
            {
                //Maintain a distance of attackRadius - 0.5f from player

                //If there's nothing in way, check if you're at a certain distnace from player. If not, go to that range
                //range = attackFov.radius - 0.1f or something

                MaintainDistanceFromPlayer();
            }
        }

        void CheckObstacle()
        {
            Vector3 origin = context.Transform.position + Vector3.up * controller.height / 2;
            bool hitSide = Physics.SphereCast(origin, obstacleAvoidRadius, context.Transform.right * circleDirectionMultiplier, out RaycastHit sideHitInfo, 2f);
            bool hitBack = Physics.SphereCast(origin, obstacleAvoidRadius, -context.Transform.forward * circleDirectionMultiplier, out RaycastHit backHitInfo, 2f);

            if (hitSide)
            {
                Debug.Log("Hit Side");
                //if obstacle then move in the opposite direction
                if (!sideHitInfo.collider.TryGetComponent(out NavMeshAgent otherAgent)) 
                {
                    targetVelocity = -targetVelocity;
                }
                //steer past the other agent.
                else if(otherAgent.avoidancePriority < agent.avoidancePriority)
                {
                    Debug.Log(context.Transform.name + "Should move closer to player", context.Transform.gameObject);
                    targetVelocity += CalculateDirection() * moveSpeed;//go closer to player
                }
            }
            //if there's nothing in the back then maintain distnace from the player
            else if (!hitBack)
            {
                MaintainDistanceFromPlayer();
            }
        }

        void CheckObstacleWithFOV()
        {
            Transform obstacle = avoidanceFOV.GetClosestTarget();

            if (obstacle == null)
                return;

            Vector3 dirToTarget = obstacle.position - context.Transform.position;
            dirToTarget.y = 0;
            dirToTarget.Normalize();

            float angleFromForward = Vector3.Angle(dirToTarget, context.Transform.forward);

            if (context.Transform.root.name == "Enemy (1)")
                Debug.Log(angleFromForward);

            bool isObstacleOnTheSide = angleFromForward > 75f && angleFromForward < 150f;

            bool isEnemyMorePriority = false;
            if (obstacle.TryGetComponent(out NavMeshAgent otherAgent))
                isEnemyMorePriority = otherAgent.avoidancePriority < agent.avoidancePriority;

            if (isObstacleOnTheSide)
            {
                bool canOvertakeEnemy = isEnemyMorePriority && Vector3.Dot(targetVelocity, dirToTarget) >= 0;
                //additional checks for if other agent is moving or not
                if (canOvertakeEnemy && !IsTooCloseToPlayer())
                    targetVelocity = CalculateDirection();
            }
            //else if (!isObstacleBehind)
            //{
            //    Debug.Log("There's no obstacle behind");
            //    MaintainDistanceFromPlayer();
            //}
        }

        private void MaintainDistanceFromPlayer()
        {
            float agentPriorityOffset = agent.avoidancePriority * 0.5f;
            float targetDistanceFromPlayer = attackFOV.Radius - targetDistanceOffsetFromPlayer - agentPriorityOffset;

            float distanceFromPlayer = Vector3.Distance(context.Transform.position, closestTarget.position);

            if (distanceFromPlayer < targetDistanceFromPlayer)
            {
                //Vector3 origin = context.Transform.position + Vector3.up * controller.height / 2;
                
                //if (!Physics.SphereCast(origin, obstacleAvoidRadius, -CalculateDirection() * 2f, out RaycastHit _))
                targetVelocity -= CalculateDirection(); //Calculate direction is vector from enemy to player. We want to go the opposite way of the vector
            }
            else
            {
                targetVelocity += CalculateDirection();
            }
        }

        bool IsTooCloseToPlayer()
        {
            if (closestTarget == null)
                return false;

            float distanceFromPlayer = Vector3.Distance(context.Transform.position, closestTarget.position);

            return distanceFromPlayer < minimumDistanceFromPlayer; 
        }

        bool IsObstacleBehind()
        {
            Transform obstacle = avoidanceFOV.GetClosestTarget();

            if (obstacle == null)
                return false;

            Vector3 dirToTarget = obstacle.position - context.Transform.position;
            dirToTarget.y = 0;
            dirToTarget.Normalize();

            float angleFromForward = Vector3.Angle(dirToTarget, context.Transform.forward);
            return angleFromForward > 150f;
        }

        private void HandleAnimation()
        {
            animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveX, circleDirectionMultiplier, 0.085f, Time.deltaTime);
            animator.SetFloat(CharacterAnimationData.Instance.Locomotion.MoveY, context.Transform.InverseTransformDirection(controller.velocity).z, 0.085f, Time.deltaTime);

            //if(context.Transform.root.name == "Enemy (1)")
            //    Debug.Log(context.Transform.InverseTransformDirection(controller.velocity));
        }

        private void OnDrawGizmosSelected()
        {
            if (controller == null)
                return;

            Vector3 from = context.Transform.position + Vector3.up * controller.height / 2;
            Vector3 to = from + targetVelocity;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(from, to);

            Gizmos.DrawWireSphere(to, obstacleAvoidRadius);
        }
    }
}
