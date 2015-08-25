using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// A piece of model representing a player.  A player has a UNIQUE name, a race, and resources.  
    /// It also contains as children a building and unit list.
    /// </summary>
    [Serializable()]
    public class PlayerComponent : ModelComponent
	{
		#region Attributes and Fields
		private string name;
		/// <summary>
		/// The name of this player.
		/// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string race;

		/// <summary>
		/// A string representing the character race of this player.
		/// </summary>
        public string Race
        {
            get { return race; }
            set { race = value; }
        }

		private List<PlayerComponent> enemyList;
		/// <summary>
		/// A List of PlayerComponents that represent which players are enemies of the player
		/// represented by this PlayerComponent.
		/// </summary>
		public List<PlayerComponent> EnemyList
		{
			get { return enemyList; }
		}

        private BuildingList buildingList;

		/// <summary>
		/// The BuildList object composed of all BuildingComponents belonging to this player.
		/// </summary>
        public BuildingList BuildingList
        {
            get { return buildingList; }
        }

		private UnitList unitList;
		/// <summary>
		/// The UnitList object composed of all UnitComponents belonging to this player.
		/// </summary>
		public UnitList UnitList
		{
			get { return this.unitList; }
		}

		#endregion

		/// <summary>
		/// Creates a new empty. PlayerComponent
		/// </summary>
        public PlayerComponent()
        {
            PlayerResources resources = new PlayerResources();
            AddChild(resources);

            unitList = new UnitList();
            AddChild(unitList);

            buildingList = new BuildingList();
            AddChild(buildingList);

			enemyList = new List<PlayerComponent>();
        }
        /// <summary>
        /// Adds a building to this Player
        /// </summary>
        /// <param name="building">The building to add</param>
        public void addBuilding(ModelComponent building)
        {
            buildingList.AddChild(building);
        }

        /// <summary>
        /// Removes a building from this Player
        /// </summary>
        /// <param name="building">The building to remove</param>
        public void removeBuilding(ModelComponent building)
        {
            buildingList.RemoveChild(building);
        }

        public int Gold
        {
            get 
            {
                return GetResources().Gold;
            }
            set
            {
                GetResources().Gold = value;
            }
        }

        public int Wood
        {
            get
            {
                return GetResources().Wood;
            }
            set
            {
                GetResources().Wood = value;
            }
        }

        public int Metal
        {
            get
            {
                return GetResources().Metal;
            }
            set
            {
                GetResources().Metal = value;
            }
        }

        /// <summary>
        /// Gets the Player Resources
        /// </summary>
        /// <returns>The PlayerResources</returns>
        public PlayerResources GetResources()
        {
            foreach (ModelComponent component in GetChildren())
            {
                if (component is PlayerResources)
                {
                    return (PlayerResources)component;
                }
            }
            return null;
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Gets the list of all the Units.
        /// </summary>
        /// <returns>The UnitList</returns>
        public UnitList GetUnitList()
        {
            UnitList list = null;
            foreach (ModelComponent component in GetChildren())
            {
                if (component is UnitList)
                {
                    list = (UnitList)component;
                    break;
                }
            }
            return list;
        }
    }
}
