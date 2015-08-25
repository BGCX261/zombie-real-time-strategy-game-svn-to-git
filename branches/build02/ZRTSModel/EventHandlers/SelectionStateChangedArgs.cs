using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class SelectionStateChangedArgs : EventArgs
    {
        private List<ModelComponent> selectedEntities;

        public List<ModelComponent> SelectedEntities
        {
            get { return selectedEntities; }
            set { selectedEntities = value; }
        }


    }
}
