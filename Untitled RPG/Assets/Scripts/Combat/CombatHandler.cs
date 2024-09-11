using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        public void HandleHit(IDamageable damageable)
        {
            damageable.Damage(10); 
        }
    }
}
