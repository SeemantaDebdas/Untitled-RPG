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

            //Debug.Log(other.ClosestPoint(transform.position));
            //Vector3 directionOfAttack = (other.transform.position - other.ClosestPointOnBounds(transform.position));
            Vector3 attackDirection = (other.transform.position - collider.ClosestPoint(other.transform.position));
            attackDirection.y = 0;
            attackDirection.Normalize();

            damageables.Add(damageable);


            DamageData damageData = new(combatHandler.transform, 10, attackDirection);
            combatHandler.HandleHit(damageable, damageData);

            onDamageDealt?.Invoke(other.ClosestPoint(transform.position));
        }
    }
}
