using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NifuuLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.StateMachine.ScreenMachine
{
    public class TitleEntry : MenuEntry
    {
        public TitleEntry(string Title)
        {
            Center = Title;
        }
    }

    public class MenuEntry
    {
        public string Left { get; set; }
        public string Center { get; set; }
        public string Right { get; set; }

        public event EventHandler OnSelected;

        public MenuEntry()
        {

        }

        public void Select()
        {
            OnSelected.Raise(this, EventArgs.Empty);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, int offset, bool isSelected)
        {
            Color color = isSelected ? Color.Yellow : Color.White;

            Vector2 origin;
            if (Left != null)
            {
                origin = new Vector2(0, (int)(font.LineSpacing / 2));
                spriteBatch.DrawString(font, Left, position, color, 0, origin, 1, SpriteEffects.None, 0);
            }
            if (Center != null)
            {
                origin = new Vector2((int)(font.MeasureString(Center).X / 2), (int)(font.LineSpacing / 2));
                spriteBatch.DrawString(font, Center, position + new Vector2(offset, 0), color, 0, origin, 1, SpriteEffects.None, 0);
            }
            if (Right != null)
            {
                origin = new Vector2((int)(font.MeasureString(Right).X), (int)(font.LineSpacing / 2));
                spriteBatch.DrawString(font, Right, position + new Vector2(offset * 2, 0), color, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
