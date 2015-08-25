using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// XnaDrawArgs
    /// 
    /// This class will hold data about spriteBatch, Location and current game time.
    /// </summary>
    public class XnaDrawArgs
    {
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch;
        public Microsoft.Xna.Framework.Rectangle Location;
        public Microsoft.Xna.Framework.GameTime gameTime;
    }
}
