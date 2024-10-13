using RPG.Combat;
using RPG.Core;
using RPG.Data;
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

            Debug.Assert(weaponHandler != null, "weaponHandler is null");
        }

        public override void Enter()
        {
            base.Enter();

            //Debug.Log("Enter: " + GetType().Name);
        }

        protected override Vector3 CalculateDirection()
        {
            Vector3 calculateStartPos = transform.position;

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


            if (agent.hasPath)
            {
                moveDirection = (agent.steeringTarget - calculateStartPos).normalized;
            }
            else
            {
                Debug.Log("No Path Found");
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
