using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
	/// <summary>
	/// This class will represent a Building entity in the game. Buildings are static, and take up
	/// full cells in the gamespace.
	/// </summary>
    public class Building : ModelComponent
    {
		/// <summary>
		/// Creates a new blank Building with a BuildingActionQueue.
		/// </summary>
		public Building()
		{
			this.actionQueue = new BuildingActionQueue();
			AddChild(this.actionQueue);
		}

        private string type;
		/// <summary>
		/// A string that denotes the type of building this object represents.
		/// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        // Dimensions of the building
        private int width;
		/// <summary>
		/// How many CellComponents wide this Building is.
		/// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

		/// <summary>
		/// How many CellComponents tall this Building is.
		/// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private bool completed;
		/// <summary>
		/// Has this building been completely built?
		/// </summary>
        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        // Max Health of the building
        private int maxHealth;
		/// <summary>
		/// The Maximum health of the building.
		/// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        private int currentHealth;
		/// <summary>
		/// The current health of the building.
		/// </summary>
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        // Can resources be dropped off at this building?
        private bool dropOffResources;
		/// <summary>
		/// Can resources be dropped off at this building?
		/// </summary>
        public bool DropOffResources
        {
            get { return dropOffResources; }
            set { dropOffResources = value; }
        }

        // Can this building produce units?
        private bool canProduce;
		/// <summary>
		/// Can this Building produce units?
		/// </summary>
        public bool CanProduce
        {
            get { return canProduce; }
            set { canProduce = value; }
        }

        // The types of units that this building can produce.
        private List<string> productionTypes = new List<string>();
		/// <summary>
		/// A List of strings where each string denotes a type of unit that this
		/// Building can produce.
		/// </summary>
        public List<string> ProductionTypes
        {
            get { return productionTypes; }
            set { productionTypes = value; }
        }

        private int waterCost;
		/// <summary>
		/// The water cost to build this building.
		/// </summary>
        public int WaterCost
        {
            get { return waterCost; }
            set { waterCost = value; }
        }
        private int foodCost;
		/// <summary>
		/// Food cost to build this building.
		/// </summary>
        public int FoodCost
        {
            get { return foodCost; }
            set { foodCost = value; }
        }
        private int lumberCost;
		/// <summary>
		/// Lumber cost to build this building.
		/// </summary>
        public int LumberCost
        {
            get { return lumberCost; }
            set { lumberCost = value; }
        }
        private int metalCost;
		/// <summary>
		/// Metal cost to build this building.
		/// </summary>
        public int MetalCost
        {
            get { return metalCost; }
            set { metalCost = value; }
        }

        private PointF pointLocation;
		/// <summary>
		/// The PointLocation of the Building's upper lefthand corner.
		/// </summary>
        public PointF PointLocation
        {
            get { return pointLocation; }
            set
            {
                pointLocation = value;
                // Make sure building has been added to tree.
                if (this.Parent != null)
                {
                    if (value != null)
                    {
                        pointLocation = value;
                        Map m = ((Gameworld)this.Parent.Parent.Parent.Parent).GetMap();
                        CellComponent cell = m.GetCellAt((int)pointLocation.X, (int)pointLocation.Y);
                        int x = cell.X;
                        int y = cell.Y;
                        for (int i = 0; i < this.Width; i++)
                        {
                            for (int j = 0; j < this.Height; j++)
                            {
                                CellComponent c = ((Map)cell.Parent).GetCellAt(x + i, y + j);
                                c.AddEntity(this);
                                this.CellsContainedWithin.Add(c);
                            }
                        }
                    }

                    else
                    {
                        foreach (CellComponent c in this.CellsContainedWithin)
                        {
                            c.RemoveEntity(this);
                        }
                        this.CellsContainedWithin.Clear();
                    }
                }
            }
        }

        private List<CellComponent> cellsContainedWithin = new List<CellComponent>();
		/// <summary>
		/// A List of CellComponents that this building sits on.
		/// </summary>
        public List<CellComponent> CellsContainedWithin
        {
            get { return cellsContainedWithin; }
            set { cellsContainedWithin = value; }
        }

		private BuildingActionQueue actionQueue = new BuildingActionQueue();
		/// <summary>
		/// A BuildingActionQueue containing the actions that this building is performing.
		/// </summary>
		public BuildingActionQueue BuildingActionQueue
		{
			get { return this.actionQueue; }
			set { this.actionQueue = value; }

		}

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
