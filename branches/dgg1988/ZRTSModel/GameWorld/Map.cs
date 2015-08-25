using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using ZRTSModel.Entities;

namespace ZRTSModel.GameWorld
{
    /// <summary>
    /// The Map class
    /// </summary>
    [Serializable()]
    public class Map
    {
        /*
         * private variables
         */

        public byte width;
        public byte height;
        public Cell[,] cells;


        /*
         * constructors
         */

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Intended Map width</param>
        /// <param name="height">Intended Map height</param>
        public Map(int width, int height)
        {
            this.width = (byte)width;
            this.height = (byte)height;

            this.cells = new Cell[width, height];
            for (byte i = 0; i < width; i++)
            {
                for (byte j = 0; j < height; j++)
                {
					cells[i, j] = new Cell();
                    cells[i, j].Xcoord = i;
                    cells[i, j].Ycoord = j;
                }
            }
        }


        /*
         * public functions
         */

        /// <summary>
        /// Inserts a StaticEntity to the Map, if possible
        /// </summary>
        /// <param name="e">The StaticEntity to add</param>
        /// <param name="x">The X-coordinate of the intended origin Cell</param>
        /// <param name="y">The Y-coordinate of the intended origin Cell</param>
        /// <returns>True if insertion was successful, false if not</returns>
        public bool insert(StaticEntity e, int x, int y)
        {
            // check to see if we can actually insert the entity here; if not, return false
            if (x < 0 || x + e.width > this.width)
                return false;
            if (y < 0 || y + e.height > this.height)
                return false;
            for (int j = y; j < y + e.height; j++)
            {
                for (int i = x; i < x + e.width; i++)
                {
                    if (!cells[i,j].isValid)
                        return false;
                }
            }

            // set the appropriate Cell StaticEntity pointers to the given StaticEntity
            for (int j = y; j < y + e.height; j++)
            {
                for (int i = x; i < x + e.width; i++)
                {
                    cells[i, j].entity = e;
					cells[i, j].isValid = false;
                }
            }

            // set the given StaticEntity's origin Cell to the given (x, y) coordinate and return true
            e.setOrginCell(cells[x, y]);

			Console.WriteLine("Inserted building at: " + x + ", " + y);
            return true;
        }

        /// <summary>
        /// Inserts a StaticEntity to the Map, if possible
        /// </summary>
        /// <param name="e">The StaticEntity to add</param>
        /// <param name="c">The intended origin Cell</param>
        /// <returns>True if insertion was successful, false if not</returns>
        public bool insert(StaticEntity e, Cell c)
        {
            return insert(e, c.Xcoord, c.Ycoord);
        }

        /// <summary>
        /// Removes a StaticEntity from the Map
        /// </summary>
        /// <param name="e">The StaticEntity to remove</param>
        public void remove(StaticEntity e)
        {
            int x = e.orginCell.Xcoord;
            int y = e.orginCell.Ycoord;
            int h = e.height;
            int w = e.width;

            for (int j = y; j < y + h; j++)
            {
                for (int i = x; i < x + w; i++)
                {
                    cells[i, j].entity = null;
					cells[i, j].isValid = true;
                }
            }
        }

        /// <summary>
        /// Returns the Cell at the given x and y coordinates
        /// </summary>
        /// <param name="x">The X-coordinate of the Cell</param>
        /// <param name="y">The Y-coordinate of the Cell</param>
        /// <returns>null if the Cell does not exist, the Cell if it does</returns>
        public Cell getCell(int x, int y)
		{
            if (x < 0 || x >= width || y < 0 || y >= height)
                return null;
			return cells[x, y];
		}

        /// <summary>
        /// Returns a portion of the overall Map
        /// </summary>
        /// <param name="x">The X-coordinate of the origin Cell</param>
        /// <param name="y">The Y-coordinate of the origin Cell</param>
        /// <param name="width">The width of the area to return</param>
        /// <param name="height">The height of the area to return</param>
        /// <returns>The requested portion of the Map</returns>
        public Cell[,] getCells(int x, int y, int width, int height)
        {
            Cell[,] c = new Cell[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    c[i, j] = getCell(x + i, y + j);
                }
            }
            return c;
        }


        /*
         * helper functions
         */

        public void printValidMap()
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (cells[i, j].isValid)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("x");
                    }

                }
                Console.WriteLine("");

            }

        }

		public void printExploredMap()
		{
			for (int j = 0; j < height; j++)
			{
				for (int i = 0; i < width; i++)
				{
					if (cells[i, j].explored)
					{
						Console.Write(" ");
					}
					else
					{
						Console.Write("x");
					}

				}
				Console.WriteLine("");

			}

		}
    }
}
