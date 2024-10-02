using RPG.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    [System.Serializable]
    public class UnityEvent_Vector3 : UnityEvent<Vector3> { }

    public class WeaponCollider : MonoBehaviour
    {
        new Collider collider = null;
        CombatHandler combatHandler = null;

        List<IDamageable> damageables = new List<IDamageable>();

        [SerializeField] UnityEvent_Vector3 onDamageDealt;

        private void Awake()
        {
            collider = GetComponentInChildren<Collider>();
        }

        public void EnableCollider() => collider.enabled = true;

        public void DisableCollider() => collider.enabled = false;

        public void ResetColliderData()
        {
            damageables.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (combatHandler == null)
            {
                combatHandler = GetComponentInParent<CombatHandler>();
            }

            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            if (damageables.Contains(damageable))
            {
                return;
            }

            Debug.Log(other.ClosestPoint(transform.position));
            Vector3 directionOfAttack = (other.transform.position - other.ClosestPoint(transform.position)).normalized;

            damageables.Add(damageable);

            combatHandler.HandleHit(damageable, directionOfAttack);

            onDamageDealt?.Invoke(other.ClosestPoint(transform.position));
        }
    }
}
