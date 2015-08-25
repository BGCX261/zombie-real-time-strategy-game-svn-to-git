using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.EventHandlers;

namespace ZRTSModel
{
    /// <summary>
    /// This class represents a collection of UnitComponents.
    /// </summary>
    [Serializable()]
    public class UnitList : ModelComponent
    {
		/// <summary>
		/// This event is fired whenever an UnitComponent is added to the list of children.
		/// </summary>
        public event UnitAddedToPlayerListHandler UnitAddedEvent;
		/// <summary>
		/// This event is fired whenever an UnitComponent is removed from the list of children.
		/// </summary>
        public event UnitRemovedFromPlayerListHandler UnitRemovedEvent;

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

		/// <summary>
		/// Adds a child to the UnitList. Only add's the child if it is a UnitComponent.
		/// Fires off an UnitAddedEvent.
		/// </summary>
		/// <param name="child">The ModelComponent to be added.</param>
        public override void AddChild(ModelComponent child)
        {
            if (child is UnitComponent)
            {
                base.AddChild(child);
                UnitAddedEventArgs args = new UnitAddedEventArgs();
                args.Unit = (UnitComponent) child;
                if (UnitAddedEvent != null)
                {
                    UnitAddedEvent(this, args);
                }
            }
        }

		/// <summary>
		/// Removes a child ModelComponent from the UnitList if it is contained by this. Fires
		/// off an UnitRemovedEvent.
		/// </summary>
		/// <param name="child"></param>
        public override void RemoveChild(ModelComponent child)
        {
            base.RemoveChild(child);
            if (UnitRemovedEvent != null && child is UnitComponent)
            {
                UnitRemovedEventArgs args = new UnitRemovedEventArgs();
                args.Unit = (UnitComponent)child;
                UnitRemovedEvent(this, args);
            }
        }
    }
}
