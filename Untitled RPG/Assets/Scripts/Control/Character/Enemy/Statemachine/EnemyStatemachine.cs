using RPG.Combat;
using RPG.Core;
using RPG.Data;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyStatemachine : Statemachine
    {
        [SerializeField] FieldOfView chaseFOV;
        [SerializeField] FieldOfView attackFOV;

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
                    FieldOfView = GetComponent<FieldOfView>(),
                    WeaponHandler = GetComponent<WeaponHandler>(),
                    ChaseFOV = chaseFOV,
                    AttackFOV = attackFOV,
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
