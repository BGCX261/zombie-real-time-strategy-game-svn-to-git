using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZRTSModel.Entities;
using ZRTSModel;
using ZRTSModel.GameModel;
using ZRTS.XnaCompositeView;
using ZRTS.XnaCompositeView.MapViewVisitors;
using Microsoft.Xna.Framework.Input;
using ZRTSModel.EventHandlers;
using System.Collections;
using ZRTS.InputEngines;


namespace ZRTS
{
    /// <summary>
    /// View class
    ///     This class will handle all view-aspects of the game. 
    ///     This class will extract each game object and translate each object's game location into a screen location
    ///     as well as interpret which frame (or animation) to be displayed (or played).
    /// 
    /// Original Author Nattapol Kemavaha
    /// Modifier/Author Marc Celani
    /// </summary>
    public class MapView : XnaUIComponent
    {
        // Drag box UI reference (null means we are not dragging)
        private TestUIComponent dragBox = null;
        // First point in the dragging, used when the mouse moves to move the dragBox.
        private Point startSelectionBoxPoint;
        private Hashtable componentToUI = new Hashtable();
        private MapViewLeftButtonStrategy leftButtonStrategy;

        public MapViewLeftButtonStrategy LeftButtonStrategy
        {
            get { return leftButtonStrategy; }
            set { leftButtonStrategy = value; }
        }

        private static int cellDimension = GameConfig.TILE_DIM;         // Dimension of each cell (square pixel)

        public static int CellDimension
        {
            get { return MapView.cellDimension; }
        }
        private static int SCROLL_SPEED = 10;

        public MapView(Game game)
            : base(game)
        {
            PlayerList players = ((XnaUITestGame)game).Model.GetScenario().GetGameWorld().GetPlayerList();
            foreach (PlayerComponent player in players.GetChildren())
            {
                UnitList unitList = player.GetUnitList();
                unitList.UnitAddedEvent += onUnitAdded;
                unitList.UnitRemovedEvent += onUnitRemoved;
                ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
                foreach (UnitComponent unit in unitList.GetChildren())
                {
                    UnitUI unitUI = factory.BuildUnitUI(unit);
                    unitUI.DrawBox = new Rectangle((int)(unit.PointLocation.X * cellDimension), (int)(unit.PointLocation.Y * cellDimension), unitUI.DrawBox.Width, unitUI.DrawBox.Height);
                    AddChild(unitUI);
                    componentToUI.Add(unit, unitUI);
                    unit.MovedEventHandlers += updateLocationOfUnit;
                    unit.HPChangedEventHandlers += killUnit;
                }
                BuildingList buildingList = player.BuildingList;
                foreach (Building b in buildingList.GetChildren())
                {
                    BuildingUI buildingUI = factory.BuildBuildingUI(b);
                    buildingUI.DrawBox = new Rectangle((int)b.PointLocation.X * cellDimension, (int)b.PointLocation.Y * cellDimension, buildingUI.DrawBox.Width, buildingUI.DrawBox.Height);
                    AddChild(buildingUI);
                }
                buildingList.BuildingAddedEventHandlers += this.onBuildingAdded;
            }
            leftButtonStrategy = new DrawSelectionBoxStrategy(this);
            OnClick += moveSelectedUnits;
        }

        /// <summary>
        /// Trigger function in the event of selected unit is moving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveSelectedUnits(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled && e.ButtonPressed == MouseButton.Right)
            {
                PointF gamePoint = new PointF((float)(e.ClickLocation.X + ScrollX) / (float)cellDimension, (float)(e.ClickLocation.Y + ScrollY) / (float)cellDimension);
                ((XnaUITestGame)Game).Controller.MoveSelectedUnitsToPoint(gamePoint);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Trigger function in the event of selected unit is told to harvest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tellSelectedUnitsToHarvestAt(object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled && e.ButtonPressed == MouseButton.Right)
            {
                PointF gamePoint = new PointF((float)(e.ClickLocation.X + ScrollX) / (float)cellDimension, (float)(e.ClickLocation.Y + ScrollY) / (float)cellDimension);
                ((XnaUITestGame)Game).Controller.TellSelectedUnitsToHarvest(gamePoint);
                e.Handled = true;
            }
        }

