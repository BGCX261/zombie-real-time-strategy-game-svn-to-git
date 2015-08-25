using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView
{
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
