using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NifuuLib.StateMachine.ScreenMachine
{
    public class MenuState : ScreenState 
    {
        protected List<MenuEntry> MenuEntries;
        private int _SelectedEntry;
        private string _MenuTitle;
        private Vector2 _MenuSize;

        public MenuState() : base() { }

        public MenuState(NifuuGame game, Vector2 menuSize) : this(game, menuSize, "") { }
        public MenuState(NifuuGame game, Vector2 menuSize, string menuTitle) : base(game)
        {
            this._MenuTitle = menuTitle;
            this._MenuSize = menuSize;
            this.MenuEntries = new List<MenuEntry>();
            SetSelectedEntry(0);
        }

        public void SetSelectedEntry(int index)
        {
            _SelectedEntry = index;
        }

        public override void Enter(params object[] args)
        {
            // TOFIX
            //Game.Input.OnButtonPressed += Input_OnButtonPressed;

            base.Enter(args);
        }

        public override void Exit()
        {
            // TOFIX
            //Game.Input.OnButtonPressed -= Input_OnButtonPressed;

            base.Exit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            Vector2 position = new Vector2((int)(Game.RenderSize.X / 2 - _MenuSize.X / 2), (int)(Game.RenderSize.Y / 2 - _MenuSize.Y / 2));

            spriteBatch.Begin();

            spriteBatch.DrawString(Game.Font, _MenuTitle, position + new Vector2((int)(Game.Font.MeasureString(_MenuTitle).X / 2), (int)(-Game.Font.LineSpacing * 1.5f)), Color.White);

            foreach (MenuEntry m in MenuEntries)
            {
                bool isSelected = (m == MenuEntries[_SelectedEntry]);
                position.Y += (int)(Game.Font.LineSpacing);
                m.Draw(spriteBatch, Game.Font, position, (int)(_MenuSize.X / 2), isSelected);
            }

            spriteBatch.End();
        }
        
        // TOFIX
        /*
        private void Input_OnButtonPressed(object sender, InputEventArgs e)
        {
            if (e.Button == Buttons.DPadUp || e.Button == Buttons.LeftThumbstickUp)
            {
                _SelectedEntry--;

                if (_SelectedEntry < 0)
                    _SelectedEntry = MenuEntries.Count - 1;

                if (MenuEntries[_SelectedEntry].GetType() == typeof(TitleEntry))
                    _SelectedEntry--;

                if (_SelectedEntry < 0)
                    _SelectedEntry = MenuEntries.Count - 1;
            }

            if (e.Button == Buttons.DPadDown || e.Button == Buttons.LeftThumbstickDown)
            {
                _SelectedEntry++;

                if (_SelectedEntry >= MenuEntries.Count)
                    _SelectedEntry = 0;

                if (MenuEntries[_SelectedEntry].GetType() == typeof(TitleEntry))
                    _SelectedEntry++;

                if (_SelectedEntry >= MenuEntries.Count)
                    _SelectedEntry = 0;
            }
            
            /*if (e.Button == Buttons.Back)
            {
                Cancel();
            }*/

            /*if (e.Button == Buttons.A)
            {
                Select(_SelectedEntry);
            }
        }*/

        public void Cancel()
        {
            Game.ScreenMachine.ClearPopup();
        }

        public void Select(int index)
        {
            MenuEntries[index].Select();
        }

    }
}
