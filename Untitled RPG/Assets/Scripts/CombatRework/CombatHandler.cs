using UnityEngine;

namespace RPG.Combat.Rework
{
    public class CombatHandler : MonoBehaviour
    {
        [SerializeField] private WeaponHandler weaponHandler;

        public void PerformAttack(bool isHeavy)
        {
            // Additional logic for targeting and validating attack
            weaponHandler.Attack(isHeavy);
        }

        public bool CanCombo()
        {
            return weaponHandler.CanCombo();
        }
    }
}
