using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NifuuLib.Extension;

namespace NifuuLib.Timer
{
    public abstract class GameTimer
    {
        public GameTimer()
        {
            CurrentTime = TimeSpan.Zero;
            Start();
        }

        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Paused;

        public TimeSpan CurrentTime { get; protected set; }
        public TimerState State { get; protected set; }

        public void Start()
        {
            State = TimerState.Running;

            Started.Raise(this, EventArgs.Empty);
            OnStarted();
        }

        public void Stop()
        {
            this.State = TimerState.Stopped;
            CurrentTime = TimeSpan.Zero;
            Stopped.Raise(this, EventArgs.Empty);
            OnStopped();
        }

        public void Restart()
        {
            Stop();
            Start();
            OnRestarted();
        }

        public void Pause()
        {
            State = TimerState.Paused;
            Paused.Raise(this, EventArgs.Empty);
            OnPaused();
        }

        public void Update(GameTime gameTime)
        {
            if (State != TimerState.Running)
            {
                return;
            }

            CurrentTime += gameTime.ElapsedGameTime;
            OnUpdate(gameTime);
        }


        protected abstract void OnStarted();
        protected abstract void OnStopped();
        protected abstract void OnRestarted();
        protected abstract void OnPaused();
        protected abstract void OnUpdate(GameTime gameTime);
    }
}
