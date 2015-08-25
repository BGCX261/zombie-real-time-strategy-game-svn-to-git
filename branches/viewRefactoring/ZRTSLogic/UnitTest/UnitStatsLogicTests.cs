using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSLogic;
using ZRTSModel.Player;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSModel.Scenario;

namespace ZRTSLogic.UnitTest
{
	/// <summary>
	/// This class contains NUnit tests of the UnitStatsLogic tests.
	/// </summary>
	[TestFixture]
	class UnitStatsLogicTests
	{
		Unit unit;

		/// <summary>
		/// Test the updateUnit method.
		/// </summary>
		[Test]
		public void testUpdateUnit()
		{
			// Create a unit with 50% health
			unit = new Unit(new Player(0), new UnitStats());
			unit.health = 100;
			unit.stats.maxHealth = 200;
			UnitStatsLogic.updateUnit(unit, 0);

			// Check if unit's buffs are set to 75%
			Assert.IsTrue(unit.attackBuff == 0.75);
			Assert.AreEqual(0.75, unit.speedBuff);
		}

		/// <summary>
		/// Test that a dead units state is update.
		/// </summary>
		[Test]
		public void testDeadUnit()
		{
			// Create a dead unit.
			unit = new Unit(new Player(0), new UnitStats());
			unit.health = 0;
			unit.stats.maxHealth = 200;
			UnitStatsLogic.updateUnit(unit, 0);

			// Check if unit's buffs are set to 50% and that the unit's primary state is set to DEAD.
			Assert.IsTrue(unit.attackBuff == 0.50);
			Assert.AreEqual(0.50, unit.speedBuff);
			Assert.AreEqual(State.PrimaryState.Dead, unit.getState().getPrimaryState());
		}
	}
}
