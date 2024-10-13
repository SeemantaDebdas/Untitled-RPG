using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Timer
    {
        readonly Action onTimerEnd = null;
        public float RemainingSeconds { get; private set; }

        public Timer(float seconds, Action onTimerEnd)
        {
            RemainingSeconds = seconds;
            this.onTimerEnd = onTimerEnd;
        }

        public bool IsOver() => RemainingSeconds <= 0;

        public void Tick(float deltaTime)
        {
            RemainingSeconds -= deltaTime;

            if (RemainingSeconds < 0)
            {
                RemainingSeconds = 0;
                onTimerEnd();
            }
        }
    }

}
