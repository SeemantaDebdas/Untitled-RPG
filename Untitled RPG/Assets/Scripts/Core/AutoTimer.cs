using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AutoTimer
    {
        Action onTimerEnd = null;
        public float RemainingSeconds { get; private set; }
        CoroutineHandle handle;
        public AutoTimer(){}

        public AutoTimer(float seconds, Action onTimerEnd)
        {
            RemainingSeconds = seconds;
            this.onTimerEnd = onTimerEnd;
            
            StopTimer();
            StartTimer();
        }

        public void SetTime(float seconds, Action onTimerEnd = default)
        {
            RemainingSeconds = seconds;
            this.onTimerEnd = onTimerEnd;
        }

        public void SetTimeAndStartTimer(float seconds, Action onTimerEnd = default)
        {
            StopTimer();
            SetTime(seconds, onTimerEnd);
            StartTimer();
        }

        public bool IsOver() => RemainingSeconds <= 0;

        public void StartTimer()
        {
            handle = Timing.RunCoroutine(Tick());
        }

        public void StopTimer()
        {
            if (handle.IsValid)
            {
                Timing.KillCoroutines(handle); // Stop the current coroutine if it is running
            }
        }
        
        IEnumerator<float> Tick()
        {
            while (RemainingSeconds > 0)
            {
                RemainingSeconds -= Time.deltaTime;

                yield return Timing.WaitForOneFrame;
            }

            onTimerEnd?.Invoke();
        }
    }
}
