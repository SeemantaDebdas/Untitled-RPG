using System;
using System.Collections;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Combat.Rework
{
    public class EnemyGroupAttackManager : MonoBehaviour
    {
        [SerializeField] ScriptableAttackerQueue enemiesInCombatQueue;
        [SerializeField] ScriptableEnemyList enemiesInAttackList;
        [SerializeField] Vector2 notInAttackTimeRange = new Vector2(1, 4);

        Timer notInAttackTimer;
        IEnumerator countdownCoroutine;

        private void Start()
        {
            ResetNotInAttackTimer();
            
            countdownCoroutine = StartNotInAttackTimer();
            StartCoroutine(countdownCoroutine);
        }

        private void OnEnable()
        {
            enemiesInAttackList.OnItemAdded += (item) =>
            {
                if(countdownCoroutine != null)
                    StopCoroutine(countdownCoroutine);
                
                ResetNotInAttackTimer();
                
                Debug.Log(item.name + "Has been removed");
            };

            enemiesInAttackList.OnItemRemoved += (item) =>
            {
                if(countdownCoroutine != null)
                    StopCoroutine(countdownCoroutine);

                countdownCoroutine = StartNotInAttackTimer();
                StartCoroutine(countdownCoroutine);
                
                Debug.Log(item.name + "Has been added");
            };
        }

        private IEnumerator StartNotInAttackTimer()
        {
            while (true)
            {
                if (enemiesInCombatQueue.Value.Count == 0)
                    yield return null;
                
                notInAttackTimer.Tick(Time.deltaTime);
                
                yield return null;
            }
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

        // private void Update()
        // {
        //     if(enemiesInAttackList == null) 
        //         return;
        //
        //     if(enemiesInAttackList.Value.Count == 0)
        //     {
        //         notInAttackTimer.Tick(Time.deltaTime);
        //     }
        //
        //     //Debug.Log("Not in attack timer: " + notInAttackTimer.RemainingSeconds);
        //
        //     if(notInAttackTimer.IsOver())
        //     {
        //         SelectEnemyForAttack();
        //     }
        // }

        void ResetNotInAttackTimer()
        {
            notInAttackTimer = new(Random.Range(notInAttackTimeRange.x, notInAttackTimeRange.y),
                SelectEnemyForAttack);   
        }
    }
}
