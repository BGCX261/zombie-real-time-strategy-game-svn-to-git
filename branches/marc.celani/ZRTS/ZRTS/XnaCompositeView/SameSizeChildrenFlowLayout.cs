using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// This class lays its children out in a flow, using the assumption that all children are the same size.  This optimization
    /// makes the DoLayout() function faster, and is the primary use case in this game.
    /// </summary>
    public class SameSizeChildrenFlowLayout : XnaUIComponent
    {
        public SameSizeChildrenFlowLayout(Game game)
            : base(game)
        {
        }

        public int SpacingBetween = 0;

        protected override void onDraw(XnaDrawArgs e)
        {
			Texture2D pixel = new Texture2D(e.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
            //e.SpriteBatch.Draw(pixel, e.Location, new Rectangle(0, 0, 1, 1), Color.Blue);
        }

        public override void DoLayout()
        {
            int count = GetChildren().Count;
            if (count > 0)
            {
                // TODO: how to set space between icons
                //System.Console.Out.WriteLine(count);
                XnaUIComponent firstComponent = GetChildren()[0];
                int boxWidth = firstComponent.DrawBox.Width;
                int boxHeight = firstComponent.DrawBox.Height;

                int totalAllowableWidthAfterFirstElementInRow = DrawBox.Width - boxWidth;
                int numberOfElementInRow = 1 + totalAllowableWidthAfterFirstElementInRow / (boxWidth + SpacingBetween);
                int current = 0;
                foreach (XnaUIComponent component in GetChildren())
                {
                    int x = (current % numberOfElementInRow) * (boxWidth + SpacingBetween);
                    int y = (current / numberOfElementInRow) * (boxHeight + SpacingBetween);

                    component.DrawBox = new Rectangle(x, y, boxWidth, boxHeight);

                    current++;
                }
            }
        }
    }
}
