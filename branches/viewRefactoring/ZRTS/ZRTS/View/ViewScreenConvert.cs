using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTS.View
{
    public interface ViewScreenConvert
    {

        Microsoft.Xna.Framework.Vector2 convertScreenLocToGameLoc(int x, int y);
    }
}
