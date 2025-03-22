using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class CharacterDamageHandler : MonoBehaviour
    {
        [SerializeField] private float stunThreshold = 50;

        [Header("Polish")]
        [SerializeField] private List<SkinnedMeshRenderer> targetRenderers;
        [SerializeField] private float blinkDuration = 0.1f;
        [SerializeField] Material blinkMaterial;
        
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
            
            BlinkRenderers();
        }
        
        void BlinkRenderers()
        {
            foreach (SkinnedMeshRenderer targetRenderer in targetRenderers)
            {
                Material originalMaterial = targetRenderer.material;
                targetRenderer.material = blinkMaterial;

                DOVirtual.Float(0, 1, blinkDuration, value => { }).OnComplete(() =>
                {
                    targetRenderer.material = originalMaterial;
                });
            }    
        }
        
        [ContextMenu("Setup Renderers")]
        void SetupRenderers()
        {
            targetRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        }
    }
}
