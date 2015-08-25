using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;

namespace ZRTSModel
{
    [Serializable()]
    public class BuildingList : ModelComponent
    {
        public event BuildingAddedOrRemovedHandler BuildingAddedEventHandlers;
        public event BuildingAddedOrRemovedHandler BuildingRemovedEventHandlers;

        public override void AddChild(ModelComponent child)
        {
            if (child is Building)
            {
                Building building = child as Building;
                base.AddChild(child);
                if (BuildingAddedEventHandlers != null)
                {
                    BuildingAddedEventArgs e = new BuildingAddedEventArgs();
                    e.Building = building;
                    BuildingAddedEventHandlers(this, e);
                }
            }
        }

        public override void RemoveChild(ModelComponent child)
        {
            if (child is Building)
            {
                Building building = child as Building;
                base.RemoveChild(child);
                if (BuildingRemovedEventHandlers != null)
                {
                    BuildingAddedEventArgs e = new BuildingAddedEventArgs();
                    e.Building = building;
                    BuildingRemovedEventHandlers(this, e);
                }
            }
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
