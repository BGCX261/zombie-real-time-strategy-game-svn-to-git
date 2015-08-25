using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;

namespace ZRTSLogic.Action
{
	/// <summary>
	/// This class will define an action where a Unit will move towards an enemy Entity until it is within range of the
	/// enemy. Once it is within range, it will then attack the enemy. If the enemy moves away, the Unit will proceed to follow 
	/// the enemy until it is killed.
	/// </summary>
	public class AttackAction : ActionCommand
	{
		MoveAction moveAction = null;
		SimpleAttackAction attackAction = null;
		Entity target; // target Entity of the AttackAction
		GameWorld gw;
		Unit unit; // Unit performing the AttackAction
		public AttackAction(Unit unit, Entity target, GameWorld gw)
		{
			this.unit = unit;
			this.target = target;
			this.gw = gw;
		}

		/// <summary>
		/// This function will perform the Attack Action.
		/// </summary>
		/// <returns>true if the action is completed, false if it is not.</returns>
		public override bool work()
		{
			// target is dead, return true.
			if (target.getState().getPrimaryState() == State.PrimaryState.Dead)
			{
				return true;
			}

			// unit cannot attack, return true.
			if (!unit.stats.canAttack)
			{
				return true;
			}

			// Target is in range attack target.
			if (targetIsInRange())
			{
				// Create a SimpleAttackAction if it is needed.
				if (this.attackAction == null)
				{
					attackAction = new SimpleAttackAction(unit, target);
				}

				// Call the SimpleAttackAction
				attackAction.work();

				// Set the MoveAction to null.
				moveAction = null;
			}
			// Target is not in range, move to it.
			else
			{
				// Create a MoveAction if it is needed.
				if (moveAction == null)
				{
					if (target.getEntityType() == Entity.EntityType.Unit)
					{
						Unit temp = (Unit)target;
						moveAction = new MoveAction(temp.x, temp.y, gw, unit);
					}
					else
					{
						StaticEntity temp = (StaticEntity)target;
						moveAction = new MoveAction(temp.orginCell.Xcoord, temp.orginCell.Ycoord, gw, unit);
					}
				}
				// Set the SimpleAttackAction to null.
				attackAction = null;

				// Call the MoveAction.
				moveAction.work();
			}

			return false;
		}

		/// <summary>
		/// This function will check if the target entity is within range of the unit.
		/// </summary>
		/// <returns>true if the target is withing the unit's range, false if it is not.</returns>
		private bool targetIsInRange()
		{
			if (target.getEntityType() == Entity.EntityType.Unit)
			{
				Unit tUnit = (Unit)target;

				double dis = Math.Sqrt(Math.Pow((tUnit.x - unit.x), 2) + Math.Pow(tUnit.y - unit.y, 2));
				return (dis <= unit.stats.attackRange);
			}
			else
			{
				StaticEntity se = (StaticEntity)(target);
				float xC = se.orginCell.Xcoord;
				float yC = se.orginCell.Ycoord;
				short width = se.width;
				short height = se.height;

				for (int i = 0; i < width; i++)
				{
					for (int j = 0; j < height; j++)
					{
						if (EntityLocController.findDistance(unit.x, unit.y, xC + i, yC + j) <= unit.stats.attackRange)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
	}
}
