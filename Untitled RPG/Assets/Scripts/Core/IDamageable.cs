using UnityEngine;

namespace RPG.Core
{
    public interface IDamageable 
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        void Damage(int damage);

    }
}
