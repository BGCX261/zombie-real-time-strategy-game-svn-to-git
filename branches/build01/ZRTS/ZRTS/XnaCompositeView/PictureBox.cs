using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// PictureBox
    /// 
    /// This class represents image representation of the user interface component.
    /// </summary>
    public class PictureBox : XnaUIComponent
    {
        private Rectangle sourceRect;

        protected Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        private Color tint = Color.White;

        

        public Color Tint
        {
            get { return tint; }
            set { tint = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="sourceRect">Location on the spritesheet</param>
        public PictureBox(Game game, Rectangle sourceRect)
            : base(game)
        {
            this.sourceRect = sourceRect;
        }

        protected override void onDraw(XnaDrawArgs e)
        {
			Rectangle actualDraw = e.Location;
			
            e.SpriteBatch.Draw(((XnaUITestGame)Game).SpriteSheet, actualDraw, sourceRect, tint);
        }

        public void setPicturebox(Rectangle r)
        {
            sourceRect = r;
        }
    }
}
