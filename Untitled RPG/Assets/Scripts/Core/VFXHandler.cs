using UnityEngine;

namespace RPG.Core
{
    public class VFXHandler : MonoBehaviour
    {
        [SerializeField] GameObject defaultVFXPrefab = null;

        public void PlayVFX(float timeTillDestroy)
        {
            GameObject vfxPrefabSpawn = Instantiate(defaultVFXPrefab, transform);
            Destroy(vfxPrefabSpawn, timeTillDestroy);
        }


        public void PlayVFX(GameObject vfxPrefab, float timeTillDestroy)
        {
            GameObject vfxPrefabSpawn = Instantiate(vfxPrefab, transform);
            Destroy(vfxPrefabSpawn, timeTillDestroy);
        }

        public void PlayVFXAtPosition(Vector3 position)
        {
            PlayVFXAtPosition(position, 1);
        }

        public void PlayVFXAtPosition(Vector3 position, float timeTillDestroy = 1f)
        {
            GameObject vfxPrefabSpawn = Instantiate(defaultVFXPrefab, position, defaultVFXPrefab.transform.rotation);
            Destroy(vfxPrefabSpawn, timeTillDestroy);
        }

        public void PlayVFXAtPosition(GameObject vfxPrefab, Vector3 position, float timeTillDestroy = 1f)
        {
            //GameObject vfxPrefabSpawn = Instantiate(vfxPrefab, position, vfxPrefab.transform.rotation);
            //Destroy(vfxPrefabSpawn, timeTillDestroy);
        }
    }
}
