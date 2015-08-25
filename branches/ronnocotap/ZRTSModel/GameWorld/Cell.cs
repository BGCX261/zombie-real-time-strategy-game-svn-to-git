using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ZRTSModel.GameEvent;
using ZRTSModel.Entities;

namespace ZRTSModel.GameWorld
{
    [Serializable()]
    public enum TileType
    {
        None, 
        Grass,
        Mountain,
        Sand
    }
    /// <summary>
    /// The class definition for a single Cell.
    /// </summary>
    [Serializable()]
    public class Cell : GameSubject
    {
        /*
         * attributes
         */
        Unit unit = null;

		public byte Xcoord;
		public byte Ycoord;
        public StaticEntity entity;
        public bool isValid;
        public Tile tile;
		public bool explored = false;


        /*
         * constructors
         */

        /// <summary>
        /// Blank constructor
        /// </summary>
        public Cell()
        {
			this.isValid = true;
            this.entity = null;
        }

        /// <summary>
        /// Includes a validity parameter
        /// </summary>
        /// <param name="isValid">Validity parameter</param>
		public Cell(bool isValid)
		{
			this.isValid = isValid;
            this.entity = null;
		}

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="c">The Cell to copy</param>
		public Cell(Cell c)
		{
            this.entity = c.entity;
			this.isValid = c.isValid;
		}

        public Cell(Tile t)
        {
            this.setTile(t);
        }

        /*
         * public functions
         */

        /// <summary>
        /// Given an x and y floating point coordinates, this function will determine if that coordinate is contained by this cell.
        /// </summary>
        /// <param name="x">The x coordinate in game space.</param>
        /// <param name="y">The y coordinate in game space</param>
        /// <returns>true if (x,y) is in this Cell, false otherwise.</returns>
        public bool contains(float x, float y)
        {
            return (x >= this.Xcoord && x < this.Xcoord + 1.0f && y >= this.Ycoord && y < this.Ycoord + 1.0f);
        }

        public Tile getTile()
        {
            return this.tile;
        }

        public void setTile(Tile tile)
        {
            this.tile = tile;
            if (this.isValid)
            {
                this.isValid = tile.passable;
            }
        }

        public bool setUnit(Unit unit)
        {
            if (this.isValid && this.unit == null)
            {
                this.unit = unit;
                this.isValid = false;
                return true;
            }

            return false;
        }

        public void removeUnit()
        {
            this.isValid = true;
            this.unit = null;
        }

        public Unit getUnit()
        {
            return this.unit;
        }
    }
}
