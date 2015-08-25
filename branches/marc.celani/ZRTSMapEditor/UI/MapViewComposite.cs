using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;
using ZRTSModel.GameWorld;
using ZRTSModel.EventHandlers;
using System.Collections;
using System.Threading;

namespace ZRTSMapEditor
{
    public partial class MapViewComposite : UserControl
    {
        private MapEditorController controller = null;
        private ScenarioComponent context = null;
        private static int PIXELS_PER_COORDINATE = 20;
        Hashtable realizedComponents = new Hashtable();

        public MapViewComposite()
        {
            InitializeComponent();
        }

        public void Init(MapEditorController controller, MapEditorFullModel model)
        {
            this.controller = controller;
            SetScenario(model.GetScenario());
            model.ScenarioChangedEvent += new ZRTSModel.EventHandlers.ScenarioChangedHandler(ChangeScenario);
        }

        /// <summary>
        /// Called when the scenario has changed.  Unregisters the map view with all pieces of model it was associated with, and registers with
        /// the new scenario.
        /// </summary>
        /// <param name="scenario"></param>
        public void SetScenario(ScenarioComponent scenario)
        {
            context = scenario;

            // Empty the view.
            this.mapPanel.Hide();
            foreach (Control c in mapPanel.Controls)
            {
                c.AllowDrop = false;
                if (c is TileUI)
                    ((TileUI)c).UnregisterFromEvents();
            }
            mapPanel.Controls.Clear();
            realizedComponents.Clear();
            this.mapPanel.Show();

            if (scenario != null)
            {
                ZRTSModel.Map map = scenario.GetGameWorld().GetMap();

                this.mapPanel.Size = new System.Drawing.Size(map.GetWidth() * PIXELS_PER_COORDINATE, map.GetHeight() * PIXELS_PER_COORDINATE);
                
                // Location of the map panel
                int xLoc, yLoc;

                if (this.mapPanel.Size.Width > this.Size.Width)
                {
                    xLoc = 0;
                }
                else
                {
                    // Place the panel in the center.
                    xLoc = (this.Size.Width - this.mapPanel.Size.Width) / 2;
                }

                if (this.mapPanel.Size.Height > this.Size.Height)
                {
                    yLoc = 0;
                }
                else
                {
                    // Place the panel in the center.
                    yLoc = (this.Size.Height - this.mapPanel.Size.Height) / 2;
                }

                this.mapPanel.Location = new System.Drawing.Point(xLoc, yLoc);
                this.RealizeView();

                if (scenario.GetGameWorld().GetPlayerList() != null)
                {
                    scenario.GetGameWorld().GetPlayerList().PlayerAddedEvent += this.PlayerAdded;
                    scenario.GetGameWorld().GetPlayerList().PlayerRemovedEvent += this.PlayerRemoved;
                    foreach (PlayerComponent p in scenario.GetGameWorld().GetPlayerList().GetChildren())
                    {
                        p.BuildingList.BuildingAddedEventHandlers += this.BuildingAdded;
                        p.BuildingList.BuildingRemovedEventHandlers += this.BuildingRemoved;
                    }
                }
            }
            
        }

