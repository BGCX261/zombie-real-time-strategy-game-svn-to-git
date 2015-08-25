using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Implements ViewSelectObserver
    /// Views the Units selected 
    /// 
    /// Observer Pattern
    /// </summary>
    class ViewSelect
    {

        protected List<Unit> selectedList;      //List of selected Units

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewSelect()
        {
            this.selectedList = new List<Unit>();
        }

        /// <summary>
        /// Append the param List to the current selectedList
        /// </summary>
        /// <param name="add"></param>
        public void addListOfUnits(List<Unit> add)
        {
            this.selectedList.AddRange(add);
        }

        /// <summary>
        /// Add a single unit to the selectedList
        /// </summary>
        /// <param name="u"></param>
        public void addUnit(Unit u)
        {
            this.selectedList.Add(u);
        }

        
        /// <summary>
        /// Removes everything from selectedList
        /// </summary>
        public void removeEverything()
        {
            this.selectedList.RemoveRange(0, this.selectedList.Count - 1);
        }
        
        /// <summary>
        /// Removes the first occurance of param from selectedList
        /// </summary>
        /// <param name="u"></param>
        public void removeUnit(Unit u)
        {
            this.selectedList.Remove(u);
        }

        /// <summary>
        /// Getter and setter for selectedList
        /// </summary>
        public List<Unit> getSelectedUnits
        { get { return this.selectedList; } set { this.selectedList = value; } }
    }
}
