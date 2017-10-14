using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NifuuLib.Input;

namespace NifuuLib.StateMachine.ScreenMachine
{
    public class ScreenState : IDrawableState
    {
        protected NifuuGame Game;

        public ScreenState()
        {
            //throw new Exception("Must provide MainGame object.");
        }

        public ScreenState(NifuuGame game)
        {
            this.Game = game;

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void Enter(params object[] args)
        {

        }


        public virtual void Exit()
        {

        }
        
        public virtual void Update(GameTime gameTime)
        {

        }

        public void HandleInput(GameTime gameTime)
        {
            if (Game != null)
            {
                if (Game.Input != null)
                {
                    Game.Input.Update(gameTime);
                }
            }
        }
    }
}
