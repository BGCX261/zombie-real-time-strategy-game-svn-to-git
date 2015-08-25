using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

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

        byte TICKS_PER_ATTACK = 20;
        byte ticksSinceLastAttk = 0;
        public SimpleAttackAction(Unit unit, Entity target)
        {
            this.unit = unit;
            this.target = target;
            this.actionType = ActionType.SimpleAttack;
        }

        public override bool work()
        {
            State targetState = target.getState();

            if (targetState.getPrimaryState() == State.PrimaryState.Dead)
            {
                return true;
            }

            if (targetIsInRange())
            {
                if (ticksSinceLastAttk % unit.stats.attackTicks == 0)
                {
                    Console.WriteLine("Attacking");
                    ticksSinceLastAttk = 0;

                    target.health -= unit.stats.attack;
                    if (target.health <= 0)
                    {
                        Console.WriteLine("Target Dead");
                        targetState.setPrimaryState(State.PrimaryState.Dead);
                        return true;
                    }
                }
            }
            else
            {
                Console.WriteLine("Target is out of range");
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

                double dis = Math.Sqrt(Math.Pow((tUnit.x - unit.x), 2) + Math.Pow(tUnit.y - unit.y,2));

                Console.WriteLine(dis);
                return (dis <= unit.stats.attackRange);
            }

            return false;
        }
    }
}
