using System;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Spawn Prefab At User Effect", menuName = "Debdas/Abilities/Effect/Spawn Prefab At User Effect")]
    public class SpawnPrefabAtUserEffect : EffectStrategy
    {
        [SerializeField] private GameObject prefabToSpawn = null;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, data.GetTargetedPosition(), prefabToSpawn.transform.rotation);
            onFinished?.Invoke();
        }
    }
}
