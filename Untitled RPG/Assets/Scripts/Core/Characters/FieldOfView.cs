using MEC;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Core
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] bool startOnAwake = true;
        
        [field:Space]
        [field: SerializeField] public float Radius { get; private set; } = 5f;
        [SerializeField] float defaultAngle = 90f;
        [SerializeField] int scanCapacity = 20;
        [SerializeField] float delayBetweenScans = 0.2f;

        [Header("FILTERING")]
        [SerializeField] LayerMask targetLayer;
        [SerializeField] LayerMask obstructionLayer;
        [SerializeField] ScriptableString ignoreTag;
        [SerializeField] Transform selfTransform;

        [Header("GIZMO")]
        [SerializeField] Color gizmoColor = Color.yellow;

        Collider[] scanArray;

        List<Transform> validTargets = new();

        private float angle;

        public event Action OnValidTargetsModified;

        private void Awake()
        {
            angle = defaultAngle; 
            
            if(startOnAwake)
                Timing.RunCoroutine(ScanRoutine());
        }

        IEnumerator<float> ScanRoutine()
        {
            while (true)
            {
                Scan();
                yield return Timing.WaitForSeconds(delayBetweenScans);
            }
        }

        public void SetMaxAngle(float angle) => this.angle = 360f;
        public void ResetAngle() => angle = defaultAngle;
        public void SetAngle(float angle) => this.angle = angle;

        public Transform GetClosestTarget()
        {
            if (validTargets == null || validTargets.Count == 0)
            {
                //Debug.LogWarning(transform.name + "Closest target is null");
                return null;
            }

            return validTargets[0].transform;
        }

        public List<Transform> GetValidTargets()
        {
            return validTargets;
        }

        void Scan()
        {
            scanArray = new Collider[scanCapacity];
            validTargets.Clear();

            int scanCount = Physics.OverlapSphereNonAlloc(transform.position, Radius, scanArray, targetLayer, QueryTriggerInteraction.Ignore);

            if (scanCount == 0)
                return;

            for (int i = 0; i < scanCount; i++)
            {
                Transform target = scanArray[i].transform;

                if (target == transform)
                    continue;

                if (selfTransform != null && target == selfTransform)
                    continue;

                Vector3 directionToTarget = target.position - transform.position;

                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);

                if (angleToTarget > angle / 2)
                    continue;

                if (ignoreTag != null && target.CompareTag(ignoreTag.Value))
                    continue;

                validTargets.Add(target);

                OnValidTargetsModified?.Invoke();
                //Debug.Log(target.name + ": Angle Satisfied");
            }

            if (validTargets.Count == 0)
                OnValidTargetsModified?.Invoke();
        }

        void OnDrawGizmosSelected()
        {
            Handles.DrawWireDisc(transform.position, Vector3.up, Radius);

            Handles.color = gizmoColor;

            Vector3 fromVector = Quaternion.Euler(0, -defaultAngle / 2, 0) * transform.forward;

            Handles.DrawSolidArc(transform.position, Vector3.up, fromVector, defaultAngle, Radius);
        }
    }
}
