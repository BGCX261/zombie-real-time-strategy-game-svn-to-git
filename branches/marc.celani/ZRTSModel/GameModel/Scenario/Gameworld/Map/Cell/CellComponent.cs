using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;

namespace ZRTSModel
{
    /// <summary>
    /// Cell object:
    /// 
    /// A discrete unit of space on a map.  Can contain a map resource.  Each cell contains a tile to display itself and
    /// what occupies the cell.
    /// </summary>
    [Serializable()]
    public class CellComponent : ModelComponent
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public event TileChangedHandler TileChangedEvent;
        public event EntityInCellChangedHandler UnitAddedEvent;
        public event EntityInCellChangedHandler UnitRemovedEvent;
		public event UnitAttackedEnemyHandler UnitInCellAttackedEnemyEvent;

        private List<ModelComponent> entitiesContainedWithin = new List<ModelComponent>();
		public List<ModelComponent> EntitiesContainedWithin { get { return entitiesContainedWithin; } }
		public bool ContainsActiveEntities
		{
			get
			{
				return (entitiesContainedWithin.Count > 0);
			}
		}

        /// <summary>
        /// Adds a Component to this CellComponent.
        /// </summary>
        /// <param name="child">The Component to add</param>
        override public void AddChild(ModelComponent child)
        {
            // Ensure that we only have one tile or resource
            List<ModelComponent> toRemove =  new List<ModelComponent>();
            if (child is Tile)
            {
                foreach (ModelComponent tile in GetChildren())
                {
                    if (tile is Tile)
                    {
                        toRemove.Add(tile);
                    }
                }
            }
            else if (child is MapResource)
            {
                foreach (ModelComponent resource in GetChildren())
                {
                    if (resource is MapResource)
                    {
                        toRemove.Add(resource);
                        if (entitiesContainedWithin.Count != 0)
                        {
                            UnitArgs args = new UnitArgs();
                            args.Unit = (UnitComponent)entitiesContainedWithin[0];
                            entitiesContainedWithin.Clear();
                            if (UnitRemovedEvent != null)
                                UnitRemovedEvent(this, args);
                        }
                    }
                }
                entitiesContainedWithin.Add(child);
            }
            foreach (ModelComponent component in toRemove)
            {
                RemoveChild(component); 
            }
            base.AddChild(child);
            
            // Handle notifications
            if (child is Tile)
            {
                if (TileChangedEvent != null)
                {
                    TileChangedEvent(this, new TileChangedEventArgs((Tile)child));
                }
            }
        }

        /// <summary>
        /// Gets the Tile located on this Cell.
        /// </summary>
        /// <returns>The Tile</returns>
        public Tile GetTile()
        {
            foreach (ModelComponent tile in GetChildren())
            {
                if (tile is Tile)
                {
                    return (Tile)tile;
                }
            }
            return null;
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Add game entity to this cell
        /// </summary>
        /// <param name="entity">The Entity to add</param>
        public void AddEntity(ModelComponent entity)
        {
            if (entitiesContainedWithin.Count == 0)
            {
                if (entity is UnitComponent || entity is MapResource || entity is Building)
                {
                    entitiesContainedWithin.Add(entity);

                    if (entity is UnitComponent)
                    {
                        UnitArgs args = new UnitArgs();
                        args.Unit = (UnitComponent)entity;
						args.Unit.UnitAttackedEnemyHanlders += new UnitAttackedEnemyHandler(handleUnitInCellAttackingEnemy);
                        if (UnitAddedEvent != null)
                        {
                            UnitAddedEvent(this, args);
                        }
                    }
                    if (entity is Building)
                    {
                        // do nothing special?
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if this Cell contains any Entities
        /// </summary>
        /// <returns>True if contains any Entities, false otherwise</returns>
        public bool ContainsEntity()
        {
            return !(entitiesContainedWithin.Count == 0);
        }

        /// <summary>
        /// Removes an Entity from this Cell.
        /// </summary>
        /// <param name="entity">The Entity to remove</param>
        public void RemoveEntity(ModelComponent entity)
        {
            entitiesContainedWithin.Remove(entity);
            if (entity is UnitComponent)
            {
                UnitArgs args = new UnitArgs();
                args.Unit = (UnitComponent)entity;
                args.Unit.UnitAttackedEnemyHanlders -= new UnitAttackedEnemyHandler(handleUnitInCellAttackingEnemy);
                if (UnitRemovedEvent != null)
                    UnitRemovedEvent(this, args);
            }
        }

        /// <summary>
        /// Event handler for when a Unit in a Cell is attacking an enemy.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
		private void handleUnitInCellAttackingEnemy(Object obj, UnitAttackedEnemyArgs e)
		{
			//Console.WriteLine("Unit in cell attacked enemy");
		}
    }
}
