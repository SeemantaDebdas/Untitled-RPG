using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class TargetHandler : MonoBehaviour
    {
        private EnemyGridManager enemyGridManager;
        private InputReader inputReader;

        [SerializeField] private Transform debugSphere = null;

        public Transform Target { get; private set; }

        private void Awake()
        {
            enemyGridManager = GetComponent<EnemyGridManager>();
            inputReader = GetComponent<InputReader>();
        }

        private void Update()
        {
            Target = GetEnemyInInputDirection();
        }

        public Transform GetEnemyInInputDirection()
        {
            if (inputReader.MoveInput3 == Vector3.zero)
            {
                return null;
            }
            
            Vector3 globalInputDirection = UnityEngine.Camera.main.transform.TransformDirection(inputReader.MoveInput3);
            globalInputDirection.y = 0;
            globalInputDirection.Normalize();

            float smallestAngle = 360f;
            Transform nearestEnemy = null;
            foreach (Transform enemy in enemyGridManager.GetEnemiesInGrid())
            {
                Vector3 dirToEnemy = enemy.position - transform.position;
                dirToEnemy.y = 0;
                dirToEnemy.Normalize();
                
                float angle = Vector3.Angle(dirToEnemy, globalInputDirection);

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
