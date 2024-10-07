using RPG.Core;
using RPG.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName = "IsTimerOver", menuName = "Condition/Is Timer Over", order = 2)]
    public class IsTimerOver : ConditionSO
    {
        [SerializeField] private float time = 1f;  // Original time value
        [SerializeField] private bool randomTime = false;  // Randomize the timer?
        [SerializeField] private Vector2 randomTimeRange;  // Range for random time

        // Class to store instance-specific timer data
        class IsTimerOverInstance
        {
            public Timer TimerInstance;  // Each instance has its own Timer
        }

        // Initialize is called when the condition is first used in the context of an object
        public override void Initialize(Context context)
        {
            // Try to get the instance-specific data from the context
            if (!context.TryGetData(out IsTimerOverInstance instance))
            {
                // If not found, create a new instance and store it in the context
                instance = new IsTimerOverInstance();
                context.SetData(instance);
            }

            // Set up the timer for this instance
            float currentTime = time;
            if (randomTime)
            {
                currentTime = Random.Range(randomTimeRange.x, randomTimeRange.y);  // Generate random time in range
            }

            // Initialize the timer for this specific instance
            instance.TimerInstance = new Timer(currentTime, () => { /* Add any callback if needed */ });
        }

        // Reset method to reset the timer if needed
        public override void Reset()
        {
            // Reset behavior can be defined based on use case
            // In this case, nothing is reset since the timer is stored in the context
        }

        // ProcessCondition checks whether the timer has finished
        protected override bool ProcessCondition(Context context)
        {
            // Retrieve the instance-specific timer from the context
            if (context.TryGetData(out IsTimerOverInstance instance))
            {
                // Check if the timer is over for this specific instance
                return instance.TimerInstance.IsOver();
            }

            // Return false if no timer is initialized
            return false;
        }
    }
}
