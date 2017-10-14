using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NifuuLib.Camera;
using System;

namespace Toptroid
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public static class Texture2dHelper
    {
        public static Color GetPixel(this Color[] colors, int x, int y, int width)
        {
            return colors[x + (y * width)];
        }
        public static Color[] GetPixels(this Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors1D);
            return colors1D;
        }
    }

    public class MainGame : Game
    {
        ResolutionIndependantCamera cam;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D maskTex;
        Texture2D pixel;
        Texture2D reticleTex;
        Texture2D viewableArea;

        bool[,] mask;

        int reticleLength = 100;

        Vector2 Position = new Vector2(150, 200);

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            graphics.PreferredBackBufferHeight = 540;
            graphics.PreferredBackBufferWidth = 960;
            graphics.ApplyChanges();

            cam = new ResolutionIndependantCamera(this, graphics, 256, 224);
            Components.Add(cam);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            maskTex = Content.Load<Texture2D>("mask");
            pixel = Content.Load<Texture2D>("pixel");
            reticleTex = Content.Load<Texture2D>("reticle");
            viewableArea = Content.Load<Texture2D>("viewable_area");

            mask = new bool[maskTex.Width, maskTex.Height];
            Color[] data = maskTex.GetPixels();
            
            for (int x = 0; x < maskTex.Width; x++)
            {
                for (int y = 0; y < maskTex.Height; y++)
                {
                    Color col = data.GetPixel(x, y, maskTex.Width);
                    if (col == Color.Magenta)
                    {
                        mask[x, y] = true;
                    }
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        public bool playerCollides(Vector2 position, int radius)
        {
            for (int x = (int)position.X - radius; x < (int)position.X + radius; x++)
            {
                for (int y = (int)position.Y - radius; y < (int)position.Y + radius; y++)
                {
                    float distance = Vector2.Distance(position, new Vector2(x, y));
                    if (distance > radius) continue;

                    if (mask[x, y]) return true;
                }
            }
            return false;
        }

        Vector2 reticle;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your update logic here
            /*Point mousePos = Mouse.GetState().Position;
            if (mask[mousePos.X, mousePos.Y])
            {
                Window.Title = "COLLIDED";
            } else
            {
                Window.Title = "NOT COLLIDED";
            }*/

            Vector2 pointer = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
            pointer.Y *= -1;
            pointer.Normalize();

            Vector2 tempReticle;

            Physics.Raycast(Position, pointer, reticleLength, mask, out tempReticle, out _);

            reticle = Vector2.Lerp(reticle, tempReticle, 0.25f);

            KeyboardState ks = Keyboard.GetState();
            float speed = 100f * deltaTime;

            Vector2 dir = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            dir.Y *= -1;

            if (ks.IsKeyDown(Keys.Up)) dir.Y -= 1;
            if (ks.IsKeyDown(Keys.Down)) dir.Y += 1;
            if (ks.IsKeyDown(Keys.Left)) dir.X -= 1;
            if (ks.IsKeyDown(Keys.Right)) dir.X += 1;

            Vector2 Velocity = dir * speed;

            if (!playerCollides(Position + Velocity, 8)) // mask[(int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y)])
            {
                Position += Velocity;
            } else
            {
                // first, try to warp around if possible...
                float deadzone = 0.75f;
                if (Velocity.X > -deadzone && Velocity.X < deadzone)
                {
                    if (playerCollides(Position + new Vector2(1, 0), 8)) {
                        Velocity.X = -deadzone * 2;
                    }
                    if (playerCollides(Position - new Vector2(1, 0), 8)) {
                        Velocity.X = deadzone * 2;
                    }
                }
                if (Velocity.Y > -deadzone && Velocity.Y < deadzone)
                {
                    if (playerCollides(Position + new Vector2(0, 1), 8))
                    {
                        Velocity.Y = -deadzone * 2;
                    }
                    if (playerCollides(Position - new Vector2(0, 1), 8))
                    {
                        Velocity.Y = deadzone * 2;
                    }
                }

                for (int i = 0; i < Math.Abs(Velocity.X); i++)
                {
                    if (playerCollides(Position + new Vector2(Math.Sign(Velocity.X), 0), 8))
                    {
                        break;
                    }
                    Position.X += Math.Sign(Velocity.X);
                }
                for (int i = 0; i < Math.Abs(Velocity.Y); i++)
                {
                    if (playerCollides(Position + new Vector2(0, Math.Sign(Velocity.Y)), 8))
                    {
                        break;
                    }
                    Position.Y += Math.Sign(Velocity.Y);
                }
            }

            cam.Move(Position, Velocity);
            //Position = Physics.Resolve(Position, dir, speed, mask);




            base.Update(gameTime);
        }

        /*
      
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;



        Vector2 pointer = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
        pointer.Y *= -1;
            pointer.Normalize();

            Vector2 tempReticle;

        Physics.Raycast(Position, pointer, reticleLength, mask, out tempReticle, out _);

            reticle = Vector2.Lerp(reticle, tempReticle, 0.25f);

            KeyboardState ks = Keyboard.GetState();
        float speed = 100f * deltaTime;

        Vector2 dir = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
        dir.Y *= -1;

            if (ks.IsKeyDown(Keys.Up)) dir.Y -= 1;
            if (ks.IsKeyDown(Keys.Down)) dir.Y += 1;
            if (ks.IsKeyDown(Keys.Left)) dir.X -= 1;
            if (ks.IsKeyDown(Keys.Right)) dir.X += 1;

            Vector2 Velocity = dir * speed;

            if (!mask[(int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y)])
            {
                Position += Velocity;
            } else
            {
                // first, try to warp around if possible...
                float deadzone = 0.4f;
                if (Velocity.X > -deadzone && Velocity.X<deadzone)
                {
                    if (mask[(int)Position.X + 2, (int)Position.Y]) {
                        Velocity.X = -deadzone* 2;
                    }
                    if (mask[(int)Position.X - 2, (int)Position.Y]) {
                        Velocity.X = deadzone* 2;
                    }
                }
                if (Velocity.Y > -deadzone && Velocity.Y<deadzone)
                {
                    if (mask[(int)Position.X, (int)Position.Y + 2])
                    {
                        Velocity.Y = -deadzone* 2;
                    }
                    if (mask[(int)Position.X, (int)Position.Y - 2])
                    {
                        Velocity.Y = deadzone* 2;
                    }
                }

                for (int i = 0; i<Math.Abs(Velocity.X); i++)
                {
                    if (mask[(int)(Position.X + Math.Sign(Velocity.X)), (int)Position.Y])
                    {
                        break;
                    }
                    Position.X += Math.Sign(Velocity.X);
                }
                for (int i = 0; i<Math.Abs(Velocity.Y); i++)
                {
                    if (mask[(int)Position.X, (int)(Position.Y + Math.Sign(Velocity.Y))])
                    {
                        break;
                    }
                    Position.Y += Math.Sign(Velocity.Y);
                }
            }

            cam.Move(Position, Velocity);
            //Position = Physics.Resolve(Position, dir, speed, mask);




            base.Update(gameTime);
        }  
    */

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform(graphics.GraphicsDevice));
            spriteBatch.Draw(maskTex, Vector2.Zero, Color.Red);

            spriteBatch.Draw(pixel, Position, new Rectangle(0, 0, 16,16), Color.White, 0, new Vector2(8, 8), 1f, SpriteEffects.None, 1f);

            float reticleAlpha = (Math.Min(Vector2.Distance(Position, reticle), 50) / 50);
            if (reticleAlpha < 0.35f)
                reticleAlpha = 0;
            spriteBatch.Draw(reticleTex, reticle, new Rectangle(0, 0, 32, 32), Color.White * reticleAlpha, 0, new Vector2(16, 16), 1f, SpriteEffects.None, 1f);

            spriteBatch.Draw(viewableArea, new Vector2(Position.X - 128, Position.Y - 112), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}