using System;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Spawn Target Prefab Effect", menuName = "Debdas/Abilities/Effect/Spawn Target Prefab Effect")]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] private GameObject prefabToSpawn = null;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
           //GameObject spawnedPrefab = Instantiate(prefabToSpawn, data.GetTargetedPosition(), prefabToSpawn.transform.rotation);
            onFinished?.Invoke();
        }
    }
}
