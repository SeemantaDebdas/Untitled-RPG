using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Combat
{
    public class GridManager : MonoBehaviour
    {
        public class GridSlot
        {
            public Vector3 Position { get; private set; }
            public bool IsOccupied { get; private set; }

            public GridSlot(Vector3 position, bool isOccupied)
            {
                Position = position;
                IsOccupied = isOccupied;
            }

            public void Occupy()
            {
                IsOccupied = true;
            }
            
            public void Unoccupy()
            {
                IsOccupied = false;
            }

            public void SetPosition(Vector3 position)
            {
                Position = position;
            }
        }
        
        [SerializeField] int slots = 8;
        [SerializeField] float meleeRadius = 5f;
        
        [Header("Noise Settings")]
        [SerializeField] Vector2 positionRegenTimeRange = Vector2.zero;
        [SerializeField] private float angleNoiseStrength = 5f;
        [SerializeField] private float positionNoiseStrength = 0.5f;
        //[SerializeField] float rangedRadius = 10f;

        [Space]
        [SerializeField] int maxGridCapacity = 12;
        [SerializeField] int maxAttackCapacity = 10;

        [Header("Debug Settings")] 
        [SerializeField] private bool debug = false;
        [SerializeField] private Color gridLineColor = Color.white;
        [SerializeField] private Color occupiedSphereColor = Color.white;
        [SerializeField] private Color unoccupiedSphereColor = Color.white;
        [SerializeField] private float sphereRadius = 0.5f;

        int currentGridCapacity, currentAttackCapacity;

        List<GridSlot> gridPositions = new();
        private Dictionary<Transform, GridSlot> gridSlotStatusDict = new();

        private void Awake()
        {
            InitializeGrid();

            currentAttackCapacity = maxAttackCapacity;
            currentGridCapacity = maxGridCapacity;
        }

        private void Start()
        {
            StartCoroutine(MoveRandomGridPosition());
        }

        void InitializeGrid()
        {
            for (int i = 0; i < slots; i++)
            {
                float degree = i * Mathf.PI * 2 / slots;
        
                // Add noise to the angle
                float angleNoise = Random.Range(-angleNoiseStrength, angleNoiseStrength) * Mathf.Deg2Rad;
                degree += angleNoise;
        
                // Calculate base position
                Vector3 gridPosition = new Vector3(
                    Mathf.Cos(degree) * meleeRadius,
                    0,
                    Mathf.Sin(degree) * meleeRadius
                );
                
                gridPosition.x += Random.Range(-positionNoiseStrength, positionNoiseStrength);
                gridPosition.z += Random.Range(-positionNoiseStrength, positionNoiseStrength);

                gridPositions.Add(new GridSlot(gridPosition, false));
            }
        }


        public GridSlot RequestGridPosition(Transform enemy, int enemyGridWeight)
        {
            if (currentGridCapacity < enemyGridWeight)
            {
                Debug.LogWarning("Grid Capacity Reached");
                return null;
            }

            GridSlot closestSlot = GetClosestGridSlotToEnemy(enemy);
            if (closestSlot == null)
            {
                Debug.LogWarning("All Grid Slots occupied");
                return null;
            }
            
            closestSlot.Occupy();
            currentGridCapacity -= enemyGridWeight;
            gridSlotStatusDict.Add(enemy, closestSlot);
            
            return closestSlot;
        }

        GridSlot GetClosestGridSlotToEnemy(Transform enemy)
        {
            GridSlot closestSlot = null;
            float closestDistance = Mathf.Infinity;

            foreach (var slot in gridPositions)
            {
                if (!slot.IsOccupied) // Only consider unoccupied slots
                {
                    float distance = Vector3.Distance(enemy.position, transform.position + slot.Position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestSlot = slot;
                    }
                }
            }

            return closestSlot;
        }

        public void UnoccupyGridPositionForEnemy(Transform enemy, int gridWeight)
        {
            if (!gridSlotStatusDict.Remove(enemy, out GridSlot gridSlot)) 
                return;

            gridSlot.Unoccupy();
            currentGridCapacity += gridWeight;
        }

        Vector3 AssignGridPositionToEnemy(Transform enemy)
        {
            for(int i = 0; i < gridPositions.Count; i++)
            {
                if (!gridPositions[i].IsOccupied)
                {
                    gridPositions[i].Occupy();
                    return gridPositions[i].Position;
                }
            }

            return Vector3.zero;
        }

        private bool IsGridPositionOccupied(Vector3 vector3)
        {
            return false;
        }

        IEnumerator MoveRandomGridPosition()
        {
            while (true)
            {
                int randomIndex = Random.Range(0, gridPositions.Count);
                
                float degree = randomIndex * Mathf.PI * 2 / slots;
        
                // Add noise to the angle
                float angleNoise = Random.Range(-angleNoiseStrength, angleNoiseStrength) * Mathf.Deg2Rad;
                degree += angleNoise;
        
                // Calculate base position
                Vector3 newPosition = new Vector3(
                    Mathf.Cos(degree) * meleeRadius,
                    0,
                    Mathf.Sin(degree) * meleeRadius
                );
                
                newPosition.x += Random.Range(-positionNoiseStrength, positionNoiseStrength);
                newPosition.z += Random.Range(-positionNoiseStrength, positionNoiseStrength);
                
                gridPositions[randomIndex].SetPosition(newPosition);
                
                yield return new WaitForSeconds(Random.Range(positionRegenTimeRange.x, positionRegenTimeRange.y));
            }
        }

        private void OnDrawGizmos()
        {
            if (!debug) return;

            // Ensure the grid is initialized
            if (gridPositions == null || gridPositions.Count == 0)
            {
                InitializeGrid(); // Initialize if it's not already
            }

            for (int i = 0; i < slots; i++)
            {
                Handles.color = gridLineColor;
                Handles.DrawLine(transform.position, transform.position + gridPositions[i].Position, 4.5f);

                // Ensure we're in Play Mode before accessing runtime properties
                bool isOccupied = Application.isPlaying && i < gridPositions.Count && gridPositions[i].IsOccupied;

                Gizmos.color = isOccupied ? occupiedSphereColor : unoccupiedSphereColor;
                Gizmos.DrawSphere(transform.position + gridPositions[i].Position, sphereRadius);
            }
        }

    }
}
