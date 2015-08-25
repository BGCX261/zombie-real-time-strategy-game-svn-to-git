using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSModel.Player;
using ZRTSModel.Scenario;
using ZRTSLogic.Action;
using ZRTSLogic;

namespace ZRTSLogic.UnitTest
{
	/// <summary>
	/// This class contains NUnit tests pertaining to the visibility map.
	/// </summary>
	[TestFixture]
	class VisibilityMapLogicTests
	{
		/// <summary>
		/// Test that a map with no player units or buildings is unexplored.
		/// </summary>
		[Test]
		public void testEmptyMap()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			// Test that all of the cells in a map with no player units in it are unexplored.
			for (int i = 0; i < 20; i++)
			{
				for (int j = 0; j < 20; j++)
				{
					Assert.IsFalse(scenario.getGameWorld().map.getCell(i, j).explored);
				}
			}
		}

		/// <summary>
		/// Test than inserting a unit updates the visibility map.
		/// </summary>
		[Test]
		public void testMapWithUnit()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);
			Unit unit = new Unit(scenario.getPlayer(), new UnitStats());
			controller.addUnit(unit, 0, 0);

			// Test that all of the cells within the units visibility range have been explored.
			for (int i = 0; i < unit.stats.visibilityRange; i++)
			{
				for (int j = 0; j < unit.stats.visibilityRange; j++)
				{
					Assert.IsTrue(scenario.getGameWorld().map.getCell(i, j).explored);
				}
			}
		}

		/// <summary>
		/// Test that the VisibilityMap is updated when a unit moves.
		/// </summary>
		[Test]
		public void testMapWithUnitMove()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);
			Unit unit = new Unit(scenario.getPlayer(), new UnitStats());
			controller.addUnit(unit, 6, 6);

			MoveAction move = new MoveAction(10, 10, scenario.getGameWorld(), unit);

			controller.giveActionCommand(unit, move);

			// Update the world so that the unit moves.
			for (int i = 0; i < 1000; i++)
			{
				controller.updateWorld();
			}


			// Test that all of the cells within the units visibility range have been explored.
			for (int i = (int)unit.x - (int)unit.stats.visibilityRange; i < (int)unit.x + (int)unit.stats.visibilityRange; i++)
			{
				for (int j = (int)unit.y - (int)unit.stats.visibilityRange; j < (int)unit.y + (int)unit.stats.visibilityRange; j++)
				{
					Assert.IsTrue(scenario.getGameWorld().map.getCell(i, j).explored);
				}
			}
		}

	}
}
