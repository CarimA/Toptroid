using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NifuuLib.StateMachine.ScreenMachine
{
    public class ScreenMachine : StateMachine<ScreenState>
    {
        private Texture2D _Black;

        private TransitionState _TransitionState = TransitionState.None;
        private string _NextID;
        private object[] _NextArgs;
        private float _TransitionPoint;

        private ScreenState _Popup;

        public ScreenMachine(Game game, Texture2D black) : base(game)
        {
            this.UpdateOrder = 0;
            this.DrawOrder = 0;
            this._Black = black;
        }

        public override void Change(string id, params object[] args)
        {
            // HOLD IT. We're gonna do this.
            this._NextArgs = args;
            this._NextID = id;

            this._TransitionState = TransitionState.ToBlack;
            this._TransitionPoint = 0;
            
            //base.Change(id, args);
        }

        public override void Update(GameTime gameTime)
        {
            if (this._TransitionState != TransitionState.None)
            {
                if (this._TransitionState == TransitionState.ToBlack)
                {
                    _TransitionPoint += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_TransitionPoint >= 1)
                    {
                        base.Change(this._NextID, this._NextArgs);
                        this._TransitionState = TransitionState.FromBlack;
                    }
                }
                else
                {
                    _TransitionPoint -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_TransitionPoint <= 0)
                    {
                        this._TransitionState = TransitionState.None;
                    }
                }
            }

            base.Update(gameTime);

            if (_Popup == null)
            {
                Current.HandleInput(gameTime);  
            }
            else
            {
                _Popup.Update(gameTime);
                _Popup.HandleInput(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_Popup != null)
            {
                Game.SpriteBatch.Begin();
                Game.SpriteBatch.Draw(_Black, new Rectangle(0, 0, (int)Game.RenderSize.X, (int)Game.RenderSize.Y), Color.Black * 0.65f);
                Game.SpriteBatch.End();

                _Popup.Draw(gameTime, Game.SpriteBatch);
            }

            if (this._TransitionState != TransitionState.None)
            {
                Game.SpriteBatch.Begin();
                Game.SpriteBatch.Draw(_Black, new Rectangle(0, 0, (int)Game.RenderSize.X, (int)Game.RenderSize.Y), Color.Black * this._TransitionPoint);
                Game.SpriteBatch.End();
            }
        }

        public void SetPopup(string id)
        {
            _Popup = States[id];
            _Popup.Enter();
        }

        public void ClearPopup()
        {
            _Popup.Exit();
            _Popup = null;
        }
    }

    public enum TransitionState
    {
        ToBlack,
        FromBlack,
        None
    }
}
