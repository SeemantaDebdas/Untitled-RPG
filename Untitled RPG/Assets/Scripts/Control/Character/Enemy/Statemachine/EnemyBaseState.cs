using RPG.Core;
using RPG.Data;
using System;
using System.Collections;
using System.Collections.Generic;
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
        EnvironmentScanner scanner;

        protected Vector3 moveDirection = Vector3.zero;

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);

            context = statemachine.Context as EnemyContext;
            animator = context.Animator;
            controller = context.CharacterController;
            agent = context.Agent;
            path = context.Path;
            physicsHandler = context.PhysicsHandler;
            scanner = context.EnvironmentScanner;
            fieldOfView = context.FieldOfView;

            Debug.Assert(agent != null, "agent is null");
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


            //if (NavMesh.CalculatePath(calculateStartPos, path.GetCurrentWaypoint(), NavMesh.AllAreas, navmeshPath))
            //{
            //    Vector3 nextPosition = navmeshPath.corners[1];
            //    Debug.Log(nextPosition);

            //    moveDirection = (nextPosition - context.Transform.position).normalized;

            //    moveDirection = ProjectDirectionOnPlane(moveDirection);

            //    for (int i = 0; i < navmeshPath.corners.Length - 1; i++)
            //        Debug.DrawLine(navmeshPath.corners[i], navmeshPath.corners[i + 1], Color.red);
            //}

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
            if(Physics.Raycast(context.Transform.position + context.Transform.up * scanner.UpOffsetFromPlayerBase,
                                Vector3.down, out RaycastHit hit,
                                scanner.MaxHeightForDownRaycast + 0.1f,
                                scanner.EnvironmentLayer))
            {
                Vector3 projectedVector = Vector3.ProjectOnPlane(dir, hit.normal);
                //Debug.Log(projectedVector);
                return projectedVector;
            }

            return dir;
        }

        protected override void FaceMovementDirection(Vector3 movement, float rotationSpeed)
        {
            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }
    }
}
