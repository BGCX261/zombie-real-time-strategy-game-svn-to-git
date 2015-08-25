using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTSModel.GameModel;
using ZRTSModel.EventHandlers;
using ZRTSModel;
using ZRTS.XnaCompositeView.SelectionViewVisitors;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// SelectionView: Represent a main panel where stat is displayed.
    /// This class act as a user interface component, which will show stat of selected units by the player and activate appropriate command menu
    /// based on a type of the selected units.
    /// </summary>
    public class SelectionView : XnaUIComponent
    {
        private SelectionState selectionState;      // State of selected units by the user
        private PictureBox mainBgPanel;             // background of the panel
        public CommandView commandBar;              // Coordinate with commandBar


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="selectionState"></param>
        public SelectionView(Game game, SelectionState selectionState)
            : base(game)
        {
            this.selectionState = selectionState;
            selectionState.SelectionStateChanged += onSelectionChanged;
            mainBgPanel = new PictureBox(game, new Rectangle(GameConfig.MAINPANEL_START_X, GameConfig.MAINPANEL_START_Y, 730, 200));
            mainBgPanel.DrawBox = new Rectangle(0, 0, 730, 200);
            AddChild(mainBgPanel);
        }
        
        /// <summary>
        /// Draw stat on the screen
        /// </summary>
        /// <param name="e"></param>
        protected override void onDraw(XnaDrawArgs e)
        {
            // For test purposes only.
			Texture2D pixel = new Texture2D(e.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White }); 
        }



        /// <summary>
        /// Update and refresh holder every time multiple new units are selected
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="e"></param>
        private void updateHolder(SameSizeChildrenFlowLayout holder, SelectionStateChangedArgs e)
        {
            int count = e.SelectedEntities.Count;
            if (holder != null)
            {
                holder.Clear();
                if (count > 1)
                {
                    holder.Visible = true;
                    foreach (ModelComponent component in e.SelectedEntities)
                    {
                        BuildSelectedEntityUIVisitor visitor = new BuildSelectedEntityUIVisitor();
                        if (component is UnitComponent)
                        {
                            UnitComponent temp = (UnitComponent)component;

                            component.Accept(visitor);
                            if (visitor.UI != null)
                            {
                                holder.AddChild(visitor.UI);
                            }

                        }
                    }//for
                }//if
            }//if
        }

        /// <summary>
        /// Flushing Stat panel of selected unit(s)
        /// </summary>
        private void flushStatPanel()
        {
            // Remove current big picture box
            for (int i = 0; i < GetChildren().Count; i++)
            {
                if (GetChildren()[i] is PictureBox && GetChildren()[i] != mainBgPanel)
                {
                    GetChildren()[i].Dispose();
                    RemoveChild(GetChildren()[i]);
                    i--;
                }
            }
        }

        /// <summary>
        /// Show player's selected units stat and activate command for the units
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="e"></param>
        private void showUnitStatAndCommand(SameSizeChildrenFlowLayout holder, SelectionStateChangedArgs e, BuildLargePreviewPictureBoxVisitor visitor)
        {
            int count = e.SelectedEntities.Count;
            // Only non-zombie stat being displayed
            if (!((UnitComponent)e.SelectedEntities[0]).IsZombie)
            {
                // Individual unit
                if (count == 1 && holder != null)
                {
                    // Use the holder to show unit stats.
                    DisplaySelectedEntityStatsVisitor visitor2 = new DisplaySelectedEntityStatsVisitor();
                    visitor2.Game = (XnaUITestGame)Game;
                    visitor2.Layout = holder;
                    e.SelectedEntities[0].Accept(visitor2);
                }

                e.SelectedEntities[0].Accept(visitor);
                PictureBox bigImage = visitor.PictureBox;

                bigImage.DrawBox = new Rectangle(25, 25, 150, 150);
                AddChild(bigImage);

                bool containsNonBuilders = true;
                for (int i = 0; i < e.SelectedEntities.Count; i++)
                {
                    if (!((UnitComponent)e.SelectedEntities[0]).CanBuild)
                    {
                        containsNonBuilders = false;
                        break;
                    }
                }
                commandBar.activateButtons(containsNonBuilders);  // show commandView if selected

            } // only non zombie
        }


        /// <summary>
        /// Update Stat panel and activate command panel for selected units
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="e"></param>
        private void updateStatAndCommandPanels(SameSizeChildrenFlowLayout holder, SelectionStateChangedArgs e)
        {
            int count = e.SelectedEntities.Count;
            if (count > 0)
            {
                BuildLargePreviewPictureBoxVisitor visitor = new BuildLargePreviewPictureBoxVisitor();

                if (e.SelectedEntities[0] is UnitComponent)
                {
                    showUnitStatAndCommand(holder, e, visitor);
                } // unit
                else if (e.SelectedEntities[0] is Building)
                {
                    Building y = (Building)e.SelectedEntities[0];
                    commandBar.activateProduceUnitButtons(y.Type);
                }
            }
            else
            {
                commandBar.disableButtons();
                commandBar.deactivateProduceUnitButtons();
            }
        }



        /// <summary>
        /// Event handler when selection of units has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onSelectionChanged(Object sender, SelectionStateChangedArgs e)
        {
            
            int count = e.SelectedEntities.Count;
            SameSizeChildrenFlowLayout holder = getSelectedEntityUIHolder();

            updateHolder(holder, e);    // Update the holder
            flushStatPanel();           // Clear all stat panels
            updateStatAndCommandPanels(holder,e);  // Update panel and activate new command menu

            
        }

        private SameSizeChildrenFlowLayout getSelectedEntityUIHolder()
        {
            SameSizeChildrenFlowLayout holder = null;
            foreach (XnaUIComponent component in GetChildren())
            {
                if (component is SameSizeChildrenFlowLayout)
                {
                   
                    holder = (SameSizeChildrenFlowLayout)component;
                    break;
                }
            }
            return holder;
        }
    }
}
