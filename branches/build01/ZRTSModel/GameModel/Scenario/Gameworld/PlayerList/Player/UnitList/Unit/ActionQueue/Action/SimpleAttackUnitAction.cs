using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel;
using ZRTSModel.GameModel;
namespace ZRTSModel
{
    /// <summary>
    /// This class will represent a simple attack where the unit will attack the target as long as the target is alive and inside
    /// the unit's range.
    /// </summary>
    public class SimpleAttackUnitAction : EntityAction
    {
        private UnitComponent unit;
        private UnitComponent target;

        private int ticksSinceLastAttk = 0;
        public SimpleAttackUnitAction(UnitComponent unit, UnitComponent target)
        {
            this.unit = unit;
            this.target = target;
        }

        private bool targetIsInRange()
        {
            float distance = (float)Math.Sqrt(Math.Pow((double)(unit.PointLocation.X - target.PointLocation.X), 2.0) + Math.Pow((double)(unit.PointLocation.Y - target.PointLocation.Y), 2.0));
            return distance <= unit.AttackRange;
        }

		private void updateUnitOrientation()
		{
			PointF directionVector = new PointF((target.PointLocation.X - unit.PointLocation.X), (target.PointLocation.Y - unit.PointLocation.Y));
            //unit.Orientation = (int)Math.Atan2((double)directionVector.Y, (double)directionVector.X);
		}

        public override bool Work()
        {
            bool cannotAttack = target.CurrentHealth <= 0;
            if (!cannotAttack)
            {
                if (targetIsInRange())
                {
                    if (ticksSinceLastAttk % (unit.AttackTicks * 10) == 0)
                    {
						unit.State = UnitComponent.UnitState.ATTACKING;
                        updateUnitOrientation();
                        ticksSinceLastAttk = 0;
                        target.CurrentHealth -= unit.Attack;
                        if (target.CurrentHealth <= 0)
                        {
                            cannotAttack = true;
                        }
						unit.createAttackEvent(target);

						if (target.PointLocation.Y > unit.PointLocation.Y)
						{
							if (target.PointLocation.X > unit.PointLocation.X)
							{
								unit.UnitOrient = UnitComponent.Orient.SE;
							}
							else if (target.PointLocation.X < unit.PointLocation.X)
							{
								unit.UnitOrient = UnitComponent.Orient.SW;
							}
							else
							{
								unit.UnitOrient = UnitComponent.Orient.S;
							}
						}
						else if (target.PointLocation.Y < unit.PointLocation.Y)
						{
							if (target.PointLocation.X > unit.PointLocation.X)
							{
								unit.UnitOrient = UnitComponent.Orient.NE;
							}
							else if (target.PointLocation.X < unit.PointLocation.X)
							{
								unit.UnitOrient = UnitComponent.Orient.NW;
							}
							else
							{
								unit.UnitOrient = UnitComponent.Orient.N;
							}
						}
						else if (target.PointLocation.X < unit.PointLocation.X)
						{
							unit.UnitOrient = UnitComponent.Orient.W;
						}
						else if (target.PointLocation.X > unit.PointLocation.X)
						{
							unit.UnitOrient = UnitComponent.Orient.E;
						}
                    }
                    ticksSinceLastAttk++;
                }
                else
                {
                    cannotAttack = true;
                }
            }
            return cannotAttack;
        }
    }
}
