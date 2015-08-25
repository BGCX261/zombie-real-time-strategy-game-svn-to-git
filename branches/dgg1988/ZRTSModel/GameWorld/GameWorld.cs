using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using System.Xml.Serialization;
using System.IO;
using ZRTSModel.Player;

namespace ZRTSModel.GameWorld
{
    /// <summary>
    /// The GameWorld; includes the Map and individual lists for Entity subclasses.
    /// </summary>
    [Serializable()]
    public class GameWorld
    {
        /*
         * private variables
         */

        public Map map;
        List<Unit> units;
        List<Building> buildings;
        List<ResourceEntity> resources;
        List<ObjectEntity> objects;


        /*
         * constructors
         */

        public GameWorld()
        {

        }

        public GameWorld(int width, int height)
        {
            map = new Map(width, height);
            units = new List<Unit>();
            buildings = new List<Building>();
            resources = new List<ResourceEntity>();
            objects = new List<ObjectEntity>();
        }


        /*
         * public functions
         */

        public List<Unit> getUnits()
        {
            return this.units;
        }

        public List<Building> getBuildings()
        {
            return this.buildings;
        }

        /// <summary>
        /// Inserts a StaticEntity to the Map and the appropriate List, if possible.
        /// </summary>
        /// <param name="e">The StaticEntity to insert</param>
        /// <param name="x">The X-coordinate of the intended origin Cell</param>
        /// <param name="y">The Y-coordinate of the intended origin Cell</param>
        /// <returns>True if insertion was successful, false otherwise</returns>
        public bool insert(StaticEntity e, int x, int y)
        {
            bool worked = map.insert(e, x, y);
            if (worked)
            {
				Console.WriteLine("Gameworld inserted StaticEntity");
                switch (e.getEntityType())
                {
                    case Entity.EntityType.Object:
                        objects.Add((ObjectEntity)e);
                        break;
                    case Entity.EntityType.Resource:
                        resources.Add((ResourceEntity)e);
                        break;
                    case Entity.EntityType.Building:
						Console.WriteLine("Inserted Building");
                        this.buildings.Add((Building)e);
                        break;
                }

            }
            return worked;
        }

        /// <summary>
        /// Inserts a StaticEntity to the Map and the appropriate List, if possible.
        /// </summary>
        /// <param name="e">The StaticEntity to insert</param>
        /// <param name="c">The intended origin Cell</param>
        /// <returns>True if insertion was successful, false otherwise</returns>
        public bool insert(StaticEntity e, Cell c)
        {
            return insert(e, c.Xcoord, c.Ycoord);
        }

        /// <summary>
        /// Removes a StaticEntity from the Map and the appropriate List.
        /// </summary>
        /// <param name="e">The StaticEntity to remove</param>
        public void remove(StaticEntity e)
        {
            map.remove(e);
            switch (e.getEntityType())
            {
                case Entity.EntityType.Object:
                    objects.Remove((ObjectEntity)e);
                    break;
                case Entity.EntityType.Resource:
                    resources.Remove((ResourceEntity)e);
                    break;
                case Entity.EntityType.Building:
                    buildings.Remove((Building)e);
                    break;
            }
        }

        /// <summary>
        /// Check if there is room for the StaticEntity
        /// </summary>
        /// <param name="b">Building to be built</param>
        /// <param name="c">Cell to be origin cell</param>
        /// <returns>True if there is enough space, false if else</returns>
        public bool checkSpace(StaticEntity e, Cell c)
        {
            int x = c.Xcoord;
            int y = c.Ycoord;
            if (x < 0 || x + e.width > map.width)
                return false;
            if (y < 0 || y + e.height > map.height)
                return false;
            for (int i = x; i < x + e.width; i++)
            {
                for (int j = y; j < y + e.height; j++)
                {
                    if (!map.getCell(i, j).isValid)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check's if the player has enough resources to build the Building
        /// </summary>
        /// <param name="b">Building to be built</param>
        /// <param name="p">Player that is building</param>
        /// <returns>True if the player has enough resources, false if else</returns>
        public bool checkResources(Building b, ZRTSModel.Player.Player p)
        {
            if (b.stats.waterCost > p.player_resources[0])
                return false;
            if (b.stats.lumberCost > p.player_resources[1])
                return false;
            if (b.stats.foodCost > p.player_resources[2])
                return false;
            if (b.stats.metalCost > p.player_resources[3])
                return false;
            return true;
        }

        public void printValidMap()
        {
            map.printValidMap();
        }

    }
}
