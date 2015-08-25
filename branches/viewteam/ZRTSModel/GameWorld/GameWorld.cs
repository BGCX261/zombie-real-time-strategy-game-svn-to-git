using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using System.Xml.Serialization;
using System.IO;

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
                switch (e.type)
                {
                    case StaticEntity.Type.Object:
                        objects.Add((ObjectEntity)e);
                        break;
                    case StaticEntity.Type.Resource:
                        resources.Add((ResourceEntity)e);
                        break;
                    case StaticEntity.Type.Building:
                        buildings.Add((Building)e);
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
            switch (e.type)
            {
                case StaticEntity.Type.Object:
                    objects.Remove((ObjectEntity)e);
                    break;
                case StaticEntity.Type.Resource:
                    resources.Remove((ResourceEntity)e);
                    break;
                case StaticEntity.Type.Building:
                    buildings.Remove((Building)e);
                    break;
            }
        }

        public void printValidMap()
        {
            map.printValidMap();
        }

    }
}
