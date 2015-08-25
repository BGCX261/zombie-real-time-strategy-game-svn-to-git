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
	/// This class contains NUnit tests for the MoveAction class.
	/// </summary>
	[TestFixture]
	class MoveActionTests
	{
		/// <summary>
		/// Test giving a Unit a MoveAction to a unit.
		/// </summary>
		[Test]
		public void testMove()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			Unit unit = new Unit(scenario.getPlayer(), 200);
			controller.addUnit(unit, 10f, 10f);

			// Create a MoveAction
			MoveAction action = new MoveAction(8f, 8f, scenario.getGameWorld(), unit);

			controller.giveActionCommand(unit, action);

			Assert.AreEqual(scenario.getGameWorld().map.getCell(10, 10), unit.getCell());

			// Run 1000 cycles of the game
			for (int i = 0; i < 1000; i++)
			{
				controller.updateWorld();
			}

			// Test if the unit has ended up in the target cell.
			Assert.AreEqual(scenario.getGameWorld().map.getCell(8, 8), unit.getCell());
		}
	}
}
