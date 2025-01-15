using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Ability.Filter
{
    [CreateAssetMenu(fileName = "New Tag Filter", menuName = "Debdas/Abilities/Filtering/Tag Filter")]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] ScriptableString filterName;
        
        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (GameObject gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(filterName.Value))
                    yield return gameObject;
            }
        }
    }
}
