using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NifuuLib.Collection;
using NifuuLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.ParticleSystem
{
    public class Emitter : Pool<Particle>
    {
        public bool Enabled { get; }
        
        public Emitter(int capacity, ParticleOptions particleOptions, bool startFresh = true, bool setRandom = true) : base(capacity, !startFresh)
        {
            if (setRandom)
            {
                particleOptions.RandomInstance = new Random();
            }

            for (var i = 0; i < capacity; i++)
            {
                _Objects[i].SetOptions(particleOptions);
            }
        }

        public void Update(GameTime gameTime)
        {
            this.IterateAction((particle) =>
            {
                particle.Update(gameTime);
            });

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            this.IterateActionSync((particle) =>
            {
                particle.Draw(gameTime, spriteBatch);
            });

            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}
