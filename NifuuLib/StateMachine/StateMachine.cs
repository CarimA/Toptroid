using Microsoft.Xna.Framework;
using NifuuLib.StateMachine.ScreenMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.StateMachine
{
    public class StateMachine<T> : DrawableGameComponent where T : class, IDrawableState, new()
    {
        public Dictionary<string, T> States { get; private set; }
        private T _CurrentState;
        private string _CurrentID;

        public T Current { get { return _CurrentState; } }
        public string CurrentID {  get { return _CurrentID;  } }

        public new NifuuGame Game { get { return (NifuuGame)base.Game; } }

        public StateMachine(Game game) : base(game)
        {
            States = new Dictionary<string, T>();
            _CurrentState = new T();
        }

        public virtual void Add(string id, T state)
        {
            States.Add(id, state);
        }

        public virtual bool Remove(string id)
        {
            return States.Remove(id);
        }

        public virtual void Clear()
        {
            States.Clear();
        }

        public virtual void Change(string id, params object[] args)
        {
            _CurrentState.Exit();
            T next = States[id];
            _CurrentID = id;
            next.Enter(args);
            _CurrentState = next;
        }

        public override void Update(GameTime gameTime)
        {
            if (_CurrentState != null)
            {
                _CurrentState.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_CurrentState != null)
            {
                if (_CurrentState is IDrawableState)
                {
                    (_CurrentState as IDrawableState).Draw(gameTime, Game.SpriteBatch);
                }
            }
            base.Draw(gameTime);
        }
    }
}
