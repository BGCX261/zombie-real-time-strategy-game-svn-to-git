using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class CellComponent : ModelComponent
    {
        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is CellVisitor)
            {
                CellVisitor cellVisitor = (CellVisitor)visitor;
                cellVisitor.Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }

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
                    }
                }
            }
            foreach (ModelComponent component in toRemove)
            {
                // Called in this manner to avoid the NotifyAll in RemoveChild().
                GetChildren().Remove(component);
                
            }
            // Handles the nofity.
            base.AddChild(child);
        }

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
    }
}
