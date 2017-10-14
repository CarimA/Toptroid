using Microsoft.Xna.Framework;
using System;

namespace NifuuLib.Input
{
    public interface IInputListener
    {
        event EventHandler OnButtonPressed;

        void Update(GameTime gameTime);
    }
}