using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameModel;

namespace ZRTSModel.EventHandlers
{
    public class UnitMovedEventArgs : EventArgs
    {
        public UnitComponent Unit;
        public PointF OldPoint;
        public PointF NewPoint;
    }
}
