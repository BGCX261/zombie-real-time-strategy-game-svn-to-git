using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// TextBox
    /// 
    /// This class represents textbox component being used for the user menu.
    /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        public TextBox(Game game)
            : base(game)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="text">Text/string to be displayed</param>
        /// <param name="alignment">Type of alignment</param>
        public TextBox(Game game, string text, string alignment)
            : base(game)
        {
            this.text = text;
            this.alignment = alignment;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="text">Text/string to be displayed</param>
        /// <param name="alignment">Type of alignment</param>
        /// <param name="color">Color of the text</param>
		public TextBox(Game game, string text, string alignment, Color color)
			: base(game)
		{
			this.text = text;
			this.alignment = alignment;
			this.color = color;
		}

        /// <summary>
        /// upon drawing this textbox on the screen event
        /// </summary>
        /// <param name="e"></param>
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
