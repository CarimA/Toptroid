using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NifuuLib.Camera;
using NifuuLib.Input;
using NifuuLib.StateMachine.ScreenMachine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib
{
    public class NifuuGame : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public ResolutionScaler ResolutionScaler { get; internal set; }
        public RenderTarget2D _ResolutionBuffer;

        private RenderTarget2D _LastFrame;

        public ScreenMachine ScreenMachine { get; internal set; }
        public SpriteFont Font { get; internal set; }
        public IInputListener Input { get; internal set; }

        public Vector2 RenderSize { get; internal set; }

        private SamplerState _Sampler = SamplerState.PointClamp;
        private string _FontLoc;
        private string _FadeLoc;
        private Vector2 _RenderRes;

        private IntPtr _DrawSurface;

        public NifuuGame(IntPtr? drawSurface, Vector2 RenderResolution, Vector2 DisplayResolution, string FontLocation, string FadeLocation, SamplerState Sampler, bool AllowWindowResize = false, bool DisplayMouse = true)
        {
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(this);

            _RenderRes = RenderResolution;
            SetResolution(DisplayResolution);

            Window.AllowUserResizing = AllowWindowResize;
            IsMouseVisible = DisplayMouse;

            _Sampler = Sampler;
            _FontLoc = FontLocation;
            _FadeLoc = FadeLocation;

            if (drawSurface != null)
            {
                _DrawSurface = drawSurface.Value;
                Graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;

                System.Windows.Forms.Control f = System.Windows.Forms.Form.FromHandle(this.Window.Handle);
                f.VisibleChanged += F_VisibleChanged;
            }
        }

        private void F_VisibleChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.Control f = System.Windows.Forms.Form.FromHandle(this.Window.Handle);
            if (f.Visible == true)
                f.Visible = false;
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = _DrawSurface;
        }

        protected override void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>(_FontLoc);

            SetVirtualResolution(_RenderRes);
            Components.Add(ResolutionScaler);

            ScreenMachine = new ScreenMachine(this, Content.Load<Texture2D>(_FadeLoc));
            Components.Add(ScreenMachine);

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // specifically don't call base.draw
            DrawTransform(DrawFrame(gameTime));
        }

        public RenderTarget2D DrawFrame(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_ResolutionBuffer);

            GraphicsDevice.Clear(Color.CornflowerBlue); // Color.TransparentBlack);
            base.Draw(gameTime);

            return _ResolutionBuffer;
        }

        public void DrawTransform(RenderTarget2D frame)
        {
            _LastFrame = frame;

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            ResolutionScaler.BeginDraw();
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, _Sampler, null, null, null, ResolutionScaler.ScaleMatrix);
            SpriteBatch.Draw(frame, Vector2.Zero, Color.White);
            SpriteBatch.End();
        }

        public void DrawText(string text, Vector2 position, Color color, float depth = 1f)
        {
            Vector2 stringSize = Font.MeasureString(text);
            SpriteBatch.DrawString(Font, text, position, color, 0f, stringSize / 2, 1f, SpriteEffects.None, depth);
        }

        public void SetVirtualResolution(Vector2 VirtualResolution)
        {
            RenderSize = VirtualResolution;
            ResolutionScaler = new ResolutionScaler(this, ref Graphics, RenderSize);
            _ResolutionBuffer = new RenderTarget2D(this.GraphicsDevice, (int)RenderSize.X, (int)RenderSize.Y);
        }

        public void SetResolution(Vector2 DisplayResolution)
        {
            Graphics.PreferredBackBufferWidth = (int)(DisplayResolution.X);
            Graphics.PreferredBackBufferHeight = (int)(DisplayResolution.Y);
            Graphics.ApplyChanges();
        }

        public void SaveScreenshot(string filename = null)
        {
            // first, decide a filename. 
            if (filename == null)
            {
                DateTime now = DateTime.Now;
                filename = $"screenshots/{now.ToString("yyyy-MM-dd_HH-mm")}_{now.Ticks}.png";
                Directory.CreateDirectory("screenshots");
            }
            else
            {
                if (filename.Substring(-4) != ".png")
                {
                    throw new ArgumentException("Filename provided must end in .png");
                }
            }

            using (Stream stream = File.OpenWrite(filename))
            {
                (_LastFrame as Texture2D).SaveAsPng(stream, _LastFrame.Width, _LastFrame.Height);
            }


        }
    }
}

/*
{
    "id": "aGFd",
    "objective-host": "insert UUID of user determining spawns here",
    "users": [
        {
            "name": "Carim",
            "id": "insert long UUID here",
            "address": "127.0.0.1"
        }
    ],
    "status": "ready or waiting"
}

Quick Play
Play with Friends -> Host(do later)
                     Join
Local Play(do later)

matchmaking; client queries matchmaking server(/quickplay) which gives a room id
clients can then query(/room/{ id}) every 5 seconds to check if anybody has joined
when 8 players are available, the players then all initialise a connection and query back(/room/{ id}/ready) when they have a connection with all 7 peers.
The objective host then queries(/room/{ id}) to check the status, when it has changed to ready, the matchmaking server unloads the room and the game commences


each user processes their own collision + scoring
position + kills are sent at a 20hz tickrate
other clients should interpolate positions between them(and calculate a direction to smooth it out)
objective host processes enemies spawning and wave times

IF THE OBJECTIVE HOST DISCONNECTS, THE ROOM DISBANDS
*/