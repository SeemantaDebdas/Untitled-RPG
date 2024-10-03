using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Combat
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] int slots = 8;
        [SerializeField] float meleeRadius = 5f;
        //[SerializeField] float rangedRadius = 10f;

        [Space]
        [SerializeField] int maxGridCapacity = 12;
        [SerializeField] int maxAttackCapacity = 10;

        int currentGridCapacity, currentAttackCapacity;

        List<Vector3> gridPositions = new();

        private void Awake()
        {
            InitializeGrid();

            currentAttackCapacity = maxAttackCapacity;
            currentGridCapacity = maxGridCapacity;
        }

        void InitializeGrid()
        {
            for (int i = 0; i < slots; i++)
            {
                float degree = i * Mathf.PI * 2 / slots;
                Vector3 gridPosition = new Vector3(Mathf.Cos(degree) * meleeRadius, 0, Mathf.Sin(degree) * meleeRadius);

                gridPositions.Add(gridPosition);
            }
        }

        public Vector3 RequestGridPosition(Transform enemy, int enemyGridWeight)
        {
            if(currentGridCapacity >= enemyGridWeight)
            {
                currentGridCapacity -= enemyGridWeight;
                return AssignGridPositionToEnemy(enemy);
            }

            return Vector3.zero;
        }

        Vector3 AssignGridPositionToEnemy(Transform enemy)
        {
            for(int i = 0; i < gridPositions.Count; i++)
            {
                if (!IsGridPositionOccupied(gridPositions[i]))
                {
                    return gridPositions[i];
                }
            }

            return Vector3.zero;
        }

        private bool IsGridPositionOccupied(Vector3 vector3)
        {
            return false;
        }

        //private void OnDrawGizmos()
        //{
        //    for (int i = 0; i < slots; i++)
        //    {
        //        float degree = i * Mathf.PI * 2 / slots;
        //        Vector3 direction = new Vector3(Mathf.Cos(degree) * meleeRadius, 0, Mathf.Sin(degree) * meleeRadius);

        //        Handles.color = Color.green;
        //        Handles.DrawLine(transform.position, transform.position + direction, 4.5f);
        //    }
        //}
    }
}
