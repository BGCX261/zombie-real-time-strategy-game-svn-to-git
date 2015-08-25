using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTS.InputEngines;

namespace ZRTS.XnaCompositeView
{
    public class BuildBuildingMapViewLeftClickStrategy : MapViewLeftButtonStrategy
    {
        private MapView mapView;
        private PictureBox building;
        private bool added = false;
        private bool currentLocationIsOkay;
        private string buildingType;

        public BuildBuildingMapViewLeftClickStrategy(MapView mapView, string buildingType)
        {
            this.mapView = mapView;
            this.buildingType = buildingType;
            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            building = factory.BuildPictureBox(buildingType, "mapView");
            int width = ZRTSModel.Factories.BuildingFactory.Instance.getStats(buildingType).width;
			int height = ZRTSModel.Factories.BuildingFactory.Instance.getStats(buildingType).height;

            // TODO: replace "2" with stats from the building factory.
            building.DrawBox = new Microsoft.Xna.Framework.Rectangle(0, 0,  width * GameConfig.TILE_DIM, height * GameConfig.TILE_DIM);
            building.OnClick += placeBuilding;
        }

        public void HandleMouseInput(bool leftButtonPressed, bool rightButtonPressed, Point mouseLocation)
        {
            // We are hovering over the map.  Update its location.
            Point drawPoint = new Point();

            // The draw point should be on a grid line intersection, and should not be outside the bounds of the map.
            drawPoint.X = Math.Max(0, mouseLocation.X - (mouseLocation.X % MapView.CellDimension));
            drawPoint.X = Math.Min(drawPoint.X, ((XnaUITestGame)mapView.Game).Model.GetScenario().GetGameWorld().GetMap().GetWidth() * MapView.CellDimension - building.DrawBox.Width);

            drawPoint.Y = Math.Max(0, mouseLocation.Y - (mouseLocation.Y % MapView.CellDimension));
            drawPoint.Y = Math.Min(drawPoint.Y, ((XnaUITestGame)mapView.Game).Model.GetScenario().GetGameWorld().GetMap().GetHeight() * MapView.CellDimension - building.DrawBox.Height);

            building.DrawBox = new Rectangle(drawPoint.X, drawPoint.Y, building.DrawBox.Width, building.DrawBox.Height);
            if (!added)
            {
                mapView.AddChild(building);
                added = true;
            }

            // Check if the cells are taken.  If so, tint the drawing red, otherwise tint it green.
            // BUG: This only checks the upper left cell - we have to check all the cells.
            if (((XnaUITestGame)mapView.Game).Controller.CellsArePassable(building.DrawBox.X / MapView.CellDimension, building.DrawBox.Y / MapView.CellDimension, building.DrawBox.Width / MapView.CellDimension, building.DrawBox.Height / MapView.CellDimension) && ((XnaUITestGame)mapView.Game).Controller.CellsAreEmpty(building.DrawBox.X / MapView.CellDimension, building.DrawBox.Y / MapView.CellDimension, building.DrawBox.Width / MapView.CellDimension, building.DrawBox.Height / MapView.CellDimension))
            {
                currentLocationIsOkay = true;
                building.Tint = new Color(0, 125, 0, 0);
            }
            else
            {
                currentLocationIsOkay = false;
                building.Tint = new Color(125, 0, 0, 0);
            }
        }

        public void CancelProgress()
        {
            if (added)
            {
                mapView.RemoveChild(building);
                added = false;
                changeLeftClickStrategy();
            }
        }

        private void changeLeftClickStrategy()
        {
            mapView.LeftButtonStrategy = new DrawSelectionBoxStrategy(mapView);
        }

        private void placeBuilding(Object sender, XnaMouseEventArgs e)
        {
            if (building.Tint.Equals(new Color(0, 125, 0, 0)))
            {
                mapView.RemoveChild(building);
                building.OnClick -= placeBuilding;
                Point drawPoint = new Point();
                drawPoint.X = building.DrawBox.X / MapView.CellDimension;
                drawPoint.Y = building.DrawBox.Y / MapView.CellDimension;
                ((XnaUITestGame)mapView.Game).Controller.TellSelectedUnitsToBuildAt(buildingType, drawPoint);
                changeLeftClickStrategy();
            }
        }
    }
}
