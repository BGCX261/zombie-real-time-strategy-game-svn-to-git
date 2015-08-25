using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;

namespace ZRTSModel.GameModel
{
    public class GameModel : ModelComponent
    {
		public GameVictoryStateChangedHandler VictoryStateChangedHandler;

        private PlayerComponent playerInContext;

        /// <summary>
        /// A reference to the player that is actually playing the game.
        /// </summary>
        public PlayerComponent PlayerInContext
        {
            get { return playerInContext; }
            set { playerInContext = value; }
        }

        public GameModel()
        {
            AddChild(new SelectionState());
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ScenarioComponent GetScenario()
        {
            ScenarioComponent scenario = null;
            foreach (ModelComponent component in GetChildren())
            {
                if (component is ScenarioComponent)
                {
                    scenario = (ScenarioComponent)component;
                    break;
                }
            }
            return scenario;
        }

        public SelectionState GetSelectionState()
        {
            SelectionState selectionState = null;
            foreach (ModelComponent component in GetChildren())
            {
                if (component is SelectionState)
                {
                    selectionState = (SelectionState)component;
                    break;
                }
            }
            return selectionState;
        }

		public enum GameVictoryState { Undecided, PlayerWin, PlayerLost };
		private GameVictoryState victoryState = GameVictoryState.Undecided;

		public GameVictoryState VictoryState
		{
			get { return this.victoryState; }
			set
			{
				Console.WriteLine("Victory State Changed: " + value);
				this.victoryState = value;
				if (VictoryStateChangedHandler != null)
				{
					VictoryStateChangedHandler(this, new GameVictoryStateChangeEventArgs(this.victoryState));
				}
			}
		}
    }
}
