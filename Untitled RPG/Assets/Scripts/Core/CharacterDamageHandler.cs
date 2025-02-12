using System;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class CharacterDamageHandler : MonoBehaviour
    {
        [SerializeField] private float stunThreshold = 50;
        private Health health;

        public event Action<DamageData> OnStunned, OnHurt; 
        
        AutoTimer damageTimer = new AutoTimer();

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnDamage += Health_OnDamage;
        }

        private void OnDisable()
        {
            health.OnDamage -= Health_OnDamage;
        }
        
        
        int continuousDamageTaken = 0;
        private void Health_OnDamage(DamageData damageData)
        {
            continuousDamageTaken += damageData.Damage;
            
            damageTimer.SetTimeAndStartTimer(2f, () => continuousDamageTaken = 0);

            if (continuousDamageTaken >= stunThreshold)
            {
                OnStunned?.Invoke(damageData);
                continuousDamageTaken = 0;
                damageTimer.StopTimer();
            }
            else
            {
                OnHurt?.Invoke(damageData);
            }
        }   
    }
}
