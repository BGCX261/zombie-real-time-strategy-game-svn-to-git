using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
    public class Building : ModelComponent
    {
		public Building()
		{
			this.actionQueue = new BuildingActionQueue();
			AddChild(this.actionQueue);
		}

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        // Dimensions of the building
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private bool completed;

        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        // Max Health of the building
        private int maxHealth;

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        private int currentHealth;

        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        // Can resources be dropped off at this building?
        private bool dropOffResources;

        public bool DropOffResources
        {
            get { return dropOffResources; }
            set { dropOffResources = value; }
        }

        // Can this building produce units?
        private bool canProduce;

        public bool CanProduce
        {
            get { return canProduce; }
            set { canProduce = value; }
        }

        // The types of units that this building can produce.
        private List<string> productionTypes = new List<string>();

        public List<string> ProductionTypes
        {
            get { return productionTypes; }
            set { productionTypes = value; }
        }

        private int waterCost;

        public int WaterCost
        {
            get { return waterCost; }
            set { waterCost = value; }
        }
        private int foodCost;

        public int FoodCost
        {
            get { return foodCost; }
            set { foodCost = value; }
        }
        private int lumberCost;

        public int LumberCost
        {
            get { return lumberCost; }
            set { lumberCost = value; }
        }
        private int metalCost;

        public int MetalCost
        {
            get { return metalCost; }
            set { metalCost = value; }
        }

        private PointF pointLocation;

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

        public List<CellComponent> CellsContainedWithin
        {
            get { return cellsContainedWithin; }
            set { cellsContainedWithin = value; }
        }

		private BuildingActionQueue actionQueue = new BuildingActionQueue();

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
