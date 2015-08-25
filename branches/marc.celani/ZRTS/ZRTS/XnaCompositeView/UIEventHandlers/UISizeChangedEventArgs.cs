using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView.UIEventHandlers
{
    public class UISizeChangedEventArgs : EventArgs
    {
        private Rectangle drawBox;

        public Rectangle DrawBox
        {
            get { return drawBox; }
            set { drawBox = value; }
        }
    }
}
