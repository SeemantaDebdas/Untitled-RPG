using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class Health : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float MaxHealth { get; private set; }

        [SerializeField] float invulnerabilityTime = 0f;
        float invulnerabilityTimer = int.MaxValue;

        [SerializeField] UnityEvent<DamageData> onDamage, onDeath;

        public float CurrentHealth { get; private set; }
        
        public bool IsDead => CurrentHealth <= 0;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }

        private void Update()
        {
            invulnerabilityTimer = Mathf.Min(invulnerabilityTime, invulnerabilityTimer + Time.deltaTime);
        }

        public void Damage(DamageData damageData)
        {
            if (invulnerabilityTimer < invulnerabilityTime)
                return;

            invulnerabilityTimer = 0;
            CurrentHealth -= damageData.damage;

            if (CurrentHealth <= 0)
            {
                onDeath?.Invoke(damageData);
                this.enabled = false;
                return;
            }

            onDamage?.Invoke(damageData);
        }
    }
}
