using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class BuildingAddedEventArgs : EventArgs
    {
        private Building building;

        public Building Building
        {
            get { return building; }
            set { building = value; }
        }
    }
}
