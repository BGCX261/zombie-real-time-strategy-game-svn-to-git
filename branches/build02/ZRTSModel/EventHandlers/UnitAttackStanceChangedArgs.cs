using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.EventHandlers
{
	public class UnitAttackStanceChangedArgs
	{
		public UnitComponent unit;
		public UnitComponent.UnitAttackStance newAttackStance;
		public UnitComponent.UnitAttackStance oldAttackStance;

		public UnitAttackStanceChangedArgs(UnitComponent unit, UnitComponent.UnitAttackStance newAttackStance, UnitComponent.UnitAttackStance oldAttackStance)
		{
			this.unit = unit;
			this.newAttackStance = newAttackStance;
			this.oldAttackStance = oldAttackStance;
		}
	}
}
