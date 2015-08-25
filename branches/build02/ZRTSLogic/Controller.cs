using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using ZRTSModel.Scenario;
using ZRTSLogic.Action;
using ZRTSModel.Factories;
//using ZRTSModel;

namespace ZRTSLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class Controller
    {
        private short DEAD_DISAPPEAR_TICKS = 300; // Number of ticks to wait for a dead unit to disappear
        private long curTick = 0;

        public GameWorld gameWorld;
        public Scenario scenario;

        // Controls adding, removing, and moving entities.
        public EntityLocController locController;

		// Updates the visibilty map
		VisibilityMapLogic visMapLogic;

        /// <summary>
        /// This method will take a Scenario as input and will create a Controller object for that Scenario.
        /// </summary>
        /// <param name="scenario"></param>
        public Controller(Scenario scenario)
        {
            setUpController(scenario);
        }

        public Controller(string fileName)
        {
            //TODO: Load scenario from fileName.
        }

        private void setUpController(Scenario scenario)
        {
            this.scenario = scenario;
            this.gameWorld = this.scenario.getGameWorld();
			this.visMapLogic = new VisibilityMapLogic(scenario.getGameWorld(), scenario.getPlayer());
            this.locController = new EntityLocController(scenario, this.visMapLogic);
            
        }

        /// <summary>
        /// This function will be called by the GUI to call for another cycle of the game to be run. This function will
        /// update all of the states of the Entitys in the GameWorld.
        /// </summary>
        public void updateWorld()
        {
            List<Entity> entitiesToRemove = new List<Entity>();

			// Update all units.
            foreach (Unit u in gameWorld.getUnits())
            {
				updateEntity(u, entitiesToRemove);
            }

			// Update all buildings.
            foreach (Building b in gameWorld.getBuildings())
            {
				updateEntity(b, entitiesToRemove);
            }

			

            /** Remove any entities **/
            foreach (Entity e in entitiesToRemove)
            {
                locController.removeEntity(e);
            }

            curTick++;
        }

		/// <summary>
		/// The basic outline of the updateEntity method is as follows:
		/// 1, Have the entity perform the current action at the top of it's action queue (if any)
		/// 2, Check for changes in the entity's stats. (ex: check for death.)
		/// 3, Have the Entity react to any Events that occur within it's visibility range.
		/// (Events that either occur to the Entity or events that the Entity can "see")
		/// 4, Check if the Entity should be removed from the game. (When it's primary state is set to Remove.)
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="entitiesToRemove"></param>
		private void updateEntity(Entity entity, List<Entity> entitiesToRemove)
		{
			/*** Have entity perform it's current action (if any) ***/
			ActionController.Instance.update(entity, locController);

			Entity.EntityType type = entity.getEntityType();
			/*** Update stats of Entity ***/
			if (type == Entity.EntityType.Unit)
			{
				UnitStatsLogic.updateUnit((Unit)entity, curTick);
			}
			else
			{
				// Only Stats update occurs upon death of any StaticEntity right?
				if (entity.health <= 0)
				{
					entity.getState().setPrimaryState(State.PrimaryState.Dead);
				}
			}

			/*** Have Entity react to any Events ***/
			if (type == Entity.EntityType.Unit) // Only Units really need to react to Events.
			{

			}

			/*** Remove Entity if it needs to be removed. ***/
			if (entity.getState().getPrimaryState() == State.PrimaryState.Dead) // Should be remove. Changed for demo.
			{
				entitiesToRemove.Add(entity);
			}
		}

        /// <summary>
        /// Given an Entity and an ActionCommand, this function will attempt to give the Entity the ActionCommand
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="command"></param>
        /// <returns>false if the command was rejected, true if the command was accepted.</returns>
        public bool giveActionCommand(Entity entity, ActionCommand command)
        {
            return ActionController.Instance.giveCommand(entity, command);
        }

        /// <summary>
        /// Adds a static entity to the game.
        /// </summary>
        /// <param name="entity">The StaticEntity being added.</param>
        /// <param name="x">X-Coord of the desired "orgin Cell" of the static entity.</param>
        /// <param name="y">Y-Coord of the desired "orgin Cell" of the static entity.</param>
        /// <returns>true if the entity is added successfully, false otherwise.</returns>
        public bool addEntity(StaticEntity entity, short x, short y)
        {
            return locController.addEntity(entity, x, y);
        }

        /// <summary>
        /// Adds a Unit to the game.
        /// </summary>
        /// <param name="unit">The unit being added.</param>
        /// <param name="x">Desired X-coord of the Unit.</param>
        /// <param name="y">Desired Y-coord of the Unit.</param>
        /// <returns>true if the unit is added successfully, false otherwise.</returns>
        public bool addUnit(Unit unit, float x, float y)
        {
            return locController.addEntity(unit, x, y);
        }

		/// <summary>
		/// Removes an Entity from the game.
		/// </summary>
		/// <param name="entity">The entity being removed.</param>
        public void removeEntity(Entity entity)
        {
			locController.removeEntity(entity);
        }

        /// <summary>
        /// Gives a command to a unit to build a building.
        /// </summary>
        /// <param name="unit">Unit to build the building</param>
        /// <param name="b">Building to be built</param>
        /// <param name="c">Origin cell of the building-to-be</param>
        /// <returns></returns>
        public bool makeUnitBuild(Entity unit, Building b, Cell c)
        {
            if (unit.entityType == Entity.EntityType.Unit)
            {
                if (gameWorld.checkSpace(b, c) && gameWorld.checkResources(b, scenario.getPlayer()))
                {
                    if (locController.addEntity(b, c.Xcoord, c.Ycoord))
                    {
                        b.health = 1;
                        return ActionController.Instance.giveCommand(unit, new BuildAction(b, (Unit)unit, gameWorld));
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }
            return false;
        }


        /// <summary>
        /// Checking if (col,row) is within the game boundary
        /// </summary>
        /// <param name="colTile">Column tile</param>
        /// <param name="rowTile">Row tile</param>
        /// <returns>True if specific (col,row) tile is valid. Otherwise, False is returned!</returns>
        public bool isWithinGameBound(float colTile, float rowTile)
        {
            return colTile < gameWorld.map.width && colTile >= 0 && rowTile < gameWorld.map.height && rowTile >= 0;
        }

        /// <summary>
        /// Update selected entities
        /// </summary>
        /// <param name="unit"></param>
        public void updateSelectedEntities(List<ZRTSModel.Entities.Entity> unit)
        {
            scenario.getPlayer().selectEntities(unit);
        }

        /// <summary>
        /// Register a new observer to the scenario
        /// </summary>
        /// <param name="obs">observer</param>
        public void registerObserver(ZRTSModel.Scenario.Observer obs)
        {
            scenario.register(obs);
        }

        /// <summary>
        /// Unregister the observer from the scenario
        /// </summary>
        /// <param name="obs">removed observer</param>
        public void unregisterObserver(ZRTSModel.Scenario.Observer obs)
        {
            scenario.unregister(obs);
        }
    }
}
