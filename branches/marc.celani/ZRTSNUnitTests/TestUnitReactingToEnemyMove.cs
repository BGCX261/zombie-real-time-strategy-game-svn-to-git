using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSModel.GameModel;
using ZRTSModel.Factories;
using NUnit.Framework;
namespace ZRTSNUnitTests
{
	/// <summary>
	/// This class contains tests for testing if UnitComponents correctly react to an Enemy UnitComponent being 
	/// placed into a CellComponent within a UnitComponent's visibility range.
	/// </summary>
	[TestFixture]
	class TestUnitReactingToEnemyMove
	{
		 GameModel model;

		public TestUnitReactingToEnemyMove()
		{
		}

		private void setupModel()
		{
			model = new GameModel();
			ScenarioComponent scenario = new ScenarioComponent(20, 20); // 20 x 20 Gameworld.

			// Add grass cells at each cell.
			ZRTSModel.Map map = scenario.GetGameWorld().GetMap();
			for (int i = 0; i < map.GetWidth(); i++)
			{
				for (int j = 0; j < map.GetHeight(); j++)
				{
					CellComponent cell = new CellComponent();
					cell.AddChild(new Sand());
					cell.X = i;
					cell.Y = j;
					map.AddChild(cell);
				}
			}
			model.AddChild(scenario);

			//Create two players and set them to be enemies.
			model.GetScenario().GetGameWorld().GetPlayerList().AddChild(new PlayerComponent());
			model.GetScenario().GetGameWorld().GetPlayerList().AddChild(new PlayerComponent());
			PlayerComponent player1 = (PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0];
			PlayerComponent player2 = (PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[1];
			player1.EnemyList.Add(player2);
			player2.EnemyList.Add(player1);


		}

		/// <summary>
		/// This test checks that if an Enemy UnitComponent is added to the game within the Visibility range of an UnitComponent (and that
		/// UnitComponents attack stance is set to Aggressive) that the UnitComponent is given an AttackAction with the Enemy UnitComponent as 
		/// the Target.
		/// </summary>
		[Test]
		public void TestUnitReactingToEnemyAddedWithinRange()
		{
			setupModel();
			// Add a unit to each player.
			UnitList list = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0]).GetUnitList();
			UnitList list2 = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[1]).GetUnitList();

			// Create a unit for Player 1
			UnitComponent unit1 = new UnitComponent();
			list.AddChild(unit1);
			unit1.PointLocation = new PointF(0.5f, 0.5f);
			unit1.AttackStance = UnitComponent.UnitAttackStance.Aggressive;

			// Create a unit for Player 2
			UnitComponent unit2 = new UnitComponent();
			list2.AddChild(unit2);
			unit2.PointLocation = new PointF(1.5f, 1.5f);

			bool output = false;
			// Check to see if unit1 noticed unit2 being added next to it and correctly gave itself an attack action.
			if (unit1.GetActionQueue().GetChildren().Count > 0)
			{
				if (unit1.GetActionQueue().GetChildren()[0] is AttackAction)
				{
					AttackAction action = (AttackAction)unit1.GetActionQueue().GetChildren()[0];

					output =  action.Target == unit2;
				}
			}

			// unit1 should react to unit2 being added next to it.
			Assert.IsTrue(output);
		}

		/// <summary>
		/// Tests that a UnitComponent doesn't react to an Enemy UnitComponent being added to a CellComponent outside
		/// of it's Visibility range.
		/// </summary>
		[Test]
		public void TestUnitNotReactingToEnemyAddedOutsideRange()
		{
			setupModel();
			// Add a unit to each player.
			UnitList list = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0]).GetUnitList();
			UnitList list2 = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[1]).GetUnitList();

			// Create a Unit for Player 1
			UnitComponent unit1 = new UnitComponent();
			list.AddChild(unit1);
			unit1.PointLocation = new PointF(0.5f, 0.5f);
			unit1.AttackStance = UnitComponent.UnitAttackStance.Aggressive;
			unit1.VisibilityRange = 4.0f;

			// Create a Unit for Player 2
			UnitComponent unit2 = new UnitComponent();
			list2.AddChild(unit2);
			unit2.PointLocation = new PointF(9.5f, 9.5f); // outisde unit1's visibility range (4.0f).

			bool output = false;
			// Check to see if unit1 noticed unit2 being added.
			if (unit1.GetActionQueue().GetChildren().Count > 0)
			{
				if (unit1.GetActionQueue().GetChildren()[0] is AttackAction)
				{
					AttackAction action = (AttackAction)unit1.GetActionQueue().GetChildren()[0];

					output = action.Target == unit2;
				}
			}

			Assert.IsFalse(output);
		}

		/// <summary>
		/// Tests that a UnitComponent reacts to an Enemy UnitComponent moving onto a CellComponent within  
		/// the UnitComponent's visibility range from a CellComponent that is outside of the UnitComponent's 
		/// visibility range.
		/// </summary>
		[Test]
		public void TestUnitReactingToEnemyMovingWithinRange()
		{
			setupModel();
			// Add a unit to each player.
			UnitList list = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0]).GetUnitList();
			UnitList list2 = ((PlayerComponent)model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[1]).GetUnitList();

			// Create a Unit for Player 1
			UnitComponent unit1 = new UnitComponent();
			list.AddChild(unit1);
			unit1.PointLocation = new PointF(0.5f, 0.5f);
			unit1.AttackStance = UnitComponent.UnitAttackStance.Aggressive;
			unit1.VisibilityRange = 4.0f;

			// Create a Unit for Player 2
			UnitComponent unit2 = new UnitComponent();
			list2.AddChild(unit2);
			unit2.PointLocation = new PointF(9.5f, 9.5f); // outisde unit1's visibility range (4.0f).

			// Check to make sure that unit1 did not notice unit2 being added.
			bool output = false;
			if (unit1.GetActionQueue().GetChildren().Count > 0)
			{
				if (unit1.GetActionQueue().GetChildren()[0] is AttackAction)
				{
					AttackAction action = (AttackAction)unit1.GetActionQueue().GetChildren()[0];

					output = action.Target == unit2;
				}
			}
			Assert.IsFalse(output);

			// Have unit2 move into unit1's visibility range.
			MoveAction move = new MoveAction(2.0f, 2.0f, model.GetScenario().GetGameWorld().GetMap(), unit2);

			//Have unit2 move until the move action is completed.
			while (!move.Work()) { }

			// Test that unit1 has been given an AttackAction with unit2 as the target.
			output = false;
			if (unit1.GetActionQueue().GetChildren().Count > 0)
			{
				if (unit1.GetActionQueue().GetChildren()[0] is AttackAction)
				{
					AttackAction action = (AttackAction)unit1.GetActionQueue().GetChildren()[0];

					output = action.Target == unit2;
				}
			}
			Assert.IsTrue(output);
		}
	}
}
