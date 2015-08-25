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
	/// This class contains NUnit tests of the ActionController class.
	/// </summary>
	[TestFixture]
	class ActionControllerTests
	{
		/// <summary>
		/// Test giving a Unit a command.
		/// </summary>
		[Test]
		public void testGiveCommand()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			Unit unit = new Unit(scenario.getPlayer(), 200);
			controller.addUnit(unit, 10f, 10f);

			// Create a MoveAction
			MoveAction action = new MoveAction(8f, 8f, scenario.getGameWorld(), unit);

			// Test if the ActionController returns true when it gives the command.
			Assert.IsTrue(controller.giveActionCommand(unit, action));

			// Test if the MoveAction is actually on the unit's action queue.
			Assert.IsTrue(unit.getActionQueue()[0] == action);

			// Create a second MoveAction
			MoveAction action2 = new MoveAction(7f, 7f, scenario.getGameWorld(), unit);

			// Test if the ActionController interrupts the current MoveAction and replaces it with the new one.
			Assert.IsTrue(controller.giveActionCommand(unit, action2));
			Assert.IsTrue(unit.getActionQueue()[0] == action2);
			Assert.IsFalse(unit.getActionQueue().Contains(action));
		}

		/// <summary>
		/// Test inserting a command.
		/// </summary>
		[Test]
		public void testInsertCommand()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			Unit unit = new Unit(scenario.getPlayer(), 200);
			controller.addUnit(unit, 10f, 10f);

			// Create a MoveAction
			MoveAction action = new MoveAction(8f, 8f, scenario.getGameWorld(), unit);

			// Test if the ActionController returns true when it gives the command.
			Assert.IsTrue(controller.giveActionCommand(unit, action));

			// Test if the MoveAction is actually on the unit's action queue.
			Assert.IsTrue(unit.getActionQueue()[0] == action);

			// Create a second MoveAction
			MoveAction action2 = new MoveAction(7f, 7f, scenario.getGameWorld(), unit);

			// Insert the second MoveAction to the beginning of the units action queue.
			ActionController.insertIntoActionQueue(unit, action2);
			Assert.IsTrue(unit.getActionQueue()[0] == action2);
			Assert.IsTrue(unit.getActionQueue()[1] == action);
		}

	}
}
