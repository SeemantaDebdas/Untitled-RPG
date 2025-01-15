using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability.Targeting
{
    [CreateAssetMenu(fileName = "New Area Targeting", menuName = "Debdas/Abilities/Targeting/Area Targeting")]
    public class AreaTargeting : TargetingStrategy
    {
        [SerializeField] private float areaRadius = 2f;
        [SerializeField] private LayerMask layers;
        public override void StartTargeting(AbilityData data, Action onTargetingFinished)
        {
            IEnumerable<GameObject> objectsInArea = GetObjectsInArea(data.GetUser());
            data.SetTargets(objectsInArea);
            data.SetTargetedPosition(data.GetUser().transform.position);
            onTargetingFinished?.Invoke();
        }

        IEnumerable<GameObject> GetObjectsInArea(GameObject user)
        {
            Collider[] results = new Collider[20];
            var size = Physics.OverlapSphereNonAlloc(user.transform.position, areaRadius, results, layers, QueryTriggerInteraction.Ignore);

            foreach (Collider result in results)
            {
                if(result != null)
                    yield return result.gameObject;
            }
        }
    }
}
