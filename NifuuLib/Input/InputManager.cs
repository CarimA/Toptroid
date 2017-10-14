using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Input
{
    public enum EventType
    {
        OnDown,
        OnUp,
        OnPressed,
        OnReleased
    }

    public class InputManager : GameComponent
    {
        private GamePadState _currentGamepadState;
        private GamePadState _lastGamepadState;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;

        private Dictionary<dynamic, string> _keybindOnDown;
        private Dictionary<dynamic, string> _keybindOnUp;
        private Dictionary<dynamic, string> _keybindOnPressed;
        private Dictionary<dynamic, string> _keybindOnReleased;

        private Dictionary<string, Action<GameTime>> _actions;

        public InputManager(Game game) : base(game)
        {
            _keybindOnDown = new Dictionary<dynamic, string>();
            _keybindOnUp = new Dictionary<dynamic, string>();
            _keybindOnPressed = new Dictionary<dynamic, string>();
            _keybindOnReleased = new Dictionary<dynamic, string>();
            _actions = new Dictionary<string, Action<GameTime>>();
        }

        public void RegisterAction(string id, Action<GameTime> action)
        {
            _actions.Add(id, action);
        }

        public void RegisterKey(EventType ev, dynamic inputPress, string action)
        {
            if (!(inputPress is Keys || inputPress is Buttons))
            {
                throw new Exception("Invalid keybind type");
            }

            switch (ev)
            {
                case EventType.OnDown:
                    _keybindOnDown.Add(inputPress, action);
                    break;
                case EventType.OnUp:
                    _keybindOnUp.Add(inputPress, action);
                    break;
                case EventType.OnPressed:
                    _keybindOnPressed.Add(inputPress, action);
                    break;
                case EventType.OnReleased:
                    _keybindOnReleased.Add(inputPress, action);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastGamepadState = _currentGamepadState;
            _currentGamepadState = GamePad.GetState(PlayerIndex.One);

            CheckEvents(_keybindOnDown, EventType.OnDown, gameTime);
            CheckEvents(_keybindOnUp, EventType.OnUp, gameTime);
            CheckEvents(_keybindOnPressed, EventType.OnPressed, gameTime);
            CheckEvents(_keybindOnReleased, EventType.OnReleased, gameTime);
        }

        private void CheckEvents(Dictionary<dynamic, string> events, EventType ev, GameTime gameTime)
        {
            foreach (KeyValuePair<dynamic, string> e in events)
            {
                if (e.Key is Keys)
                {
                    switch (ev)
                    {
                        case EventType.OnDown:
                            if (IsKeyDown(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnUp:
                            if (IsKeyUp(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnPressed:
                            if (IsKeyPressed(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnReleased:
                            if (IsKeyReleased(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                    }
                }
                else if (e.Key is Buttons)
                {
                    switch (ev)
                    {
                        case EventType.OnDown:
                            if (IsButtonDown(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnUp:
                            if (IsButtonUp(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnPressed:
                            if (IsButtonPressed(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                        case EventType.OnReleased:
                            if (IsButtonReleased(e.Key))
                                _actions[e.Value](gameTime);
                            break;
                    }
                }
                else
                {
                    throw new Exception("Invalid keybind type");
                }
            }
        }

        public bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public bool IsKeyPressed(Keys key) => _currentKeyboardState.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
        public bool IsKeyReleased(Keys key) => _currentKeyboardState.IsKeyUp(key) && _lastKeyboardState.IsKeyDown(key);

        public bool IsButtonDown(Buttons button) => _currentGamepadState.IsButtonDown(button);
        public bool IsButtonUp(Buttons button) => _currentGamepadState.IsButtonUp(button);
        public bool IsButtonPressed(Buttons button) => _currentGamepadState.IsButtonDown(button) && _lastGamepadState.IsButtonUp(button);
        public bool IsButtonReleased(Buttons button) => _currentGamepadState.IsButtonUp(button) && _lastGamepadState.IsButtonDown(button);


    }
}
