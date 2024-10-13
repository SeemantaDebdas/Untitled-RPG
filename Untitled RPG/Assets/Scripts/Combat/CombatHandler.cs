using UnityEngine;
using RPG.Core;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        public UnityEvent onHit;
        public UnityEvent onAttack;

        //have a list of attacks and calculate stats of player
        //then handle the hit
        public void HandleHit(IDamageable damageable, Vector3 attackDirection)
        {
            damageable.Damage(new DamageData(10, attackDirection));
            onHit?.Invoke();
        }

        public void Attack()
        {
            onAttack?.Invoke();
        }
    }
}
