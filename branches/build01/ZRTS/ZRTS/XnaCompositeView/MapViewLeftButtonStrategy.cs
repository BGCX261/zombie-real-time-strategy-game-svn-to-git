using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// This class represents the minimap using for user menu.
    /// </summary>
    public interface MapViewLeftButtonStrategy
    {
        void HandleMouseInput(bool leftButtonPressed, bool rightButtonPressed, Point mouseLocation);
        void CancelProgress();
    }
}
