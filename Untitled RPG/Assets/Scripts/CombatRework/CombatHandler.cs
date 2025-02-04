using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat.Rework
{
    public class CombatHandler : MonoBehaviour
    {
        [SerializeField] private WeaponHandler weaponHandler;
        [SerializeField] private UnityEvent onAttack;

        public void PerformAttack(bool isHeavy)
        {
            // Additional logic for targeting and validating attack
            weaponHandler.Attack(isHeavy);
        }

        public bool CanCombo()
        {
            return weaponHandler.CanCombo();
        }
        
        public void Attack()
        {
            onAttack?.Invoke();
        }
    }
}
