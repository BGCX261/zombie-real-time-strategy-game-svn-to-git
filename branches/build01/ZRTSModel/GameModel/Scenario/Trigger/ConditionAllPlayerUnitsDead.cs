using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Trigger;
using ZRTSModel;

namespace ZRTSModel.Trigger
{
	public class ConditionAllPlayerUnitsDead : Condition
	{
		PlayerComponent player;
		bool alldead = false;
		UnitList unitList;

		public ConditionAllPlayerUnitsDead(Trigger decorated, PlayerComponent player) : base(decorated)
		{
			this.player = player;
			this.unitList = player.GetUnitList();
		}

		public override bool CheckMyCondition()
		{
			//Console.WriteLine(unitList.GetChildren().Count);
			if (alldead || this.unitList.GetChildren().Count == 0)
			{
				alldead = true;
			}

			return alldead;
		}
	}
}
