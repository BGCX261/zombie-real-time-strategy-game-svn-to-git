using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSModel.Scenario;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using ZRTSLogic;

namespace ZRTSLogic.UnitTest
{
	/// <summary>
	/// This class will contain tests for the EntityLocController with governs inserting and removing entities as well as updating
	/// a Unit's Cell location when a Unit moves.
	/// </summary>
	[TestFixture]
	class EntityLocControllerTests
	{
		[Test]
		public void testAdd()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			// Test a scuccessful insert
			Unit unit = new Unit(scenario.getPlayer(), 200);
			Assert.IsTrue(controller.addUnit(unit, 10f, 10f));

			// Test if GameWorld contains the unit
			Assert.IsTrue(controller.gameWorld.getUnits().Contains(unit));

			// Test if the Player has the unit.
			Assert.IsTrue(scenario.getPlayer().hasEntity(unit));

			// Test an invalid insert
			Unit unit2 = new Unit(scenario.getPlayer(), 200);

			// Test for inerting into an already occupied location.
			Assert.IsFalse(controller.addUnit(unit2, 10f, 10f));

			// Test for inserting outside the boundaries of the map.
			Assert.IsFalse(controller.addUnit(unit2, 20f, 20f));
		}

		[Test]
		public void testRemove()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			// Test a scuccessful insert
			Unit unit = new Unit(scenario.getPlayer(), 200);
			Assert.IsTrue(controller.addUnit(unit, 10f, 10f));

			//Test if GameWorld contains the unit
			Assert.IsTrue(controller.gameWorld.getUnits().Contains(unit));

			// Test if the player has the unit
			Assert.IsTrue(scenario.getPlayer().hasEntity(unit));

			// Test a successful remove
			controller.removeEntity(unit);
			Assert.IsFalse(controller.gameWorld.getUnits().Contains(unit));
			Assert.IsFalse(scenario.getPlayer().hasEntity(unit));
		}

		[Test]
		public void testUpdateUnitLocation()
		{
			Scenario scenario = new Scenario(20, 20);
			Controller controller = new Controller(scenario);

			// Test a scuccessful insert
			Unit unit = new Unit(scenario.getPlayer(), 200);
			Assert.IsTrue(controller.addUnit(unit, 10f, 10f));
			Assert.IsTrue(controller.gameWorld.getUnits().Contains(unit));
			Assert.IsTrue(scenario.getPlayer().hasEntity(unit));

			// Move unit to a point in Cell(0,0)
			unit.x = 0.5f;
			unit.y = 0.5f;

			// Test that the Unit is pointed to by the Cell at (10,10)
			Assert.IsTrue(scenario.getGameWorld().map.getCell(10, 10).getUnit() == unit);

			// Update the units location
			controller.locController.updateUnitLocation(unit);

			// Test if the unit is now pointed to by the Cell at (0,0)
			Assert.IsTrue(scenario.getGameWorld().map.getCell(0, 0).getUnit() == unit);

			Assert.IsFalse(scenario.getGameWorld().map.getCell(10, 10).getUnit() == unit);
		}
	}
}
