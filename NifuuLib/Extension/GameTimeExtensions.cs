using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Extension
{
    public static class GameTimeExtensions
    {
        public static float GetElapsedSeconds(this GameTime gameTime) => (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}
