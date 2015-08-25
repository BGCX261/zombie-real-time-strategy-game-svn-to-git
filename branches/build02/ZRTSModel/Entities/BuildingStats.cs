using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
	/// <summary>
	/// Defines the stats for a paticular "type" of building entity. 
	/// </summary>
    public class BuildingStats
    {
        public string buildingType;

        // Dimensions of the building
        public byte width = 2;
        public byte height = 2;

        // Max Health of the building
        public short maxHealth = 2000;

        // Can resources be dropped off at this building?
        public bool dropOffResources = false;

        // Can this building produce units?
        public bool canProduce = false;

        // The types of units that this building can produce.
        public List<string> productionTypes = new List<string>();

		// Resource Costs of the Building
		public short waterCost = 100;
		public short foodCost = 100;
		public short lumberCost = 100;
		public short metalCost = 100;

        public override string ToString()
        {
            string output = "Building Stats:\n";
            output += "Type:\t\t\t" + buildingType + "\n";
            output += "Dimensions:\t\t" + width + " X " + height + "\n";
            output += "Max Health:\t\t" + maxHealth + "\n";
            output += "Drop off Resources:\t" + dropOffResources + "\n";

            return output;
        }
    }
}
