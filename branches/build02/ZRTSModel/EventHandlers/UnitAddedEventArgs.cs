using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class UnitAddedEventArgs : EventArgs
    {
        private UnitComponent unit;

        public UnitComponent Unit
        {
            get { return unit; }
            set { unit = value; }
        }
    }
}
