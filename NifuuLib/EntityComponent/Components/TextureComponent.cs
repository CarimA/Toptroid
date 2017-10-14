using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NifuuLib.EntityComponent.Components
{
    public class TextureComponent : Component
    {
        public Texture2D _Texture;

        public TextureComponent(Texture2D texture)
        {
            _Texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // todo: temp testing crap
            PositionComponent position = BaseObject.GetComponent<PositionComponent>();
            if (position != null)
            {
                spriteBatch.Draw(_Texture, position.Position, Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
