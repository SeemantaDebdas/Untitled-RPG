using RPG.Core;
using System.Collections;
using System.Collections.Generic;
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
            Destroy(rb);

            if (other.TryGetComponent(out IDamageable damageable))
            {
                Vector3 directionOfAttack = (other.transform.position - other.ClosestPoint(transform.position)).normalized;
                damageable.Damage(new(10, directionOfAttack));
            }

            transform.position += transform.forward * 0.25f;
        }
    }
}
