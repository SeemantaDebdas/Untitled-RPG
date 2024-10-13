using UnityEngine;
using RPG.Data;

namespace RPG.Control
{
    public abstract class ConditionSO : ScriptableObject
    {
        [SerializeField] protected bool invert = false;
        public abstract void Initialize(Context context);
        public virtual bool Evaluate(Context context)
        {
            bool result = ProcessCondition(context);
            return invert ? !result : result;
        }

        protected abstract bool ProcessCondition(Context context);
        public abstract void Reset();
    }
}