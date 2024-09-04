using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsTimerOver", menuName = "Condition/Is Timer Over", order = 1)]
    public class IsTimerOver : ConditionSO
    {
        [SerializeField] float time = 1f;

        Timer timer;

        public override void Initialize(Context context)
        {
            timer = new(time, () => { });
        }

        public override void Reset()
        {

        }

        protected override bool ProcessCondition(Context context)
        {
            return timer.IsOver();
        }
    }
}

