using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSModel;

namespace ZRTSModel
{
	/// <summary>
	/// This class will define an action where a Unit will move towards an enemy Entity until it is within range of the
	/// enemy. Once it is within range, it will then attack the enemy. If the enemy moves away, the Unit will proceed to follow 
	/// the enemy until it is killed.
	/// </summary>
	public class AttackAction : EntityAction
	{
		MoveAction moveAction = null;
		SimpleAttackUnitAction attackAction = null;
		UnitComponent target; // target Entity of the AttackAction

		public UnitComponent Target
		{
			get { return this.target; }
		}
		Gameworld gw;
		UnitComponent unit; // Unit performing the AttackAction
		public AttackAction(UnitComponent unit, ModelComponent target, Gameworld gw)
		{
			this.unit = unit;

			if (target is UnitComponent)
			{
				this.target = (UnitComponent)target;
			}
			this.gw = gw;
		}

		/// <summary>
		/// This function will perform the Attack Action.
		/// </summary>
		/// <returns>true if the action is completed, false if it is not.</returns>
		public override bool Work()
		{
			if (target == null)
			{
				return true;
			}

			// target is dead, return true.
			if (target.CurrentHealth <=0)
			{
				return true;
			}

			// unit cannot attack, return true.
			if (!unit.CanAttack)
			{
				return true;
			}

			// Target is in range attack target.
			if (targetIsInRange())
			{
				// Create a SimpleAttackAction if it is needed.
				if (this.attackAction == null)
				{
					attackAction = new SimpleAttackUnitAction(unit, target);
				}

				// Call the SimpleAttackAction
				attackAction.Work();

				// Set the MoveAction to null.
				moveAction = null;
			}
			// Target is not in range, move to it.
			else
			{
				if(moveAction != null && !targetIsInRange(target.PointLocation.X, target.PointLocation.Y, moveAction.targetX, moveAction.targetY))
				{
					moveAction = null;
				}


				// Create a MoveAction if it is needed.
				if (moveAction == null)
				{
					if (target is UnitComponent)
					{
						UnitComponent temp = (UnitComponent)target;
						moveAction = new MoveAction(temp.PointLocation.X, temp.PointLocation.Y, gw.GetMap(), unit);
					}
					else if (target is Building)
					{
						//StaticEntity temp = (StaticEntity)target;
						//moveAction = new MoveAction(temp.orginCell.Xcoord, temp.orginCell.Ycoord, gw, unit);
					}
				}
				// Set the SimpleAttackAction to null.
				attackAction = null;

				// Call the MoveAction.
				moveAction.Work();
			}

			return false;
		}

		/// <summary>
		/// This function will check if the target entity is within range of the unit.
		/// </summary>
		/// <returns>true if the target is withing the unit's range, false if it is not.</returns>
		private bool targetIsInRange()
		{
			if (target is UnitComponent)
			{
				UnitComponent tUnit = (UnitComponent)target;

				double dis = Math.Sqrt(Math.Pow((tUnit.PointLocation.X - unit.PointLocation.X), 2) + Math.Pow(tUnit.PointLocation.Y - unit.PointLocation.Y, 2));
				return (dis <= unit.AttackRange);
			}
			else
			{
				/*
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
				 */
			}

			return false;
		}

		/// <summary>
		/// This function will check if the target entity is within range of the unit.
		/// </summary>
		/// <returns>true if the target is withing the unit's range, false if it is not.</returns>
		private bool targetIsInRange(float x1, float y1, float x2, float y2)
		{
			if (target is UnitComponent)
			{
				UnitComponent tUnit = (UnitComponent)target;

				double dis = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow(y1 - y2, 2));
				return (dis <= unit.AttackRange);
			}
			else
			{
				/*
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
				 */
			}

			return false;
		}
	}
}