        private void ChangeScenario(object sender, EventArgs e)
        {
            if (sender is MapEditorFullModel) // sanity check
            {
                SetScenario(((MapEditorFullModel)sender).GetScenario());
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            this.RealizeView();
            base.OnScroll(se);
        }

        private void RealizeView()
        {
            // Get the indices of the upper left hand cell that is visible.
            int xPos = HorizontalScroll.Value / PIXELS_PER_COORDINATE;
            int yPos = this.VerticalScroll.Value / PIXELS_PER_COORDINATE;

            HashSet<Object> componentsToVirtualize = new HashSet<Object>();
            foreach (Object o in realizedComponents.Keys)
            {
                componentsToVirtualize.Add(o);
            }
            mapPanel.SuspendLayout();
            if (context != null)
            {
                ZRTSModel.Map map = context.GetGameWorld().GetMap();
                int maxXPos = xPos + (Width / PIXELS_PER_COORDINATE) + 1;
                int mapMaxX = map.GetWidth();
                int maxX = Math.Min(maxXPos, mapMaxX);
                int maxYPos = yPos + (Height / PIXELS_PER_COORDINATE) + 1;
                int mapMaxY = map.GetHeight();
                int maxY = Math.Min(maxYPos, mapMaxY);
                List<Control> controlsToAdd = new List<Control>();
                for (int i = xPos; i < maxX; i++)
                {
                    for (int j = yPos; j < maxY; j++)
                    {
                        CellComponent cell = map.GetCellAt(i, j);
                        if (!realizedComponents.Contains(cell))
                        {
                            TileUI ui = new TileUI(controller, cell);
                            realizedComponents.Add(cell, ui);

                            ui.Location = new Point(cell.X * PIXELS_PER_COORDINATE, cell.Y * PIXELS_PER_COORDINATE);
                            ui.Size = new Size(PIXELS_PER_COORDINATE, PIXELS_PER_COORDINATE);
                            controlsToAdd.Add(ui);
                        }
                        else
                        {
                            componentsToVirtualize.Remove(cell);
                        }
                        foreach (ModelComponent m in cell.EntitiesContainedWithin)
                        {
                            if (m is Building)
                            {
                                Building building = (Building)m;
                                if (!realizedComponents.Contains(m))
                                {
                                    BuildingUI b = new BuildingUI(controller, building);
                                    b.Location = new Point((int)building.PointLocation.X * PIXELS_PER_COORDINATE, (int)building.PointLocation.Y * PIXELS_PER_COORDINATE);
                                    b.Size = new Size(building.Width * PIXELS_PER_COORDINATE, building.Height * PIXELS_PER_COORDINATE);
                                    realizedComponents.Add(building, b);
                                    // Don't bundle the buildings or else BringToFront won't work
                                    mapPanel.Controls.Add(b);
                                    b.BringToFront();
                                }
                                else
                                {
                                    componentsToVirtualize.Remove(m);
                                }
                            }
                            
                        }
                    }
                }
                mapPanel.Controls.AddRange(controlsToAdd.ToArray());
                foreach (Object o in componentsToVirtualize)
                {
                    Control c = (Control)realizedComponents[o];
                    if (c is TileUI)
                    {
                        // TileUIs are set up to allow drop - turn this off so that the handle to it is destroyed and so the tileUI can be destroyed by the GC.
                        c.AllowDrop = false;
                        ((TileUI)c).UnregisterFromEvents();
                    }
                    mapPanel.Controls.Remove(c);
                    realizedComponents.Remove(o);
                }
                mapPanel.ResumeLayout();
            }
        }

        private void BuildingAdded(Object sender, BuildingAddedEventArgs args)
        {
            BuildingUI b = new BuildingUI(controller, args.Building);
            b.Location = new Point((int) args.Building.PointLocation.X * PIXELS_PER_COORDINATE, (int)args.Building.PointLocation.Y * PIXELS_PER_COORDINATE);
            b.Size = new Size(args.Building.Width * PIXELS_PER_COORDINATE, args.Building.Height * PIXELS_PER_COORDINATE);
            realizedComponents.Add(args.Building, b);
            mapPanel.Controls.Add(b);
            b.BringToFront();
        }

        private void BuildingRemoved(Object sender, BuildingAddedEventArgs args)
        {
            Control c = (Control)realizedComponents[args.Building];
            c.Dispose();
            realizedComponents.Remove(args.Building);
        }

        private void PlayerAdded(Object sender, PlayerListChangedEventArgs args)
        {
            if (args != null) {
            foreach (PlayerComponent p in args.PlayersAddedOrRemoved)
            {
                p.BuildingList.BuildingAddedEventHandlers += this.BuildingAdded;
                p.BuildingList.BuildingRemovedEventHandlers += this.BuildingRemoved;
            }
        }
        }

        private void PlayerRemoved(Object sender, PlayerListChangedEventArgs args)
        {
            if (args != null)
            {
                foreach (PlayerComponent p in args.PlayersAddedOrRemoved)
                {
                    List<ModelComponent> copyList = new List<ModelComponent>();
                    copyList.AddRange(p.BuildingList.GetChildren());
                    foreach (Building b in copyList)
                    {
                        p.BuildingList.RemoveChild(b);
                    }
                    p.BuildingList.BuildingAddedEventHandlers -= this.BuildingAdded;
                    p.BuildingList.BuildingRemovedEventHandlers -= this.BuildingRemoved;

                    copyList = new List<ModelComponent>();
                    copyList.AddRange(p.GetUnitList().GetChildren());
                    foreach (UnitComponent u in copyList)
                    {
                        p.GetUnitList().RemoveChild(u);
                        u.Location.RemoveEntity(u);
                    }
                }
            }
        }
    }
}
