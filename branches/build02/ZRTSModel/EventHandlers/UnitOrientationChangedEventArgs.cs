using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.EventHandlers
{
	public class UnitOrientationChangedEventArgs
	{
		public UnitComponent unit;
		public UnitComponent.Orient oldOrientation;
		public UnitComponent.Orient newOrientation;

		public UnitOrientationChangedEventArgs(UnitComponent unit, UnitComponent.Orient oldOrientation, UnitComponent.Orient newOrientation)
		{
			this.unit = unit;
			this.oldOrientation = oldOrientation;
			this.newOrientation = newOrientation;
		}
	}
}
