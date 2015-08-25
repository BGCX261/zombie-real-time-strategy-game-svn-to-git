using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;

namespace ZRTSLogic.Action
{
    /// <summary>
    /// This class will represent an Action that can be given to a unit to tell that unit to "build" a building.
    /// 
    /// Note: This action assumes that the Building is already inserted into the GameWorld. This action does not deduct the 
    /// resources required to build the Building from the Player.
    /// 
    /// When a Unit is given a BuildAction, it will move towards the building until it is adjacent to the building. Then, for every
    /// build cycle (which occurs every TICKS_PER_CYCLE ticks), the current health of the building will be increased by the Unit's
    /// "buildSpeed" until the building is at maximum health. Then the buildings "isComplete" flag will be set to true, indicating that
    /// the building has been completed.
    /// </summary>
    public class BuildAction : ActionCommand
    {
        Building building;
        Unit unit;
        GameWorld gw;
        short TICKS_PER_CYCLE = 20;
        short curTicks = 0;

        /// <summary>
        /// </summary>
        /// <param name="building"></param>
        /// <param name="unit"></param>
        public BuildAction(Building building, Unit unit, GameWorld gw)
        {
            this.building = building;
            this.unit = unit;
            this.actionType = ActionType.BuildBuilding;
            this.gw = gw;
        }

        /// <summary>
        /// This function will perform a building cycle if the number of ticks since the last cycle is equal to TICKS_PER_CYCLE.
        /// </summary>
        /// <returns>true if the building is complete and the action is finished, false otherwise.</returns>
        public override bool work()
        {
            // Building is complete, finish action.
            if (building.health == building.stats.maxHealth)
            {
                return true;
            }

            // Unit cannot build, disregard action.
            if (!unit.stats.canBuild)
            {
                return true;
            }

            if (curTicks % TICKS_PER_CYCLE == 0)
            {
                // Check if unit is adjacent to building.
                if (isUnitNextToBuilding())
                {
                    unit.getState().setPrimaryState(State.PrimaryState.Building);
                    if (building.stats.maxHealth - building.health <= unit.stats.buildSpeed)
                    {
                        // Finish the building.
                        building.health = building.stats.maxHealth;
                        building.isCompleted = true;
                        return true;
                    }
                    else
                    {
                        // Continue building the building.
                        building.health += unit.stats.buildSpeed;
                    }
                }
                else
                {
                    // Move towards the building. Insert a move action into the Unit's action queue.
                    Cell targetCell = EntityLocController.findClosestCell(unit, building, gw);
                    MoveAction moveAction = new MoveAction(targetCell.Xcoord, targetCell.Ycoord, gw, unit);
                    ActionController.insertIntoActionQueue(unit, moveAction);
                }
            }

            curTicks++;
            return false;
        }

        /// <summary>
        /// This function checks if the unit is occupying a Cell next to the building.
        /// </summary>
        /// <returns>true if the unit is next to the building, false otherwise.</returns>
        private bool isUnitNextToBuilding()
        {
            float xC = building.orginCell.Xcoord;
            float yC = building.orginCell.Ycoord;
            short width = building.width;
            short height = building.height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (EntityLocController.findDistance(unit.x, unit.y, xC + i, yC + j) <= 1.99)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
