using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView.MapViewVisitors
{
    public class DrawTileVisitor : NoOpModelComponentVisitor
    {
        private Texture2D texture;
        private Rectangle drawRectangle;
        private SpriteBatch spriteBatch;
        public DrawTileVisitor(SpriteBatch spriteBatch, Texture2D texture, Rectangle drawRectangle)
        {
            this.texture = texture;
            this.drawRectangle = drawRectangle;
            this.spriteBatch = spriteBatch;
        }
        public override void Visit(Grass grass)
        {
            // TODO: add additinal logic for using different grass texture
            spriteBatch.Draw(texture, drawRectangle, new Rectangle(GameConfig.TILE_GRASS * GameConfig.TILE_DIM, GameConfig.TILE_START_Y, GameConfig.TILE_DIM, GameConfig.TILE_DIM), Color.White);
        }

        public override void Visit(Mountain mountain)
        {
            spriteBatch.Draw(texture, drawRectangle, new Rectangle(GameConfig.TILE_TREE* GameConfig.TILE_DIM, GameConfig.TILE_START_Y, GameConfig.TILE_DIM, GameConfig.TILE_DIM), Color.White);
        }

        public override void Visit(Sand sand)
        {
            spriteBatch.Draw(texture, drawRectangle, new Rectangle(GameConfig.TILE_SAND * GameConfig.TILE_DIM, GameConfig.TILE_START_Y, GameConfig.TILE_DIM, GameConfig.TILE_DIM), Color.White);
        }
    }
}
