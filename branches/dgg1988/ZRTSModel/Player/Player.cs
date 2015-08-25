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
        List<Entity> entities;
        private List<Entity> selected_entities;
        public int[] player_resources; //0: Water, 1: Lumber, 2: Food, 3: Metal
		private List<Player> enemies;

        public List<Entity> SelectedEntities
        {
            get { return this.selected_entities; }
        }


        public Player(byte id)
        {
            this.id = id;
            entities = new List<Entity>();
            selected_entities = new List<Entity>();
            player_resources = new int[4];
			enemies = new List<Player>();
        }

        public void insertEntity(ZRTSModel.Entities.Entity entity)
        {
            entities.Add(entity);
        }

        public void removeEntity(ZRTSModel.Entities.Entity entity)
        {
            entities.Remove(entity);
        }

		public bool hasEntity(Entity entity)
		{
			return entities.Contains(entity);
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

		public void addEnemy(Player enemy)
		{
			enemies.Add(enemy);
		}

		public void removeEnemy(Player enemy)
		{
			enemies.Remove(enemy);
		}

		public bool isEnemy(Player enemy)
		{
			return enemies.Contains(enemy);
		}
    }
}
