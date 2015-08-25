using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using ZRTSModel.GameWorld;
using ZRTSModel;
using Microsoft.Xna.Framework;
using ZRTSModel.GameModel;

namespace ZRTSModel
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
    public class BuildAction : EntityAction
    {
        private Building building;
        private Map map;
        short TICKS_PER_CYCLE = 20;
        short curTicks = 0;

        /// <summary>
        /// </summary>
        /// <param name="building"></param>
        /// <param name="unit"></param>
        public BuildAction(Building building, Map map)
        {
            this.building = building;
            this.map = map;
        }

        /// <summary>
        /// This function will perform a building cycle if the number of ticks since the last cycle is equal to TICKS_PER_CYCLE.
        /// </summary>
        /// <returns>true if the building is complete and the action is finished, false otherwise.</returns>
        public override bool Work()
        {

            if (!building.Completed)    
            {
                if (curTicks % TICKS_PER_CYCLE == 0)
                {
                    // Check if unit is adjacent to building.
                    if (isUnitNextToBuilding())
                    {
						UnitComponent unit = (UnitComponent)Parent.Parent;
						unit.State = UnitComponent.UnitState.BUILDING;

                        UnitComponent worker = (UnitComponent)Parent.Parent;
                        // Add the building to the model if we have not done so yet.
                        if (building.Parent == null)
                        {
                            
                            // TODO: Ensure that the spaces are cleared.  Perhaps wait/give up, as with move?
                            PlayerComponent player = Parent.Parent.Parent.Parent as PlayerComponent;
                            
                            if (!map.addBuildingToMap(building))    // add building to the map
                            {
                                return false;
                            }

                            player.addBuilding(building);       // add building to player's building list
                        }

                        updateBuildingProgress(building, worker);
                        
                    }
                    else
                    {
                        // Move towards the building. Insert a move action into the Unit's action queue.
                        CellComponent targetCell = findClosestCell(((UnitComponent)Parent.Parent).PointLocation);
						MoveAction moveAction = new MoveAction(targetCell.X, targetCell.Y, map, ((UnitComponent)Parent.Parent));
                        Parent.AddChildAt(moveAction, 0);
                    }
                }
            }
            curTicks++;
            return building.Completed;
        }


        /// <summary>
        /// Update building's construction progress
        /// </summary>
        /// <param name="building">building underconstruction</param>
        /// <param name="worker">Assigned worker for place the building</param>
        private void updateBuildingProgress(Building building, UnitComponent worker)
        {
            if (building.MaxHealth - building.CurrentHealth <= worker.BuildSpeed)
            {
                // Finish the building.
                building.CurrentHealth = building.MaxHealth;
                building.Completed = true;
            }
            else
            {
                // Continue building the building.
                building.CurrentHealth += ((UnitComponent)Parent.Parent).BuildSpeed;
            }
        }



        /// <summary>
        /// This function checks if the unit is occupying a Cell next to the building.
        /// </summary>
        /// <returns>true if the unit is next to the building, false otherwise.</returns>
        private bool isUnitNextToBuilding()
        {
            float xC = building.PointLocation.X;
            float yC = building.PointLocation.Y;
            int width = building.Width;
            int height = building.Height;
            UnitComponent unit = (UnitComponent)Parent.Parent;
            if (building.PointLocation.X - 1 < unit.PointLocation.X && unit.PointLocation.X < building.PointLocation.X + width + 2)
            {
                if (building.PointLocation.Y - 1 < unit.PointLocation.Y && unit.PointLocation.Y < building.PointLocation.Y + height + 2)
                {
                    return true;
                }
            }

            return false;
        }

        private static double findDistanceSquared(float x1, float y1, float x2, float y2)
        {
            double dis = Math.Pow((double)(x1 - x2), 2) + Math.Pow((double)(y1 - y2), 2);
            return dis;
        }


        /// <summary>
        /// find the closest cell from the specified destination
        /// </summary>
        /// <param name="point">Destination</param>
        /// <returns>Closest cell</returns>
        private CellComponent findClosestCell(PointF point)
        {
            double distanceSquared = Math.Pow(map.GetWidth(), 2.0) + Math.Pow(map.GetHeight(), 2.0);
            CellComponent cell = null;
            for (int i = Math.Max((int)building.PointLocation.X - 1, 0); i <= building.PointLocation.X + building.Width + 1; i++)
            {
                for (int j = Math.Max((int)building.PointLocation.Y - 1, 0); j <= building.PointLocation.Y + building.Height + 1; j++)
                {
                    // Ignore cases in the middle of the proposed building site.
                    if (!(i >= (int)building.PointLocation.X && i < (int)building.PointLocation.X + building.Width && j >= (int)building.PointLocation.Y && j < (int)building.PointLocation.Y + building.Height))
                    {
                        double calculatedDistanceSquared = findDistanceSquared(point.X, point.Y, i, j);
                        if (calculatedDistanceSquared <= distanceSquared)
                        {
                            if (map.GetCellAt(i, j) != null)
                            {
                                cell = map.GetCellAt(i, j);
                                distanceSquared = calculatedDistanceSquared;
                            }
                        }
                    }
                }
            }

            return cell;
        }
    }
}
