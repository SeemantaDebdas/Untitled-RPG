using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Timer
    {
        float seconds = 0;
        readonly Action onTimerEnd = null;
        public float RemainingSeconds { get; private set; }

        public Timer(float seconds, Action onTimerEnd)
        {
            this.seconds = seconds;
            this.onTimerEnd = onTimerEnd;

            Timing.RunCoroutine(Tick());
        }

        public bool IsOver() => RemainingSeconds <= 0;


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
