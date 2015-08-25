using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

namespace ZRTSModel.Player
{
    [Serializable()]
    public class Player
    {
        byte id;
        List<Unit> units;
        List<Building> buildings;
        List<Entity> entities;
        private List<Entity> selected_entities;

        public List<Entity> SelectedEntities
        {
            get { return this.selected_entities; }
        }


        public Player(byte id)
        {
            this.id = id;
            units = new List<Unit>();
            buildings = new List<Building>();
            entities = new List<Entity>();
            selected_entities = new List<Entity>();
        }

        public void insertEntity(ZRTSModel.Entities.Entity entity)
        {
            entities.Add(entity);
        }

        public void removeEntity(ZRTSModel.Entities.Entity entity)
        {
            entities.Remove(entity);
        }


        public void selectEntities(List<ZRTSModel.Entities.Entity> list)
        {
            selected_entities = list;
        }


        public void selectEntity(ZRTSModel.Entities.Entity entity)
        {
            selected_entities.Add(entity);
        }

        public void unselectEntity(ZRTSModel.Entities.Entity entity)
        {
            selected_entities.Remove(entity);
        }
    }
}
