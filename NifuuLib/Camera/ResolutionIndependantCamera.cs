using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static System.Math;
using NifuuLib.Extension;

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
        private float shakeIntensity;
        private float shakeTime;
        private Vector2 shake;
        private Random rand = new Random();

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

            if (shakeTime < 0)
            {
                shakeTime = 0;
                shakeIntensity = 0;
                shake = Vector2.Zero; 
            }
            else if (shakeTime > 0)
            {
                Vector2 next = rand.NextPoint(new Rectangle(-(int)shakeIntensity, -(int)shakeIntensity, (int)shakeIntensity * 2, (int)shakeIntensity * 2));
                shake = Vector2.Lerp(shake, next, 0.25f);
                shake *= Min(shakeTime, 1f);
                shakeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        public void ShakeScreen(float intensity, TimeSpan time)
        {
            shakeIntensity += intensity;
            shakeTime += (float)time.TotalSeconds;
        }

        public void SetTargetResolution(GraphicsDevice graphics, float width, float height)
        {
            gameWidth = width;
            gameHeight = height;
            float scaleWidth = graphics.Viewport.Width / width;
            float scaleHeight = graphics.Viewport.Height / height;
            Scale = Min(scaleWidth, scaleHeight);
        }

        public void Move(Vector2 target, Vector2 velocity, float velocityScale, float maxVelocity, float tween)
        {
            // todo: something about the jumping + make camera able to look ahead
            Vector2 trueVelocity = (velocity * velocityScale);
            if (trueVelocity.X > maxVelocity) trueVelocity.X = maxVelocity;
            if (trueVelocity.Y > maxVelocity) trueVelocity.Y = maxVelocity;
            if (trueVelocity.X < -maxVelocity) trueVelocity.X = -maxVelocity;
            if (trueVelocity.Y < -maxVelocity) trueVelocity.Y = -maxVelocity;

            Position = Vector2.SmoothStep(Position, target + trueVelocity, tween);
        }

        public Matrix GetTransform(GraphicsDevice graphics)
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X + shake.X, -Position.Y + shake.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Scale, Scale, 1)) *
                Matrix.CreateTranslation(new Vector3(graphics.Viewport.Width * 0.5f, graphics.Viewport.Height * 0.5f, 0));

            return Transform;
        }
    }
}
