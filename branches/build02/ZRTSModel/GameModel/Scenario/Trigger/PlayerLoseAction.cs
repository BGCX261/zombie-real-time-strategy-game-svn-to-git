using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.Trigger
{
	public class PlayerLoseAction : Action
	{
		ScenarioComponent scenario;
		
		public PlayerLoseAction(Trigger decorated, ScenarioComponent scenario) : base(decorated)
		{
			this.scenario = scenario;
		}

		public override void  PerformActions()
		{
			((GameModel.GameModel)scenario.Parent).VictoryState = GameModel.GameModel.GameVictoryState.PlayerLost;
		}

		public override void PerformMyAction()
		{
			PerformActions();
		}

	}
}
