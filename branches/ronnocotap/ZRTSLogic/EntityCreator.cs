using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.Factories;
namespace ZRTSLogic
{
    /// <summary>
    /// The EntityCreator class will be used to create new entities in the game.
    /// </summary>
    public class EntityCreator
    {
        UnitFactory uFact;
        BuildingFactory bFact;

        public EntityCreator(UnitFactory uFact, BuildingFactory bFact)
        {
            this.uFact = uFact;
            this.bFact = bFact;
        }

        public Unit createUnit(ZRTSModel.Player.Player owner, string unitType)
        {
            Unit unit = new Unit(owner, uFact.getStats(unitType));
            return unit;
        }
    }
}
