using System;
using DG.Tweening;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Spawn Prefab At User Effect", menuName = "Debdas/Abilities/Effect/Spawn Prefab At User Effect")]
    public class SpawnPrefabAtUserEffect : EffectStrategy
    {
        [SerializeField] private GameObject prefabToSpawn = null;
        [SerializeField] float destroyAfterSeconds = 1f;
        
        float timeToDestroy = 0f;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, data.GetUser().transform);
            
            timeToDestroy = destroyAfterSeconds;

            if (Mathf.Approximately(timeToDestroy, -1))
            {
                timeToDestroy = data.GetAbilityDuration();
            }
            
            DOVirtual.Float(0, 1, timeToDestroy, _ => { }).OnComplete(() =>
            {
                Destroy(spawnedPrefab);
            });
            
            onFinished?.Invoke();
        }
    }
}
