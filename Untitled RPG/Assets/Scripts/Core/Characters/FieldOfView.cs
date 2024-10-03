using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] bool startOnAwake = true;
        
        [Space]
        [SerializeField] float radius = 5f;
        [SerializeField] float angle = 90f;
        [SerializeField] int scanCapacity = 20;
        [SerializeField] float delayBetweenScans = 0.2f;

        [Header("FILTERING")]
        [SerializeField] LayerMask targetLayer;
        [SerializeField] LayerMask obstructionLayer;
        [SerializeField] string ignoreTag = string.Empty; //change this later to a scriptable object based solution
        [SerializeField] Transform selfTransform;

        [Header("GIZMO")]
        [SerializeField] Color gizmoColor = Color.yellow;

        Collider[] scanArray;
        List<Transform> validTargets;


        private void Start()
        {
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

        public Transform GetClosestTarget()
        {
            Scan();
            if (validTargets == null || validTargets.Count == 0)
                return null;

            return validTargets[0].transform;
        }

        public List<Transform> GetValidTargets()
        {
            Scan();
            return validTargets;
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
                    continue;

                if (selfTransform != null && target == selfTransform)
                    continue;

                Vector3 directionToTarget = target.position - transform.position;

                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);

                if (angleToTarget > angle / 2)
                    continue;

                if (ignoreTag != string.Empty && target.CompareTag(ignoreTag))
                    continue;

                validTargets.Add(target);

                Debug.Log(target);
                //Debug.Log(target.name + ": Angle Satisfied");
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
