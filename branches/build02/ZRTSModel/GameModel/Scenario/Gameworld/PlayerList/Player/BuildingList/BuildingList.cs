using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;

namespace ZRTSModel
{
	/// <summary>
	/// This class represents a list of BuildingComponents.
	/// </summary>
    [Serializable()]
    public class BuildingList : ModelComponent
    {
		/// <summary>
		/// This event is fired whenever a BuildingComponent is added to this object.
		/// </summary>
        public event BuildingAddedOrRemovedHandler BuildingAddedEventHandlers;
		/// <summary>
		/// This event is dired whenever a BuildingComponent is removed from this object.
		/// </summary>
        public event BuildingAddedOrRemovedHandler BuildingRemovedEventHandlers;

		/// <summary>
		/// Adds a BuildingComponent to this object. Fires off a BuildingAdded event.
		/// </summary>
		/// <param name="child">The BuildingComponent to be added.</param>
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

		/// <summary>
		/// Removed a BuildingComponent from this object. Fires off a BuildingRemoved event.
		/// </summary>
		/// <param name="child">The BuildingComponent to be removed.</param>
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
