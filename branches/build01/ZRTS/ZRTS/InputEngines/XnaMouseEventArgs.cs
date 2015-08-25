using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTS.InputEngines
{
    public enum MouseButton
    {
        Left,
        Right
    }

    /// <summary>
    /// XNAMouseEventArgs
    /// 
    /// Handle mouse I/O
    /// </summary>
    public class XnaMouseEventArgs : EventArgs
    {
        public XnaCompositeView.XnaUIComponent Target;
        public Microsoft.Xna.Framework.Point ClickLocation;
        public bool Handled;
        public bool Bubbled;
        public bool SingleTarget;
        public MouseButton ButtonPressed;
		public long time;

    }

    public delegate void ClickEventHandler(Object sender, XnaMouseEventArgs args);
}
