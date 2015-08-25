using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ZRTS.XnaCompositeView
{
    public class TestUIComponent : XnaUIComponent
    {
        private Color color;

        public TestUIComponent(Game game, Color color)
            : base(game)
        {
            this.color = color;
        }

        protected override void onDraw(XnaDrawArgs e)
        {
			Texture2D pixel = new Texture2D(e.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
            e.SpriteBatch.Draw(pixel, e.Location, new Rectangle(0, 0, 1, 1), color);
        }
    }
}
