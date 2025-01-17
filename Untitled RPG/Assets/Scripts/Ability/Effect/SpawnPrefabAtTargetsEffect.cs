using System;
using UnityEngine;

namespace RPG.Ability.Effect
{
    [CreateAssetMenu(fileName = "New Spawn Prefab At Targets Effect", menuName = "Debdas/Abilities/Effect/Spawn Prefab At Targets Effect")]
    public class SpawnPrefabAtTargetsEffect : EffectStrategy
    {
        [SerializeField] private GameObject prefabToSpawn = null;
        public override void StartEffect(AbilityData data, Action onFinished = null)
        {
            foreach (GameObject target in data.GetTargets())
            {
                Instantiate(prefabToSpawn, target.transform.position, target.transform.rotation);
            }
            
            onFinished?.Invoke();
        }
    }
}
