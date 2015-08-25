using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;
using Microsoft.Xna.Framework.Graphics;
namespace ZRTS
{
    /// <summary>
    /// Implements ViewSelectObserver
    ///     Views the Units selected 
    ///     This view class will focus on the selected units by the user.
    /// 
    /// Observer Pattern
    /// </summary>
    public class ViewSelect:ZRTSModel.Scenario.ViewSelectObserver
    {

        protected List<Entity> selectedList;      //List of selected Units
        protected SpriteSheet utilSheet;          // Spritesheet for utilities graphic

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewSelect()
        {
            this.selectedList = new List<Entity>();
        }

        /// <summary>
        /// Append the param List to the current selectedList
        /// </summary>
        /// <param name="add"></param>
        public void addListOfUnits(List<Entity> add)
        {
            this.selectedList.AddRange(add);
        }

        /// <summary>
        /// Add a single unit to the selectedList
        /// </summary>
        /// <param name="u"></param>
        public void addUnit(Entity u)
        {
            this.selectedList.Add(u);
        }

        
        /// <summary>
        /// Removes everything from selectedList
        /// </summary>
        public void removeEverything()
        {
            this.selectedList.Clear();
        }
        
        /// <summary>
        /// Removes the first occurance of param from selectedList
        /// </summary>
        /// <param name="u"></param>
        public void removeUnit(Entity u)
        {
            this.selectedList.Remove(u);
        }

        /// <summary>
        /// Getter and setter for selectedList
        /// </summary>
        public List<Entity> getSelectedUnits
        { get { return this.selectedList; } set { this.selectedList = value; } }


        /// <summary>
        /// Load a new spritesheet
        /// </summary>
        /// <param name="sheet"></param>
        public void loadSpriteSheet(ZRTS.SpriteSheet sheet)
        {
            this.utilSheet = sheet;
        }

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            foreach (ZRTSModel.Entities.Entity e in selectedList)
            {
                utilSheet.drawAtIndex(0, 0, new Microsoft.Xna.Framework.Vector2(translateXScreen(e.x), translateYScreen(e.y)));
            }
        }


        /// <summary>
        /// Translate game Location to screen location for X
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float translateXScreen(float x)
        {
            return x * GameConfig.TILE_WIDTH - (this.utilSheet.frameDimX / 2);
        }

        /// <summary>
        /// Translate game Location to screen locaiton for Y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private float translateYScreen(float y)
        {
            return y * GameConfig.TILE_HEIGHT - (this.utilSheet.frameDimY + 14 /* figure out over here*/ - GameConfig.TILE_HEIGHT);
        }

        /// <summary>
        /// Check if the unit has already been selected
        /// </summary>
        /// <param name="e">Entity</param>
        /// <returns>True if it is already in the selected list</returns>
        public bool hasSelectedUnitBefore(Entity e)
        {
            return this.getSelectedUnits.Contains(e);
        }

        /// <summary>
        /// Update
        /// </summary>
        public void update() 
        {
            //System.Console.Out.WriteLine("View Select is notifed!");
        }
    }
}
