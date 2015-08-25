using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;

namespace ZRTS
{
    /// <summary>
    /// View Observer
    /// </summary>
    public class ViewObserver:ZRTSModel.Scenario.Observer
    {

        /// <summary>
        /// Notify of update
        /// </summary>
        public override void notify()
        {
            //these should be passed in or something
            GameWorld gameWorld = new GameWorld(20, 20);
            int x1, x2, y1, y2;    //boundary includes 1 not 2
            x1 = 0; x2 = 0; y1 = 0; y2 = 0;


            // Do some update on View
            //(1) Find the way to get all units within the boundary (all selected units)
            Cell c;
            Unit s;
            List<Unit> selected = new List<Unit>();

            for(int x = x1; x<x2; x++)
            {
                for(int y=y1; y<y2; y++)
                {
                    c = gameWorld.map.cells[x, y];
                    s = c.getUnit();
                    //if (e == null)
                      //  e = c.entity;
                    //if (s!=null && s.getOwner()==)
                        //selected.Add(s);
                }
            }

            //(2) Notify draw in the view object
            // view.DrawSelected(selected);


            // (3) Look at View drawSelected()
        }
    }
}