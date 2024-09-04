using RPG.Core;
using RPG.Data;
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

        public override void Initialize(IStatemachine statemachine)
        {
            base.Initialize(statemachine);

            context = statemachine.Context as EnemyContext;
            animator = context.Animator;
            controller = context.CharacterController;
            agent = context.Agent;
            path = context.Path;
            physicsHandler = context.PhysicsHandler;    

            navmeshPath = new NavMeshPath();
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Enter: " + GetType().Name);
        }

        protected override Vector3 CalculateDirection()
        {
            
            if (NavMesh.CalculatePath(transform.position, path.GetCurrentWaypoint(), NavMesh.AllAreas, navmeshPath))
            {
                Debug.Log("Can go");

                Vector3 nextPosition = navmeshPath.corners[1];

                Vector3 dir = (nextPosition - transform.position).normalized;

                for (int i = 0; i < navmeshPath.corners.Length - 1; i++)
                    Debug.DrawLine(navmeshPath.corners[i], navmeshPath.corners[i + 1], Color.red);

                return dir;
            }

            return base.CalculateDirection();
        }

        protected override void FaceMovementDirection(Vector3 movement, float rotationSpeed)
        {
            context.Transform.rotation = Quaternion.Slerp(context.Transform.rotation,
                                                Quaternion.LookRotation(movement),
                                                rotationSpeed * Time.deltaTime);
        }
    }
}
