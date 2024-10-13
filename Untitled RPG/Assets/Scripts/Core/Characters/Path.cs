using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public struct WaypointData
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    public class Path : MonoBehaviour
    {
        [field: SerializeField]
        public WaypointData[] WaypointDataArray { get; set; }

        int waypointIndex = 0;

        public Vector3 GetCurrentWaypoint() => WaypointDataArray[waypointIndex].position;

        public void IncrementCurrentWaypointIndex()
        {
            waypointIndex = (waypointIndex + 1) % WaypointDataArray.Length;
        }
    }
}
