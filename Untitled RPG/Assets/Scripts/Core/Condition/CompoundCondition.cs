using System.Collections.Generic;
using RPG.Data;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class CompoundCondition<T> where T : ConditionSO
    {
        [SerializeField] private List<Disjunction> and = new();

        public bool Evaluate(Context context)
        {
            foreach (Disjunction disjunction in and)
            {
                if (!disjunction.Evaluate(context))
                    return false;
            }

            return true;
        }
        
        [System.Serializable]
        class Disjunction
        {
            [SerializeField] private List<T> or = new();

            public bool Evaluate(Context context)
            {
                foreach (T conditions in or)
                {
                    if (conditions.Evaluate(context))
                        return true;
                }

                return false;
            }
        }
    }
}