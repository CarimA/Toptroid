using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.EntityComponent
{
    public abstract class Component
    {
        public BaseObject BaseObject { get; internal set; }

        public void Initialize(BaseObject baseObject)
        {
            BaseObject = baseObject;
        }

        public void RemoveMe()
        {
            BaseObject.RemoveComponent(this);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
