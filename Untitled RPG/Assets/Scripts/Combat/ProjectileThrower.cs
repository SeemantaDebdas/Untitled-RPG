using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Combat
{
    public class ProjectileThrower : MonoBehaviour
    {
        float force;
        
        public void SetForce(float force)
        {
            this.force = force;
        }

        public void ResetForce() => force = 0.0f;

        public void ThrowObject(Rigidbody projectilePrefab, Vector3 position, Vector3 direction)
        {
            Rigidbody thrownObject = Instantiate(projectilePrefab, position, Quaternion.identity);
            thrownObject.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
