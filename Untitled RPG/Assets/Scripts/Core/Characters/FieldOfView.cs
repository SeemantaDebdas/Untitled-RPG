using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] float radius = 5f;
        [SerializeField] float angle = 90f;
        [SerializeField] int scanCapacity = 20;
        [SerializeField] float delayBetweenScans = 0.2f;
        [SerializeField] LayerMask targetLayer;
        [SerializeField] LayerMask obstructionLayer;

        [Header("GIZMO")]
        [SerializeField] Color gizmoColor = Color.yellow;

        Collider[] scanArray;
        List<Transform> validTargets;

        private void Start()
        {
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

        public Transform GetClosestTarget()
        {
            Scan();
            if (validTargets == null || validTargets.Count == 0)
                return null;

            return validTargets[0].transform;
        }

        void Scan()
        {
            scanArray = new Collider[scanCapacity];
            validTargets = new();

            int scanCount = Physics.OverlapSphereNonAlloc(transform.position, radius, scanArray, targetLayer, QueryTriggerInteraction.Ignore);

            if (scanCount == 0)
                return;

            for (int i = 0; i < scanCount; i++)
            {
                Transform target = scanArray[i].transform;

                if (target == transform)
                    return;

                Vector3 directionToTarget = target.position - transform.position;

                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);

                if (angleToTarget > angle / 2)
                    return;

                validTargets.Add(target);
                Debug.Log(target.name + ": Angle Satisfied");
            }
        }

        void OnDrawGizmosSelected()
        {
            Handles.DrawWireDisc(transform.position, Vector3.up, radius);

            Handles.color = gizmoColor;

            Vector3 fromVector = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;

            Handles.DrawSolidArc(transform.position, Vector3.up, fromVector, angle, radius);
        }
    }
}
