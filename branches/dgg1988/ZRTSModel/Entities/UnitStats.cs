using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    public class UnitStats
    {
		/// <summary>
		/// The type of the unit.
		/// </summary>
        public string type = "";
        /** UNIT STATS **/
		
		/// <summary>
		/// The maximum health of a unit.
		/// </summary>
        public short maxHealth = 100;

		/// <summary>
		/// How much a Unit should move during a move Cycle.
		/// </summary>
        public float speed = 0.1f;
		/// <summary>
		/// How far a Unit can Attack.
		/// </summary>
        public float attackRange = 5.0f;
		/// <summary>
		/// How much damage a Unit does when it attacks.
		/// </summary>
        public short attack = 10;
		/// <summary>
		/// How many ticks per attack cycle.
		/// </summary>
        public byte attackTicks = 10;
		/// <summary>
		/// How far the Unit can see.
		/// </summary>
        public float visibilityRange = 6.0f;
		/// <summary>
		/// How much health a building gets per cycle when a Unit is building or repairing a building.
		/// </summary>
        public byte buildSpeed = 30;

		/** PRODUCTION COSTS **/
		
		///<summary>
		/// Water cost to produce this Unit.
		/// </summary>
        public byte waterCost = 0;
		/// <summary>
		/// Food cost to produce this unit.
		/// </summary>
        public byte foodCost = 0;
		/// <summary>
		/// Lumber cost to produce this unit.
		/// </summary>
        public byte lumberCost = 0;
		/// <summary>
		/// Metal cost to produce this unit.
		/// </summary>
        public byte metalCost = 0;

		/// <summary>
		/// Can this unit attack?
		/// </summary>
        public bool canAttack = true;
		/// <summary>
		/// Can this unit harvest resources?
		/// </summary>
        public bool canHarvest = false;
		/// <summary>
		/// Can this unit build buildings?
		/// </summary>
        public bool canBuild = true;
		/// <summary>
		/// Is this unit a Zombie?
		/// </summary>
        public bool isZombie = false;
    }
}
