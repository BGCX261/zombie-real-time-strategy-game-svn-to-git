using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    public class UnitStats
    {
        /** UNIT STATS **/
        public short maxHealth = 100;
        public float speed = 0.1f;
        public float attackRange = 5.0f;
        public short attack = 10;
        public byte attackTicks = 10;
        public float visibilityRange = 6.0f;


        /** UNIT ABILITIES **/
        public bool canAttack = false;
        public bool canHarvest = false;
        public bool canBuild = false;
    }
}
