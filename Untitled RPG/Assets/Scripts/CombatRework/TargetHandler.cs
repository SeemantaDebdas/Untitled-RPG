using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class TargetHandler : MonoBehaviour
    {
        private EnemyGridManager enemyGridManager;
        private InputReader inputReader;

        [SerializeField] private Transform debugSphere = null;
        [SerializeField] private ScriptableEnemyList attackingEnemiesList = null;
        [SerializeField] private float targetDetectionRadius = 5f;
        [SerializeField] private LayerMask detectionLayer;
        
        Collider[] detectionResultArray = new Collider[20];

        public CombatHandler Target { get; private set; }

        private void Awake()
        {
            enemyGridManager = GetComponent<EnemyGridManager>();
            inputReader = GetComponent<InputReader>();
        }

        private void Update()
        {
            Target = GetEnemyInInputDirection();
        }

        public CombatHandler GetEnemyInInputDirection()
        {
            Vector3 targetDirection = transform.forward;
            
            if (inputReader.MoveInput3 != Vector3.zero)
            {
                Vector3 globalInputDirection = UnityEngine.Camera.main.transform.TransformDirection(inputReader.MoveInput3);
                globalInputDirection.y = 0;
                globalInputDirection.Normalize();
                
                targetDirection = globalInputDirection;
            }
            
            int hits = Physics.OverlapSphereNonAlloc(transform.position, targetDetectionRadius, detectionResultArray, detectionLayer);
            
            List<CombatHandler> enemyList = new List<CombatHandler>(); 
            for (int i = 0; i < hits; i++)
            {
                Transform detectedElement = detectionResultArray[i].transform;
                if (!detectedElement.TryGetComponent(out CombatHandler combatHandler))
                    continue;

                if (detectedElement.transform == transform)
                    continue;
                
                enemyList.Add(combatHandler);
            }
            
            float smallestAngle = 360f;
            CombatHandler nearestEnemy = null;
            foreach (CombatHandler enemy in enemyList)
            {
                Vector3 dirToEnemy = enemy.transform.position - transform.position;
                dirToEnemy.y = 0;
                dirToEnemy.Normalize();
                
                float angle = Vector3.Angle(dirToEnemy, targetDirection);

                if (angle < smallestAngle)
                {
                    smallestAngle = angle;
                    nearestEnemy = enemy;
                }
            }

            // if (nearestEnemy != null)
            // {
            //     debugSphere.position = nearestEnemy.position;
            // }
            //
            // Debug.DrawRay(transform.position, globalInputDirection, Color.red);

            return nearestEnemy;
        }
    }
}
