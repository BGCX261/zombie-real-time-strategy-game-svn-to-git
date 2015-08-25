using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    public class BuildingStats
    {
		/// <summary>
		/// defines the "type" of the building.
		/// </summary>
        public string buildingType;

        /// <summary>
        /// Width of the building in number of Cells.
        /// </summary>
        public byte width = 2;
		/// <summary>
		/// Height of the building in number of Cells.
		/// </summary>
        public byte height = 2;

        /// <summary>
        /// Maximum health of the Building.
        /// </summary>
        public short maxHealth = 2000;

        /// <summary>
        /// Can Resources be dropped off at the Building.
        /// </summary>
        public bool dropOffResources = false;

        /// <summary>
        /// Can this building produce units.
        /// </summary>
        public bool canProduce = false;

        /// <summary>
        /// A List of strings that define the types of Units that this building can produce.
        /// </summary>
        public List<string> productionTypes = new List<string>();

		/// <summary>
		/// Water cost to build this building.
		/// </summary>
		public short waterCost = 100;
		/// <summary>
		/// Food cost to build this building.
		/// </summary>
		public short foodCost = 100;
		/// <summary>
		/// Lumber cost to build this building.
		/// </summary>
		public short lumberCost = 100;
		/// <summary>
		/// Metal cost to build this building.
		/// </summary>
		public short metalCost = 100;

        public override string ToString()
        {
            string output = "Building Stats:\n";
            output += "Type:\t\t\t" + buildingType + "\n";
            output += "Dimensions:\t\t" + width + " X " + height + "\n";
            output += "Max Health:\t\t" + maxHealth + "\n";
            output += "Drop off Resources:\t" + dropOffResources + "\n";

			foreach (string s in productionTypes)
			{
				output += "Produces: " + s + "\n";
			}

            return output;
        }
    }
}
