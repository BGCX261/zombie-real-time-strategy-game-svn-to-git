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

namespace ZRTSLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class Controller
    {
        public GameWorld gameWorld;
        public Scenario scenario;

        // Controls all actions that entities may do.
        ActionController actionController;

        // Controls adding, removing, and moving entities.
        EntityLocController locController;

        // Creates Entities to add to the game.
        EntityCreator creator;

        // Factories
        UnitFactory unitFactory;
        BuildingFactory buildingFactory;
        TileFactory tileFactory;

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
            this.actionController = new ActionController(scenario);
            this.locController = new EntityLocController(scenario);
            

            setUpFactories();
            this.creator = new EntityCreator(this.unitFactory, this.buildingFactory);
        }

        private void setUpFactories()
        {
            //unitFactory = new UnitFactory();
            //buildingFactory = new BuildingFactory();
            //tileFactory = new TileFactory();
        }

        /// <summary>
        /// Returns a list of strings where each string denotes a kind of unit in the game. (This is the list of units stored 
        /// in 'Content/units/unitList.xml')
        /// </summary>
        /// <returns></returns>
        public List<string> getUnitStrings()
        {
            return unitFactory.getPrefixes();
        }

        public List<string> getBuildingStrings()
        {
            return buildingFactory.getBuildingTypes();
        }

        public List<string> getTileStrings()
        {
            return tileFactory.getTileTypes();
        }

        public List<Tile> getTiles()
        {
            return tileFactory.getTiles();
        }
        /// <summary>
        /// This function will be called by the GUI to call for another cycle of the game to be run. This function will
        /// update all of the states of the Entitys in the GameWorld.
        /// </summary>
        public void updateWorld()
        {
            foreach (Unit u in gameWorld.getUnits())
            {
                actionController.update(u, locController);
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
            return actionController.giveCommand(entity, command);
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

        public bool removeEntity(Entity entity)
        {
            return true;
        }

        public ActionController getActionController()
        {
            return this.actionController;
        }

    }
}
