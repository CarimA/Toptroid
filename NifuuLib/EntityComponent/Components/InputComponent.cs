using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NifuuLib.Input;
using Microsoft.Xna.Framework.Input;

namespace NifuuLib.EntityComponent.Components
{
    public class InputComponent : Component
    {
        readonly InputManager _inputManager;

        public InputComponent(InputManager inputManager)
        {
            _inputManager = inputManager;
            SetupInput();
        }

        private void SetupInput()
        {
            _inputManager.RegisterAction("left", (GameTime gameTime) =>
            {
                PositionComponent position = BaseObject.GetComponent<PositionComponent>();
                if (position != null)
                {
                    position.Position += new Vector2(-10 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                }
            });
            _inputManager.RegisterAction("right", (GameTime gameTime) =>
            {
                PositionComponent position = BaseObject.GetComponent<PositionComponent>();
                if (position != null)
                {
                    position.Position += new Vector2(10 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                }
            });
            _inputManager.RegisterAction("up", (GameTime gameTime) =>
            {
                PositionComponent position = BaseObject.GetComponent<PositionComponent>();
                if (position != null)
                {
                    position.Position += new Vector2(0, -10 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            });
            _inputManager.RegisterAction("down", (GameTime gameTime) =>
            {
                PositionComponent position = BaseObject.GetComponent<PositionComponent>();
                if (position != null)
                {
                    position.Position += new Vector2(0, 10 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            });

            _inputManager.RegisterKey(EventType.OnDown, Keys.W, "up");
            _inputManager.RegisterKey(EventType.OnDown, Keys.A, "left");
            _inputManager.RegisterKey(EventType.OnDown, Keys.S, "down");
            _inputManager.RegisterKey(EventType.OnDown, Keys.D, "right");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
