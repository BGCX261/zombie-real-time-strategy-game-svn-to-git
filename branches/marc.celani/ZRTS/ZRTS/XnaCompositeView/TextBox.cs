using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{
    public class TextBox : XnaUIComponent
    {
        private string text = "";
        private string alignment = "left";
		Color color = Color.White;

        public string Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public TextBox(Game game)
            : base(game)
        { }

        public TextBox(Game game, string text, string alignment)
            : base(game)
        {
            this.text = text;
            this.alignment = alignment;
        }

		public TextBox(Game game, string text, string alignment, Color color)
			: base(game)
		{
			this.text = text;
			this.alignment = alignment;
			this.color = color;
		}
        protected override void onDraw(XnaDrawArgs e)
        {
            SpriteFont font = ((XnaUITestGame)Game).Font;
            Vector2 destPos = new Vector2((float)e.Location.X, (float)e.Location.Y);
            if (alignment.Equals("right"))
            {
                Vector2 desiredSize = font.MeasureString(text);
                destPos.X += (float)e.Location.Width - desiredSize.X;
            }
            e.SpriteBatch.DrawString(font, text, destPos, color);
        }
    }
}
