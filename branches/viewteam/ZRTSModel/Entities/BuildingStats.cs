using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    public class BuildingStats
    {
        public string buildingType;

        // Dimensions of the building
        public short width = 1;
        public short height = 1;

        // Max Health of the building
        public short maxHealth = 100;

        // How many cycles a build action must complete for the building to be completed.
        public short buildCycles = 20;

        // Can resources be dropped off at this building?
        public bool dropOffResources = false;

        public override string ToString()
        {
            string output = "Building Stats:\n";
            output += "Type:\t\t\t" + buildingType + "\n";
            output += "Dimensions:\t\t" + width + " X " + height + "\n";
            output += "Max Health:\t\t" + maxHealth + "\n";
            output += "Build Cycles:\t\t" + buildCycles + "\n";
            output += "Drop off Resources:\t" + dropOffResources + "\n";



            return output;
        }
    }
}
