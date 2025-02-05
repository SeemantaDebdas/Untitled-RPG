using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat.Rework
{
    public class EnemyGroupAttackManager : MonoBehaviour
    {
        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;
        [SerializeField] ScriptableEnemyList enemiesInAttackList;
        [SerializeField] Vector2 notInAttackTimeRange = new Vector2(1, 4);

        Timer notInAttackTimer;

        private void Start()
        {
            ResetNotInAttackTimer();
        }

        void SelectEnemyForAttack()
        {
            //choose the enemy that has been the longest in enemiesInCombatList
            if (enemiesInCombatQueue.Value.Count == 0)
                return;

            CombatHandler enemy = enemiesInCombatQueue.FirstElement;

            Debug.Assert(enemy != null, "Enemy is null");
            Debug.Log(enemy.name);

            enemy.Attack();

            ResetNotInAttackTimer();
        }

        private void Update()
        {
            if(enemiesInAttackList == null) 
                return;

            if(enemiesInAttackList.Value.Count == 0)
            {
                notInAttackTimer.Tick(Time.deltaTime);
            }

            //Debug.Log("Not in attack timer: " + notInAttackTimer.RemainingSeconds);

            if(notInAttackTimer.IsOver())
            {
                SelectEnemyForAttack();
            }
        }

        void ResetNotInAttackTimer() => notInAttackTimer = new(Random.Range(notInAttackTimeRange.x, notInAttackTimeRange.y), () => { });
    }
}
