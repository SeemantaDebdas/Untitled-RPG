using RPG.Combat;
using RPG.Core;
using RPG.Data;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyBaseState : CharacterBaseState
    {
        protected EnemyContext context;
        protected NavMeshAgent agent;
        protected Path path;
        protected NavMeshPath navmeshPath;
        protected FieldOfView chaseFov, attackFOV, avoidanceFOV;

        protected Vector3 moveDirection = Vector3.zero;

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);

            context = statemachine.Context as EnemyContext;

            agent = context.Agent;
            path = context.Path;
            fieldOfView = context.FieldOfView;
            chaseFov = context.ChaseFOV;
            attackFOV = context.AttackFOV;
            avoidanceFOV = context.AvoidanceFOV;

            //Debug.Assert(weaponHandler != null, "weaponHandler is null");
        }

        public override void Enter()
        {
            base.Enter();

            //Debug.Log("Enter: " + GetType().Name);
        }

        protected override Vector3 CalculateDirection()
        {
            Vector3 calculateStartPos = transform.position;

            if (agent == null)
            {
                Debug.LogWarning("No agent found");
                return Vector3.zero;
            }
            
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, controller.height * 2, NavMesh.AllAreas))
            {
                calculateStartPos = hit.position;
            }

            #region very old code
            //if (NavMesh.CalculatePath(calculateStartPos, path.GetCurrentWaypoint(), NavMesh.AllAreas, navmeshPath))
            //{
            //    Vector3 nextPosition = navmeshPath.corners[1];
            //    Debug.Log(nextPosition);

            //    moveDirection = (nextPosition - context.Transform.position).normalized;

            //    moveDirection = ProjectDirectionOnPlane(moveDirection);

            //    for (int i = 0; i < navmeshPath.corners.Length - 1; i++)
            //        Debug.DrawLine(navmeshPath.corners[i], navmeshPath.corners[i + 1], Color.red);
            //}
            #endregion


            // Check if the agent has a valid path and the path status is complete
            if (agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                moveDirection = (agent.steeringTarget - calculateStartPos).normalized;
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.LogWarning("Path is partial. Agent can't fully reach the target.");
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.LogWarning("Invalid path. No path found or unreachable target.");
            }
            else
            {
                //Debug.LogWarning("Agent has no valid path. Still calculating or destination not set.");
            }


            return moveDirection;
        }

        Vector3 ProjectDirectionOnPlane(Vector3 dir)
        {
            if(Physics.Raycast(context.Transform.position + context.Transform.up * environmentScanner.UpOffsetFromPlayerBase,
                                Vector3.down, out RaycastHit hit,
                                environmentScanner.MaxHeightForDownRaycast + 0.1f,
                                environmentScanner.EnvironmentLayer))
            {
                Vector3 projectedVector = Vector3.ProjectOnPlane(dir, hit.normal);
                //Debug.Log(projectedVector);
                return projectedVector;
            }

            return dir;
        }

        protected override void FaceDirection(Vector3 movement, float rotationSpeed)
        {
            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }
    }
}
