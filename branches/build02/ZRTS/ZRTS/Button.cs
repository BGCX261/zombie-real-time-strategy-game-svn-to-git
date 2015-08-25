using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameModel;

namespace ZRTS
{
    /// <summary>
    /// Represents a button on the Menu.  Can be clicked with the Mouse and then executes an action or changes
    /// the game state.
    /// </summary>
    public class Button
    {
        private PointF location;    // The top-left corner of the Button
        private float width;
        private float height;
        private string name;    // The text the button displays

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="width">Width of button</param>
        /// <param name="height">Height of button</param>
        /// <param name="name">Text displayed on the button</param>
        public Button(float x, float y, float width, float height, string name)
        {
            location = new PointF(x, y);
            this.width = width;
            this.height = height;
            this.name = name;
        }

        public PointF PointLocation
        {
            get { return location; }
            set { location = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        public float Height
        {
            get { return height; }
            set { height = value; }
        }


        /// <summary>
        /// Checks to see the point (x, y) is contained within the dimensions of the button.
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <returns></returns>
        public bool contains(float x, float y)
        {
            if(x >= location.X 
                    && x < location.X + width
                    && y >= location.Y
                    && y < location.Y + height)
                return true;
            return false;
        }
    }
}
