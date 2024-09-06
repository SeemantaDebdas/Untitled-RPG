using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class EnvironmentScanner : MonoBehaviour
    {
        [field: SerializeField] public float UpOffsetFromPlayerBase { get; private set; } = 0.5f;
        [field: SerializeField] public float FrontRayDistance { get; private set; } = 1f;
        [field: SerializeField] public float MaxHeightForDownRaycast { get; private set; } = 5f;
        [field: SerializeField] public LayerMask EnvironmentLayer { get; private set; }
        
        public bool IsObjectInfront()
        {
            return GetObjectInfront(transform.position) != null;
        }
        public bool IsObjectBelow(Vector3 origin)
        {
            return GetObjectBelow(origin) != null;
        }

        public RaycastHit? GetObjectInfront(Vector3 origin)
        {
            if(Physics.Raycast(origin + transform.up * UpOffsetFromPlayerBase,
                               transform.forward,
                               out RaycastHit hit,
                               FrontRayDistance,
                               EnvironmentLayer))
            { return hit; }
            
            return null;
        }

        public RaycastHit? GetObjectBelow(Vector3 origin)
        {
            if (Physics.Raycast(origin + Vector3.up * MaxHeightForDownRaycast,
                                Vector3.down,
                                out RaycastHit hit,
                                MaxHeightForDownRaycast + 0.1f,
                                EnvironmentLayer))
            { return hit; }

            return null;
        }
    }
}
