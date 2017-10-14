using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NifuuLib.Timer
{
    public class StopwatchTimer : GameTimer
    {
        public StopwatchTimer()
        {

        }

        protected override void OnPaused() { }

        protected override void OnRestarted() { }

        protected override void OnStarted() { }

        protected override void OnStopped() { }

        protected override void OnUpdate(GameTime gameTime) { }
    }
}
