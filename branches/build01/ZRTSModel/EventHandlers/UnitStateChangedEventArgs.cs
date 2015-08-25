using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.EventHandlers
{
	public class UnitStateChangedEventArgs
	{
		public UnitComponent unit;
		public UnitComponent.UnitState oldState;
		public UnitComponent.UnitState newState;

		public UnitStateChangedEventArgs(UnitComponent unit, UnitComponent.UnitState oldState, UnitComponent.UnitState newState)
		{
			this.unit = unit;
			this.oldState = oldState;
			this.newState = newState;
		}
	}
}
