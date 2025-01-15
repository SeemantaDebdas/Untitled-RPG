using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(AbilityData data, Action onFinished = null);
    }
}
