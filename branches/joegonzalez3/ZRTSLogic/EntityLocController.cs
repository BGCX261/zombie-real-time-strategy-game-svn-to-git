using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Scenario;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using ZRTSModel.Player;

namespace ZRTSLogic
{
    /// <summary>
    /// This class will be used to handle all changes that need to be made when adding, removing, and moving
    /// entities in the game. It will handle all of these changes to make sure that all of the appropriate data 
    /// structures are updated. All of these changes should use the methods in this class to ensure consistency.
    /// </summary>
    public class EntityLocController
    {

        Scenario scenario;
        GameWorld gw;

        public EntityLocController(Scenario scenario)
        {
            this.scenario = scenario;
            this.gw = scenario.getGameWorld();
        }

        /// <summary>
        /// This method will attempt to add the entity at the given location.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="x">x coordinate in gamespace of where the entity should be inserted</param>
        /// <param name="y">y coordinate in gamespace of where the entity should be inserted</param>
        /// <returns>true if the entity is inserted successfully, false otherwise</returns>
        public bool addEntity(Entity entity, float x, float y)
        {
            bool success = false;

            /** Try Inserting into GameWorld first **/
            if (entity.entityType == Entity.EntityType.Unit)
            {
                // The coordinates of the Cell the unit is being inserted into.
                int xC = (int)Math.Floor(x);
                int yC = (int)Math.Floor(y);

                Cell c = gw.map.getCell(xC, yC);
                // Insert into gameworld if the target cell is empty
                if (c.isValid)
                {
                    Unit u = (Unit)entity;
                    c.setUnit(u); // Insert into the cell
                    gw.getUnits().Add(u); // Insert into the Unit list
                    u.setCell(c);
                    u.x = (float)xC + 0.5f;
                    u.y = (float)yC + 0.5f;
                    success = true;
                }
            }
            else // Inserting a Building, ResourceEntity, or ObjectEntity
            {
                success = gw.insert((StaticEntity)entity, (int)x, (int)y);
            }

            // If insert into GameWorld was a success, insert into right player
            if (success)
            {
                scenario.insertEntityIntoPlayer(entity);
            }

            return success;
        }


        /// <summary>
        /// This method will update the pointers to the Cell in the GameWorld that is occuppied by the Unit.
        /// </summary>
        /// <param name="unit">The unit who's location is being updated.</param>
        public void updateUnitLocation(Unit unit)
        {
            
            if (!unit.getCell().contains(unit.x, unit.y))
            {
                Cell newCell = gw.map.getCell((int)Math.Floor(unit.x), (int)Math.Floor(unit.y));
                Cell oldCell = unit.getCell();

                oldCell.removeUnit();

                // NOTE: doesn't check if newCell is already occupied.
                unit.setCell(newCell);
                newCell.setUnit(unit);
            }
        }


        /// <summary>
        /// This method will remove a Entity from the game.
        /// </summary>
        /// <param name="entity">The Entity being removed.</param>
        public void removeEntity(Entity entity)
        {
            if (entity.getEntityType() == Entity.EntityType.Unit) // entity is a Unit
            {
                // Remove Unit from its cell
                Unit u = (Unit)entity;
                Cell c = u.getCell();

                c.removeUnit();
                u.setCell(null);

                // Remove Unit from the Unit List.
                gw.getUnits().Remove(u);

            }
            else
            {
                gw.remove((StaticEntity)entity);
            }

            // Remove entity from its player.
            scenario.removeEntityFromPlayer(entity);
        }

    }
}
