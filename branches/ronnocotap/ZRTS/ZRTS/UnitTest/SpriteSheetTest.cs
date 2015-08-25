using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

namespace ZRTS.UnitTest
{
    /// <summary>
    /// Unit Testing for SpriteSheet object
    /// Author: Nattapol Kemavaha
    /// </summary>
    [TestFixture]
    class SpriteSheetTest
    {

        /// <summary>
        /// Testing if SpriteSheet has created correct size of array representing each frame.
        /// </summary>
        [Test]
        public void testCreateStructureWithCorrectSize()
        {
            //GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            SpriteSheet testSheet = new SpriteSheet(50, 50);//new SpriteSheet(new Texture2D(graphics.GraphicsDevice,50,50), new SpriteBatch(GraphicsDevice), 50, 50);
            //Assert.IsNotNull(testSheet);
            testSheet.createStructure(500/testSheet.frameDimX, 2000/testSheet.frameDimY);
            Assert.AreEqual(10, testSheet.Location.GetLength(0));
            Assert.AreEqual(40, testSheet.Location.GetLength(1));
        }

        /// <summary>
        /// Testing if each frame's location of the spritesheet is correctly defined
        /// </summary>
        [Test]
        public void testCorrectFrames()
        {
            SpriteSheet testSheet = new SpriteSheet(50, 50);
            int widthTotalImage = 500;
            int heightTotalImage = 2000;
            testSheet.createStructure(widthTotalImage / testSheet.frameDimX, heightTotalImage / testSheet.frameDimY);

            for (int i = 0; i < testSheet.Location.GetLength(0); ++i)
            {
                for (int j = 0; j < testSheet.Location.GetLength(1); ++j)
                {
                    Assert.AreEqual(testSheet.frameDimX * i, testSheet.getFrameLocationOnSheetAt(i, j).X, "At Col=" + i );
                    Assert.AreEqual(testSheet.frameDimY * j, testSheet.getFrameLocationOnSheetAt(i, j).Y, "At Row=" + j );
                }
            }
        }

        /// <summary>
        /// Testing correct construction of array if there is only one frame of image in the sprite
        /// </summary>
        [Test]
        public void testOneFrameInSprite()
        {
            SpriteSheet testSheet = new SpriteSheet(20, 20);
            testSheet.createStructure(1, 1);
            Assert.AreEqual(1, testSheet.Location.GetLength(0));
            Assert.AreEqual(1, testSheet.Location.GetLength(1));
            Assert.AreEqual(testSheet.frameDimX * 0, testSheet.getFrameLocationOnSheetAt(0, 0).X, "At Col = 0");
            Assert.AreEqual(testSheet.frameDimY * 0, testSheet.getFrameLocationOnSheetAt(0, 0).Y, "At Row = 0");
        }
    }
}
