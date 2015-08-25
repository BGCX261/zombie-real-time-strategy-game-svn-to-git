using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.EventHandlers
{
	public class UnitListChangedEventArgs
	{
		UnitList unitList;

		public UnitListChangedEventArgs(UnitList unitList)
		{
			this.unitList = unitList;
		}
	}
}
