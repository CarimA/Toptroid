using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NifuuLib.Extension;

namespace NifuuLib.Timer
{
    public class ContinuousClock : GameTimer
    {
        private TimeSpan _Interval { get; set; }
        public int TicksRaised { get; protected set; }
        public event EventHandler Tick;

        public ContinuousClock(TimeSpan interval)
        {
            SetInterval(interval);
            TicksRaised = 0;
        }

        public void SetInterval(TimeSpan interval)
        {
            _Interval = interval;
        }

        protected override void OnStarted() { }

        protected override void OnStopped() { }

        protected override void OnRestarted() { }

        protected override void OnPaused() { }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (CurrentTime >= _Interval)
            {
                TicksRaised++;
                Tick.Raise(this, EventArgs.Empty);
                Restart();
            }
        }
    }
}
