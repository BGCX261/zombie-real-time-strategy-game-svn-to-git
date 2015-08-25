using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
	public class BuildingActionQueue : ModelComponent
	{
		public void Work()
		{
			if (GetChildren().Count != 0)
			{
				BuildingAction action = GetChildren()[0] as BuildingAction;
				if (action.Work())
				{
					RemoveChild(action);
					//((UnitComponent)Parent).State = UnitComponent.UnitState.IDLE;
				}
			}

		}
		public override void AddChild(ModelComponent child)
		{
			if (child is BuildingAction)
				base.AddChild(child);
		}
		public override void Accept(ModelComponentVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
