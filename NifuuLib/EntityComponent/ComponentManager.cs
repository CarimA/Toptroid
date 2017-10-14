using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.EntityComponent
{
    public class ComponentManager : DrawableGameComponent
    {
        private readonly List<BaseObject> _BaseObjects;
        public readonly Dictionary<Type, List<BaseObject>> Components;

        public ComponentManager(Game game) : base(game)
        {
            _BaseObjects = new List<BaseObject>();
            Components = new Dictionary<Type, List<BaseObject>>();
        }

        public void AddBaseObject(BaseObject baseObject)
        {
            _BaseObjects.Add(baseObject);
        }

        public List<BaseObject> GetBaseObjectWithComponent<TComponentType>() where TComponentType : Component
        {
            return (Components[typeof(TComponentType)]);
        }

        public override void Update(GameTime gameTime)
        {
            _BaseObjects.ForEach((baseObject) =>
            {
                baseObject.Update(gameTime);
            });
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (Game as NifuuGame).SpriteBatch;

            spriteBatch.Begin();
            _BaseObjects.ForEach((baseObject) =>
            {
                baseObject.Draw((Game as NifuuGame).SpriteBatch, gameTime);
            });
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
