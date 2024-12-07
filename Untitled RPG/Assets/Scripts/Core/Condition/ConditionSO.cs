using UnityEngine;
using RPG.Data;

namespace RPG.Core
{
    public abstract class ConditionSO : ScriptableObject
    {
        [SerializeField] protected bool invert = false;
        public abstract void Initialize(Context context);
        public virtual bool Evaluate(Context context)
        {
            bool result = ProcessCondition(context);

            if (!result && invert)
            {
                Debug.Log(name + "Condition is true");
            }
            
            return invert ? !result : result;
        }

        protected abstract bool ProcessCondition(Context context);
        public abstract void Reset();
    }
}