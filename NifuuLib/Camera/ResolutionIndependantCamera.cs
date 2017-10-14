using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static System.Math;

namespace NifuuLib.Camera
{
    public class ResolutionIndependantCamera : GameComponent
    {
        protected float Scale { get; set; }
        protected Vector2 Position { get; set; }
        protected float Rotation { get; set; }

        public Matrix Transform { get; private set; }

        protected GraphicsDeviceManager GraphicsDevice;
        private bool windowIsResizing = false;

        private float gameWidth, gameHeight;

        public ResolutionIndependantCamera(Game game, GraphicsDeviceManager graphicsDevice, int width, int height) : base(game)
        {
            game.Window.ClientSizeChanged += (object sender, EventArgs e) =>
            {
                windowIsResizing = true;
            };
            GraphicsDevice = graphicsDevice;
            SetTargetResolution(graphicsDevice.GraphicsDevice, (float)width, (float)height);
        }

        public override void Update(GameTime gameTime)
        {
            if (windowIsResizing)
            {
                windowIsResizing = false;

                GraphicsDevice.PreferredBackBufferWidth = Game.Window.ClientBounds.Width;
                GraphicsDevice.PreferredBackBufferHeight = Game.Window.ClientBounds.Height;
                GraphicsDevice.ApplyChanges();

                SetTargetResolution(GraphicsDevice.GraphicsDevice, gameWidth, gameHeight);
            }
            base.Update(gameTime);
        }

        public void SetTargetResolution(GraphicsDevice graphics, float width, float height)
        {
            gameWidth = width;
            gameHeight = height;
            float scaleWidth = graphics.Viewport.Width / width;
            float scaleHeight = graphics.Viewport.Height / height;
            Scale = Min(scaleWidth, scaleHeight);
        }

        public void Move(Vector2 target, Vector2 velocity)
        {
            Position = Vector2.SmoothStep(Position, target + (velocity * 35f), 0.12f);
        }

        public Matrix GetTransform(GraphicsDevice graphics)
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 1)) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0));

            return Transform;
        }
    }
}
