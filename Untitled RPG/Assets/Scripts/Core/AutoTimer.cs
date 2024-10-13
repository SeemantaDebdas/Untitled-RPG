using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AutoTimer
    {
        float seconds = 0;
        readonly Action onTimerEnd = null;
        public float RemainingSeconds { get; private set; }
        CoroutineHandle handle;

        public AutoTimer(float seconds, Action onTimerEnd)
        {
            this.seconds = seconds;
            this.onTimerEnd = onTimerEnd;

            handle = Timing.RunCoroutine(Tick());
        }

        public bool IsOver() => RemainingSeconds <= 0;

        public void StopTimer()
        {
            if (handle.IsValid)
            {
                Timing.KillCoroutines(handle); // Stop the current coroutine if it is running
            }
        }

        IEnumerator<float> Tick()
        {
            while (seconds > 0)
            {
                seconds -= Time.deltaTime;
                RemainingSeconds = seconds;

                yield return Timing.WaitForOneFrame;
            }

            onTimerEnd?.Invoke();
        }
    }
}
