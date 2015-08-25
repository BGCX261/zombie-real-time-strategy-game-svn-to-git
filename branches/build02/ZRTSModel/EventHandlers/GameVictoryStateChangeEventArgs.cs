using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
	public class GameVictoryStateChangeEventArgs
	{
		public GameModel.GameModel.GameVictoryState victoryState;

		public GameVictoryStateChangeEventArgs(GameModel.GameModel.GameVictoryState victoryState)
		{
			this.victoryState = victoryState;
		}
	}
}
