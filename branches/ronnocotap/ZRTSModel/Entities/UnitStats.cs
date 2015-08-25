using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    public class UnitStats
    {
        public string type = "";            // The type of the Unit.
        /** UNIT STATS **/
        public short maxHealth = 100;       // The maximum health of a unit.
        public float speed = 0.1f;          // How much a Unit should move during a move cycle.
        public float attackRange = 5.0f;    // How far a Unit can attack
        public short attack = 10;           // How much damage a Unit does when it attacks.
        public byte attackTicks = 10;       // How many ticks occur per attack cycle.
        public float visibilityRange = 6.0f;// How far can the unit see.
        public byte buildSpeed = 30;        // How much health a building gets per build cycle the Unit completes when building or repairing a building.

		/** PRODUCTION COSTS **/
        public byte waterCost = 0;
        public byte foodCost = 0;
        public byte lumberCost = 0;
        public byte metalCost = 0;

        /** UNIT ABILITIES **/
        public bool canAttack = false;      // Can this Unit attack?
        public bool canHarvest = false;     // Can this Unit harvest resources?
        public bool canBuild = true;       // Can this Unit build buildings?
        public bool isZombie = false;       // Is this Unit a zombie?
    }
}
