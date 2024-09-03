using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class EnvironmentScanner : MonoBehaviour
    {
        [SerializeField] float upOffsetFromPlayerBase = 0.5f;
        [SerializeField] float frontRayDistance = 1f;
        [SerializeField] float maxHeightForDownRaycast = 5f;
        [SerializeField] LayerMask environmentLayer;
        
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
            if(Physics.Raycast(origin + transform.up * upOffsetFromPlayerBase,
                               transform.forward,
                               out RaycastHit hit,
                               frontRayDistance,
                               environmentLayer))
            { return hit; }
            
            return null;
        }

        public RaycastHit? GetObjectBelow(Vector3 origin)
        {
            if (Physics.Raycast(origin + Vector3.up * maxHeightForDownRaycast,
                                Vector3.down,
                                out RaycastHit hit,
                                maxHeightForDownRaycast + 0.1f,
                                environmentLayer))
            { return hit; }

            return null;
        }
    }
}
