using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyStatemachine : Statemachine
    {
        EnemyContext enemyContext;

        public override Context Context
        {
            get
            {
                if (enemyContext != null)
                    return enemyContext;

                enemyContext = new EnemyContext
                {
                    Transform = transform,
                    Animator = GetComponent<Animator>(),
                    CharacterController = GetComponent<CharacterController>(),
                    Agent = GetComponent<NavMeshAgent>(),
                    Path = GetComponent<Path>(),
                    PhysicsHandler = GetComponent<CharacterPhysicsHandler>(),
                    EnvironmentScanner = GetComponent<EnvironmentScanner>(),
                    FieldOfView = GetComponent<FieldOfView>()
                };

                return enemyContext;    
            }
        }

        private void Start()
        {
            SwitchState(initialState);
        }
    }
}
