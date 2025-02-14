using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ability
{
    public abstract class FilterStrategy : ScriptableObject
    {
        public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject> objects);
    }
}
