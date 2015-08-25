using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSModel.Trigger
{
	public class PlayerWinAction : Action
	{
		ScenarioComponent scenario;
		
		public PlayerWinAction(Trigger decorated, ScenarioComponent scenario) : base(decorated)
		{
			this.scenario = scenario;
		}

		public override void  PerformActions()
		{
			((GameModel.GameModel)scenario.Parent).VictoryState = GameModel.GameModel.GameVictoryState.PlayerWin;
		}

		public override void PerformMyAction()
		{
			PerformActions();
		}
	}
}
