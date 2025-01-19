using System;
using DG.Tweening;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Spawn Prefab At Targets Effect", menuName = "Debdas/Abilities/Effect/Spawn Prefab At Targets Effect")]
    public class SpawnPrefabAtTargetsEffect : EffectStrategy
    {
        [SerializeField] private GameObject prefabToSpawn = null;
        [SerializeField] float destroyAfterSeconds = -1f;

        private float timeToDestroy;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            timeToDestroy = destroyAfterSeconds;
            
            
            foreach (GameObject target in data.GetTargets())
            {
                GameObject spawnedPrefab = Instantiate(prefabToSpawn, target.transform.position, target.transform.rotation);
                
                timeToDestroy = destroyAfterSeconds;

                if (Mathf.Approximately(timeToDestroy, -1))
                {
                    timeToDestroy = data.GetAbilityDuration();
                }
            
                DOVirtual.Float(0, 1, timeToDestroy, _ => { }).OnComplete(() =>
                {
                    Destroy(spawnedPrefab);
                });
            }
            
            onFinished?.Invoke();
        }
    }
}
