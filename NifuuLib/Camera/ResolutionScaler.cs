using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Camera
{
    // TODO: tidy up.
    public class ResolutionScaler : GameComponent
    {
        private GraphicsDeviceManager _GraphicsDeviceManager;

        public ResolutionScaling Scale = ResolutionScaling.FloatScaling;

        private Vector2 _VirtualResolution;
        private float _VirtualAspectRatio;
        public Matrix ScaleMatrix { get; private set; }
        public Viewport Viewport { get; private set; }

        bool dirty = false;

        public ResolutionScaler(Game game, ref GraphicsDeviceManager graphicsDeviceManager, Vector2 virtualResolution) : base(game)
        {
            _GraphicsDeviceManager = graphicsDeviceManager;
            SetVirtualResolution(virtualResolution);
        }

        public override void Initialize()
        {
            Game.Window.ClientSizeChanged += Window_ClientSizeChanged;

            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            InvokeResize();
        }

        public void SetVirtualResolution(Vector2 virtualResolution)
        {
            _VirtualResolution = virtualResolution;
            _VirtualAspectRatio = virtualResolution.X / virtualResolution.Y;
            dirty = true;
        }

        public void SetScaling(ResolutionScaling scale)
        {
            this.Scale = scale;
            dirty = true;
        }

        public void InvokeResize()
        {
            dirty = true;
        }

        public void BeginDraw()
        {
            if (!dirty)
            {
                Game.GraphicsDevice.Viewport = Viewport;
                return;
            }

            _GraphicsDeviceManager.PreferredBackBufferWidth = Game.Window.ClientBounds.Width;
            _GraphicsDeviceManager.PreferredBackBufferHeight = Game.Window.ClientBounds.Height;
            _GraphicsDeviceManager.ApplyChanges();

            // calculate viewport
            int width = _GraphicsDeviceManager.PreferredBackBufferWidth;
            int height = (int)(width / _VirtualAspectRatio + 0.5f);

            if (height > _GraphicsDeviceManager.PreferredBackBufferHeight)
            {
                height = _GraphicsDeviceManager.PreferredBackBufferHeight;
                width = (int)(height * _VirtualAspectRatio + 0.5f);
            }

            Viewport viewport = new Viewport();

            if (Scale == ResolutionScaling.FloatScaling)
            {
                viewport.X = (_GraphicsDeviceManager.PreferredBackBufferWidth / 2) - (width / 2);
                viewport.Y = (_GraphicsDeviceManager.PreferredBackBufferHeight / 2) - (height / 2);
                viewport.Width = width;
                viewport.Height = height;
            }
            else if (Scale == ResolutionScaling.IntegerScaling)
            {
                viewport.Width = (int)(_VirtualResolution.X * System.Math.Floor(width / _VirtualResolution.X));
                viewport.Height = (int)(_VirtualResolution.Y * System.Math.Floor(height / _VirtualResolution.Y));
                viewport.X = (_GraphicsDeviceManager.PreferredBackBufferWidth / 2) - (viewport.Width / 2);
                viewport.Y = (_GraphicsDeviceManager.PreferredBackBufferHeight / 2) - (viewport.Height / 2);
            }
            else if (Scale == ResolutionScaling.None)
            {
                viewport.Width = (int)(_VirtualResolution.X);
                viewport.Height = (int)(_VirtualResolution.Y);
                viewport.X = (_GraphicsDeviceManager.PreferredBackBufferWidth / 2) - (viewport.Width / 2);
                viewport.Y = (_GraphicsDeviceManager.PreferredBackBufferHeight / 2) - (viewport.Height / 2);
            }

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            Game.GraphicsDevice.Viewport = viewport;
            this.Viewport = viewport;


            // recreate scale matrix
            if (Scale == ResolutionScaling.FloatScaling)
            {
                ScaleMatrix = Matrix.CreateScale(
                    (float)Game.GraphicsDevice.Viewport.Width / _VirtualResolution.X,
                    (float)Game.GraphicsDevice.Viewport.Width / _VirtualResolution.X,
                    1f);
            }
            else if (Scale == ResolutionScaling.IntegerScaling)
            {
                ScaleMatrix = Matrix.CreateScale(
                    (int)System.Math.Floor(Game.GraphicsDevice.Viewport.Width / _VirtualResolution.X),
                    (int)System.Math.Floor(Game.GraphicsDevice.Viewport.Width / _VirtualResolution.X),
                    1f);
            }
            else if (Scale == ResolutionScaling.None)
            {
                ScaleMatrix = Matrix.CreateScale(
                    1f,
                    1f,
                    1f);
            }
            dirty = false;
        }
    }
}
