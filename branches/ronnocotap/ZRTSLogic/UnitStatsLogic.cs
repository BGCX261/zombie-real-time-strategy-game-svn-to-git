using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

namespace ZRTSLogic
{
	/// <summary>
	/// This class wil contain methods that will update the temporary stats of a Unit (attackBuff, speedBuff, ect.)
	/// </summary>
	public class UnitStatsLogic
	{
		/// <summary>
		/// Will update the temporary stats of the Unit.
		/// </summary>
		/// <param name="unit"></param>
		public static void updateUnit(Unit unit, long curTick)
		{
			updateAttackBuff(unit);
			updateSpeedBuff(unit);
			isDead(unit, curTick);
		}

		private static void updateAttackBuff(Unit unit)
		{
			/* The attackBuff is affected by a unit's health. The attack buff may be reduced to 50% if the unit loses all of it's health. */
			double baseAttack = 0.5 + 0.5 * unit.health / unit.stats.maxHealth;
			unit.attackBuff = baseAttack;
		}

		private static void updateSpeedBuff(Unit unit)
		{
			double baseSpeed = 0.5 + 0.5 * unit.health / unit.stats.maxHealth;
			unit.speedBuff = baseSpeed;
		}

		private static bool isDead(Entity entity, long curTick)
		{
			if (entity.health <= 0 && !entity.getState().inState(State.PrimaryState.Dead))
			{
				entity.getState().setPrimaryState(State.PrimaryState.Dead);
				entity.tickKilled = curTick;
				return true;
			}

			return false;
		}
	}
}
