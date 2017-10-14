using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NifuuLib.Extension;

namespace NifuuLib.Timer
{
    public class CountdownTimer : GameTimer
    {
        private TimeSpan _Interval { get; set; }
        public TimeSpan TimeRemaining { get; private set; }

        public event EventHandler TimeRemainingChanged;
        public event EventHandler Completed;

        public CountdownTimer(TimeSpan interval)
        {
            SetInterval(interval);
        }

        public void SetInterval(TimeSpan interval)
        {
            _Interval = interval;
        }

        public void Complete()
        {
            State = TimerState.Completed;
            CurrentTime = _Interval;
            TimeRemaining = TimeSpan.Zero;
            Completed.Raise(this, EventArgs.Empty);
        }

        public void IncreaseTimer(TimeSpan time)
        {
            SetInterval(CurrentTime + time);
        }

        public void DecreaseTimer(TimeSpan time)
        {
            SetInterval(CurrentTime - time);
        }

        protected override void OnStarted() { }

        protected override void OnStopped() { }

        protected override void OnRestarted() { }

        protected override void OnPaused() { }

        protected override void OnUpdate(GameTime gameTime)
        {
            TimeRemaining = _Interval - CurrentTime;
            TimeRemainingChanged.Raise(this, EventArgs.Empty);

            if (CurrentTime >= _Interval)
            {
                Complete();
            }
        }
    }
}
