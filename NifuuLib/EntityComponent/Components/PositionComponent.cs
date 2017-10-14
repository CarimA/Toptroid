using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace NifuuLib.EntityComponent.Components
{
    public class PositionComponent : Component
    {
        public Vector2 Position;

        public PositionComponent(Vector2 position)
        {
            Position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
