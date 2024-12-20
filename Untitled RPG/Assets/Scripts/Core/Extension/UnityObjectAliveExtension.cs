using UnityEngine;

namespace RPG.Core
{
    public static class UnityObjectAliveExtension
    {
        public static bool IsAlive(this object aObj)
        {
            var o = aObj as UnityEngine.Object;
            return o != null;
        }
    }
}
