using RPG.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    [Serializable]
    public class Transition
    {
        [field: SerializeField] public List<ConditionSO> ConditionList { get; private set; }
        [field: SerializeReference] public State ToState { get; private set; }

        bool isConditionInitialized = false;

        public bool EvaluateConditions(Context context)
        {
            if (!isConditionInitialized)
            {
                isConditionInitialized = true;
                InitializeConditions(context);
            }

            foreach (ConditionSO condition in ConditionList)
            {
                if (!condition.Evaluate(context))
                    return false;
            }

            return true;
        }

        public void Reset()
        {
            isConditionInitialized = false;

            foreach (ConditionSO condition in ConditionList)
            {
                condition.Reset();
            }
        }

        void InitializeConditions(Context context)
        {
            foreach (ConditionSO condition in ConditionList)
            {
                condition.Initialize(context);
            }
        }
    }
}
