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
    public class Particle : IPoolable
    {
        public bool InUse { get; set; }
        public ParticleOptions Options { get; set; }

        private Rectangle _Source;
        internal float _Life;
        private float _MaxLife;
        private Vector2 _Position;
        private Vector2 _Velocity;
        private Vector2 _Acceleration;
        private float _Scale;
        private float _MaxScale;
        private float _Alpha;

        public Particle()
        {
            InUse = false;
        }

        public Particle(ParticleOptions options) : base()
        {
            SetOptions(options);
        }

        public void SetOptions(ParticleOptions options)
        {
            Options = options;
            Initialise();
            _Life = Options.RandomInstance.NextSingle(0, Options.MaxLife);
        }

        public void Initialise()
        {
            var r = Options.RandomInstance;
            _MaxLife = r.NextSingle(Options.MinLife, Options.MaxLife);
            _Life = _MaxLife;
            _Position = r.NextPoint(Options.BoxEmitter);
            _Velocity = new Vector2(r.NextSingle(Options.MinVelocity.X, Options.MaxVelocity.X), r.NextSingle(Options.MinVelocity.Y, Options.MaxVelocity.Y));
            _Acceleration = new Vector2(r.NextSingle(Options.MinAcceleration.X, Options.MaxAcceleration.X), r.NextSingle(Options.MinAcceleration.Y, Options.MaxAcceleration.Y));
            _MaxScale = r.NextSingle(Options.MinScale, Options.MaxScale);
            _Source = r.RandomItem(Options.TextureSources);
        }

        public void Update(GameTime gameTime)
        {
            _Life -= gameTime.GetElapsedSeconds();
            if (_Life <= 0)
            {
                InUse = false;
                return;
            }

            _Alpha = _Life / _MaxLife;
            _Scale = _MaxScale * _Alpha;

            _Position += _Velocity * gameTime.GetElapsedSeconds();
            _Velocity += _Acceleration * gameTime.GetElapsedSeconds();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Options.Texture, _Position, _Source, Color.Lerp(Options.StartTint, Options.EndTint, 1 - _Alpha), 0f, new Vector2(_Source.Width / 2, _Source.Height / 2), _Scale, SpriteEffects.None, 0);
        }
    }
}
