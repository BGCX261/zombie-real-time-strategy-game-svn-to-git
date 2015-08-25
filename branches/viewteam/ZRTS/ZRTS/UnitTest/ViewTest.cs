using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ZRTS.UnitTest
{
    /// <summary>
    /// Unit Testing for View object
    /// </summary>
    [TestFixture]
    class ViewTest
    {
        /// <summary>
        /// Testing if View convert mouse(x,y) location to the correct game location
        /// </summary>
        [Test]
        public void testConvertScreenLocToGameLoc()
        {
           View test = new View(50, 50);
            int width = 20;
            int height = 20;
            Assert.AreEqual(0, test.convertScreenLocToGameLoc(0, 0).X, "Game X location  should be 0.");
            Assert.AreEqual(0, test.convertScreenLocToGameLoc(0, 0).Y, "Game Y location should be 0");
            Assert.AreEqual(20 / width, test.convertScreenLocToGameLoc(20, 20).X, "Game X location should be " + 20 / width);
            Assert.AreEqual(20 / height, test.convertScreenLocToGameLoc(20, 20).Y, "Game Y location should be " + 20 / height);
            Assert.AreEqual(300 / width, test.convertScreenLocToGameLoc(300, 100).X, "Game X location should be " + 300 / width);
            Assert.AreEqual(100 / height, test.convertScreenLocToGameLoc(300, 100).Y, "Game Y location should be " + 100 / height);
            
        }
    }
}
