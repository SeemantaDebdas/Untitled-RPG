using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class CombatHandler : MonoBehaviour
    {
        //have a list of attacks and calculate stats of player
        //then handle the hit
        public void HandleHit(IDamageable damageable)
        {
            damageable.Damage(10); 
        }
    }
}
