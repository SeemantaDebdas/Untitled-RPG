using UnityEngine;

namespace RPG.Core
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] bool ignoreYRotation = false;
        private void Update()
        {
            Vector3 dirToCamera = (Camera.main.transform.position - transform.position).normalized;
            
            if (ignoreYRotation)
            {
                dirToCamera.y = 0;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(dirToCamera);

            transform.rotation = targetRotation;
        }
    }
}
