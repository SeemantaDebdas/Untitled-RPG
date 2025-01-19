using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability
{
    public class AbilityData
    {
        private GameObject user;
        private IEnumerable<GameObject> targets;
        private float abilityDuration;

        public AbilityData(GameObject user, float abilityDuration)
        {
            SetUser(user);
            SetAbilityDuration(abilityDuration);
        }
        
        public GameObject GetUser() => user;
        public GameObject SetUser(GameObject user) => this.user = user;
        
        public IEnumerable<GameObject> GetTargets() => targets;
        public void SetTargets(IEnumerable<GameObject> targets) => this.targets = targets;
        
        public float GetAbilityDuration() => abilityDuration;
        public void SetAbilityDuration(float abilityDuration) => this.abilityDuration = abilityDuration;
    }
}
