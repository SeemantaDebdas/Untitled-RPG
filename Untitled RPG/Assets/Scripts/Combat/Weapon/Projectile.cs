using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        //[SerializeField] float raycastDistance = 0.5f;

        //void Update()
        //{
        //    if(Physics.Raycast(transform.position + transform.forward * 1f, transform.forward, out RaycastHit hit, raycastDistance))
        //    {

        //        if(hit.collider.TryGetComponent(out IDamageable damageable))
        //        {
        //            damageable.Damage(10);
        //        }

        //        Debug.Log(hit.point);

        //        Rigidbody rb = GetComponent<Rigidbody>();
        //        Destroy(rb);

        //        transform.position = hit.point;
        //        Destroy(this);
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            

            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 directionOfAttack = (other.transform.position - other.ClosestPoint(transform.position)).normalized;
                damageable.Damage(new(transform, 10, directionOfAttack));
            }
            
            Destroy(gameObject);

            //transform.position += transform.forward * 0.1f;
            //transform.SetParent(other.transform);
            
            
        }
    }
}
