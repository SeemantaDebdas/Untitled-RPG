using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability
{
    public class AbilityData
    {
        private GameObject user;
        private IEnumerable<GameObject> targets;
        private Vector3 targetedPosition;

        public AbilityData(GameObject user)
        {
            SetUser(user);
        }
        
        public GameObject GetUser() => user;
        public GameObject SetUser(GameObject user) => this.user = user;
        
        public IEnumerable<GameObject> GetTargets() => targets;
        public void SetTargets(IEnumerable<GameObject> targets) => this.targets = targets;
        
        public Vector3 GetTargetedPosition() => targetedPosition;
        public void SetTargetedPosition(Vector3 targetedPosition) => this.targetedPosition = targetedPosition;
    }
}
