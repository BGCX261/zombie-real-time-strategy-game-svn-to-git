using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSLogic;
namespace ZRTSLogic.Action
{
    /// <summary>
    /// This class will represent a simple attack where the unit will attack the target as long as the target is alive and inside
    /// the unit's range.
    /// </summary>
    public class SimpleAttackAction : ActionCommand
    {
        Unit unit;
        Entity target;

        byte ticksSinceLastAttk = 0;
        public SimpleAttackAction(Unit unit, Entity target)
        {
            this.unit = unit;
            this.target = target;
            this.actionType = ActionType.SimpleAttack;
        }

        /// <summary>
        /// Makes the attack aciton.
        /// </summary>
        /// <returns>returns true if the target is out of range or if the target is dead. false otherwise.</returns>
        public override bool work()
        {
            State targetState = target.getState();

            // End action if target is dead.
            if (targetState.getPrimaryState() == State.PrimaryState.Dead)
            {
                return true;
            }

            bool targetInRange = targetIsInRange();

            // If target is not dead, is in range, and it is time to complete an attack cycle.
            if (ticksSinceLastAttk % unit.stats.attackTicks == 0 && targetInRange)
            {
				updateUnitOrientation(); // Make unit face its target.
                unit.getState().setPrimaryState(State.PrimaryState.Attacking);
                ticksSinceLastAttk = 0;

                target.health -= unit.stats.attack;
                if (target.health <= 0)
                {
                    targetState.setPrimaryState(State.PrimaryState.Dead);
                    return true;
                }
            }
            else if (!targetInRange)
            {
                // Target is out of range, end action.
                return true;
            }


            ticksSinceLastAttk++;
            return false;
        }

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

		private void updateUnitOrientation()
		{
			float x, y;
			if (target.entityType == Entity.EntityType.Unit)
			{
				Unit tempUnit = (Unit)target;
				x = tempUnit.x;
				y = tempUnit.y;
			}
			else
			{
				StaticEntity tempSE = (StaticEntity)target;
				// NOTE: This should be changed to the cell that is closest to the unit.
				x = tempSE.orginCell.Xcoord;
				y = tempSE.orginCell.Ycoord;
			}

			if (x == unit.x)
			{
				if (y > unit.y)
				{
					unit.orientation = Unit.Orientation.S;
				}
				else
				{
					unit.orientation = Unit.Orientation.N;
				}

			}
			else if (y == unit.y)
			{
				if (x > unit.x)
				{
					unit.orientation = Unit.Orientation.E;
				}
				else
				{
					unit.orientation = Unit.Orientation.W;
				}
			}
			else if (y < unit.y && x < unit.x)
			{
				unit.orientation = Unit.Orientation.NW;
			}
			else if (y < unit.y && x > unit.x)
			{
				unit.orientation = Unit.Orientation.NE;
			}
			else if (y > unit.y && x < unit.x)
			{
				unit.orientation = Unit.Orientation.SW;
			}
			else if (y > unit.y && x > unit.x)
			{
				unit.orientation = Unit.Orientation.SE;
			}
		}
    }
}
