using UnityEngine;

namespace RPG
{
    public class AimRigTarget : MonoBehaviour
    {
        [SerializeField, Range (0f, 50f)] float distanceFromCamera = 50f;

        void Update()
        {
            Ray ray = UnityEngine.Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            transform.position = ray.GetPoint(distanceFromCamera);
        }
    }
}
