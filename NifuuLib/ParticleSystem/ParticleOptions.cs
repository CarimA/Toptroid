using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.ParticleSystem
{
    public class ParticleOptions
    {
        public Random RandomInstance { get; set; }

        public Texture2D Texture { get; set; }

        public List<Rectangle> TextureSources { get; set; }
        public Rectangle TextureSource { set
            {
                TextureSources = new List<Rectangle>() { value };
            }
        }

        public Rectangle BoxEmitter { get; set; }

        public Vector2 MinVelocity { get; set; }
        public Vector2 MaxVelocity { get; set; }
        public Vector2 Velocity
        {
            set
            {
                MinVelocity = value;
                MaxVelocity = value;
            }
        }

        public Vector2 MinAcceleration { get; set; }
        public Vector2 MaxAcceleration { get; set; }
        public Vector2 Acceleration
        {
            set
            {
                MinAcceleration = value;
                MaxAcceleration = value;
            }
        }

        public float MinLife { get; set; }
        public float MaxLife { get; set; }
        public float Life
        {
            set
            {
                MinLife = value;
                MaxLife = value;
            }
        }

        public float MinScale { get; set; }
        public float MaxScale { get; set; }
        public float Scale
        {
            set
            {
                MinScale = value;
                MaxScale = value;
            }
        }

        public Color StartTint { get; set; }
        public Color EndTint { get; set; }
        public Color Tint
        {
            set
            {
                StartTint = value;
                EndTint = value;
            }
        }
    }
}
