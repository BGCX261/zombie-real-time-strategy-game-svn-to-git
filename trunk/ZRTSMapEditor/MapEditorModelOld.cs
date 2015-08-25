using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Scenario;
using ZRTSModel.GameWorld;

namespace ZRTSMapEditor
{
    public enum SelectionType
    {
        None,
        Tile,
        EntityToAdd,
        EntitiesInMap
    }
    public class MapEditorModelOld
    {
        public Scenario scenario = null;
        // public Queue<MapEditorCommand> commands = new Queue<MapEditorCommand>();
        public bool saved = true;
        public String filename = null;
        public SelectionType SelectionType = SelectionType.None;
        public String TileTypeSelected;
        // public Entity entityTypeSelected = null;
        // public ArrayList<Entity> EntitiesInMapSelected = null;
        private List<MapEditorModelListener> listeners = new List<MapEditorModelListener>();

        public void register(MapEditorModelListener listener)
        {
            listeners.Add(listener);
        }

        public void unregister(MapEditorModelListener listener)
        {
            listeners.Remove(listener);
        }

        public void notifyAll()
        {
            foreach (MapEditorModelListener l in listeners)
            {
                l.notify(this);
            }
        }
    }
}