        protected override void onDraw(XnaDrawArgs e)
        {
            // Determine all of the cells in view
            ZRTSModel.Map map = ((XnaUITestGame)Game).Model.GetScenario().GetGameWorld().GetMap();
            Point upperLeftCell = new Point(ScrollX / CellDimension, ScrollY / CellDimension);
            Point lowerRightCell = new Point(Math.Min((ScrollX + DrawBox.Width) / CellDimension, map.GetWidth() - 1), Math.Min((ScrollY + DrawBox.Height) / CellDimension, map.GetHeight() - 1));
            Point offset = new Point(ScrollX % CellDimension, ScrollY % CellDimension);
            
            // Draw all of the tiles
            for (int x = upperLeftCell.X; x <= lowerRightCell.X; x++)
            {
                for (int y = upperLeftCell.Y; y <= lowerRightCell.Y; y++)
                {
                    CellComponent cell = map.GetCellAt(x, y);
                    Tile tile = cell.GetTile();
                    DrawTileVisitor drawer = new DrawTileVisitor(e.SpriteBatch, ((XnaUITestGame)Game).SpriteSheet, new Rectangle((x - upperLeftCell.X) * CellDimension - offset.X, (y - upperLeftCell.Y) * CellDimension - offset.Y, CellDimension, CellDimension));
                    tile.Accept(drawer);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            handleScrolling();
            handleMouse();
            base.Update(gameTime);
        }

        /// <summary>
        /// Handle mouse click on the game screen
        /// </summary>
        private void handleMouse()
        {
            MouseState mouseState = Mouse.GetState();
            XnaUIComponent target = ((XnaUITestGame)Game).MouseInputEngine.GetTarget(new Point(mouseState.X, mouseState.Y));
            bool within = false;
            while (target != null)
            {
                if (target == this)
                {
                    within = true;
                    break;
                }
                target = target.Parent;
            }
            if (within)
            {
                Point mousePoint = new Point(ScrollX + mouseState.X, ScrollY + mouseState.Y);
                leftButtonStrategy.HandleMouseInput(mouseState.LeftButton == ButtonState.Pressed, mouseState.RightButton == ButtonState.Pressed, mousePoint);
            }
            else
            {
                leftButtonStrategy.CancelProgress();
            }
        }

        /// <summary>
        /// Handle scrolling of the game view
        /// </summary>
        private void handleScrolling()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Map map = ((XnaUITestGame)Game).Model.GetScenario().GetGameWorld().GetMap();
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                ScrollX = Math.Max(ScrollX - SCROLL_SPEED, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                ScrollX = Math.Min(ScrollX + SCROLL_SPEED, map.GetWidth() * CellDimension - DrawBox.Width);
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                ScrollY = Math.Max(ScrollY - SCROLL_SPEED, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                ScrollY = Math.Min(ScrollY + SCROLL_SPEED, map.GetHeight() * CellDimension - DrawBox.Height);
            }
        }

        /// <summary>
        /// Trigger fucntion in the event of new unit being added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUnitAdded(object sender, UnitAddedEventArgs e)
        {
            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            UnitUI unitUI = factory.BuildUnitUI(e.Unit);
            unitUI.DrawBox = new Rectangle((int)(e.Unit.PointLocation.X * cellDimension), (int)(e.Unit.PointLocation.Y * cellDimension), unitUI.DrawBox.Width, unitUI.DrawBox.Height); 
            AddChild(unitUI);
            componentToUI.Add(e.Unit, unitUI);
            e.Unit.MovedEventHandlers += updateLocationOfUnit;
            e.Unit.HPChangedEventHandlers += killUnit;
        }

        /// <summary>
        /// Trigger function in the event of unit being removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUnitRemoved(object sender, UnitRemovedEventArgs e)
        {
            /**
            UnitUI component = (UnitUI)componentToUI[e.Unit];
            component.Dispose();
            RemoveChild(component);
            componentToUI.Remove(e.Unit);
             * */
            e.Unit.MovedEventHandlers -= updateLocationOfUnit;
            e.Unit.HPChangedEventHandlers -= killUnit;
          
        }

        /// <summary>
        /// Trigger funciton in the event of a new building being added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onBuildingAdded(Object sender, BuildingAddedEventArgs e)
        {
            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            BuildingUI buildingUI = factory.BuildBuildingUI(e.Building);
            buildingUI.DrawBox = new Rectangle((int)e.Building.PointLocation.X * cellDimension, (int)e.Building.PointLocation.Y * cellDimension, buildingUI.DrawBox.Width, buildingUI.DrawBox.Height);
            AddChild(buildingUI);
        }

        /// <summary>
        /// Trigger function in the event of unit moving across the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateLocationOfUnit(Object sender, UnitMovedEventArgs e)
        {
            UnitUI ui = componentToUI[e.Unit] as UnitUI;
            ui.DrawBox = new Rectangle((int)(e.NewPoint.X * cellDimension) - ui.DrawBox.Width/2, (int)(e.NewPoint.Y * cellDimension)-ui.DrawBox.Height/2, ui.DrawBox.Width, ui.DrawBox.Height);
        }

        /// <summary>
        /// Trigger function in the event of unit's health point has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void killUnit(Object sender, UnitHPChangedEventArgs e)
        {
            
            if (e.NewHP <= 0)
            {
                // Do not remove the unit until the animation is done for at least 3 seconds
                //System.Console.Out.WriteLine("Kill Unit");
                /**
                UnitUI ui = (UnitUI)componentToUI[e.Unit];
                
                ui.Dispose();
                
                RemoveChild(ui);
                **/
                e.Unit.MovedEventHandlers -= updateLocationOfUnit;
                e.Unit.HPChangedEventHandlers -= killUnit;
                
            }
            
           
        }
    }
}
