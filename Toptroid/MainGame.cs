using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NifuuLib.Camera;
using System;
using System.Collections.Generic;
using System.Linq;

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

        Vector2 Position = new Vector2(60, 60);

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

        public bool playerCollides(Vector2 position, int radius, out Vector2 point)
        {
            List<Vector2> points = new List<Vector2>();

            for (int x = (int)position.X - radius; x < (int)position.X + radius; x++)
            {
                for (int y = (int)position.Y - radius; y < (int)position.Y + radius; y++)
                {
                    float distance = Vector2.Distance(position, new Vector2(x, y));
                    if (distance > radius) continue;

                    if (mask[x, y])
                    {
                        points.Add(new Vector2(x, y));
                    }
                }
            }
            if (points.Count > 0)
            {
                points.Sort((a, b) =>
                {
                    float distA = Vector2.Distance(position, a);
                    float distB = Vector2.Distance(position, b);
                    if (distA < distB)
                    {
                        return -1;
                    }
                    else if (distA > distB)
                    {
                        return 1;
                    } else
                    {
                        return 0;
                    }
                });
                point = points.First();
                return true;
            }
            else
            {
                point = position;
                return false;
            }
        }

        Vector2 reticle;
        GamePadState lastState;
        int playerRadius = 8;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed && lastState.Buttons.B == ButtonState.Released)
            {
                cam.ShakeScreen(10f, TimeSpan.FromSeconds(10));
            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && lastState.Buttons.Start == ButtonState.Released)
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                graphics.ApplyChanges();
            }

            if (GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.8f && lastState.Triggers.Right < 0.8f)
            {
                if (playerRadius == 8)
                    playerRadius = 3;
                else
                {
                    if (!playerCollides(Position, 8, out _))
                    {
                        playerRadius = 8;
                    }
                }
            }

            lastState = GamePad.GetState(PlayerIndex.One);

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

            if (!playerCollides(Position + Velocity, playerRadius, out _)) // mask[(int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y)])
            {
                Position += Velocity;
            }
            else
            {
                for (int i = 0; i < Math.Abs(Velocity.X); i++)
                {
                    if (playerCollides(Position + new Vector2(Math.Sign(Velocity.X), 0), playerRadius, out _))
                    {
                        break;
                    }
                    Position.X += Math.Sign(Velocity.X);
                }
                for (int i = 0; i < Math.Abs(Velocity.Y); i++)
                {
                    if (playerCollides(Position + new Vector2(0, Math.Sign(Velocity.Y)), playerRadius, out _))
                    {
                        break;
                    }
                    Position.Y += Math.Sign(Velocity.Y);
                }
                
                // try to warp around if possible...
                Vector2 newPosition = (Position + Velocity);
                Vector2 colPoint;
                if (playerCollides(Position + Velocity, playerRadius, out colPoint) && dir != Vector2.Zero)
                {
                    // todo: figure out bouncing issue
                    Vector2 wallNormal = new Vector2((int)(colPoint.X - newPosition.X), (int)(colPoint.Y - newPosition.Y));
                    wallNormal.Normalize();

                    Vector2 undesiredMotion = wallNormal * Vector2.Dot(Velocity, wallNormal);
                    Vector2 desiredMotion = Velocity - undesiredMotion;
                    //desiredMotion.Normalize();

                    if (!(wallNormal.X == 0 || wallNormal.Y == 0))
                    {
                        //System.Diagnostics.Debug.Print(wallNormal.ToString());

                        for (int i = 0; i < Math.Abs(desiredMotion.X); i++)
                        {
                            if (playerCollides(Position + new Vector2(Math.Sign(desiredMotion.X), 0), playerRadius, out _))
                            {
                                break;
                            }
                            Position.X += Math.Sign(desiredMotion.X);
                        }
                        for (int i = 0; i < Math.Abs(desiredMotion.Y); i++)
                        {
                            if (playerCollides(Position + new Vector2(0, Math.Sign(desiredMotion.Y)), playerRadius, out _))
                            {
                                break;
                            }
                            Position.Y += Math.Sign(desiredMotion.Y);
                        }
                    }
                }

            }

            // todo: figure out a good tolerance to stop the jittering
            cam.Move(Position, Velocity, 2f, 1.8f, 0.275f);
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
            spriteBatch.Draw(reticleTex, reticle, new Rectangle(0, 0, 32, 32), Color.White, 0, new Vector2(16, 16), reticleAlpha, SpriteEffects.None, 1f);

            spriteBatch.Draw(viewableArea, cam.ScreenToWorld(Vector2.Zero), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}