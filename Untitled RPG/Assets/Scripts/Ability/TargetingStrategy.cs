using UnityEngine;

namespace RPG.Ability
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void StartTargeting();
    }
}
