using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSModel.GameWorld;
using ZRTSModel.Scenario;
using ZRTSLogic;
using ZRTSModel.Entities;

namespace ZRTSLogic.UnitTest
{
	[TestFixture]
	class EntityLocControllerTest
	{
		[Test]
		public void testAddUnit()
		{
			/*
			Scenario testScenario = new Scenario(20, 20);
			Unit testUnit = new Unit(testScenario.getPlayer(), new UnitStats());
			EntityLocController locController = new EntityLocController(testScenario);

			locController.addEntity(testUnit, 10, 10);

			Assert.IsTrue(testScenario.getPlayer().hasEntity(testUnit));
			Assert.AreEqual(testUnit.getCell(), testScenario.getGameWorld().map.getCell(10, 10));
			 */
		}
	}
}
