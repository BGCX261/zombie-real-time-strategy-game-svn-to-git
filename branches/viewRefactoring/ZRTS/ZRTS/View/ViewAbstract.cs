using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZRTSModel.Entities;

namespace ZRTS.View
{
    /// <summary>
    /// ViewAbstract
    /// 
    /// This abstract class will provide common methods and properties for all inherited classes.
    /// </summary>
    public abstract class ViewAbstract
    {
        public int width, height;
        protected SpriteSheet sheet;


        /// <summary>
        /// Load spriteSheet
        /// </summary>
        public virtual void loadSheet(SpriteSheet sheet)
        {
            this.sheet = sheet;
        }

        /// <summary>
        /// Draw
        /// </summary>
        public virtual void Draw()
        {

        }

        /// <summary>
        /// Translate X game location to X screen location
        /// </summary>
        /// <param name="x">X-Game location</param>
        /// <returns>X Screen location </returns>
        protected virtual float translateXScreen(float x)
        {
            return x * GameConfig.TILE_WIDTH;
        }

        /// <summary>
        ///  Translate Y game location to Y screen location
        /// </summary>
        /// <param name="y">Y-Game location</param>
        /// <returns>Y screen location</returns>
        protected virtual float translateYScreen(float y)
        {
            return y * GameConfig.TILE_HEIGHT;
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void update()
        {

        }
    }
}
