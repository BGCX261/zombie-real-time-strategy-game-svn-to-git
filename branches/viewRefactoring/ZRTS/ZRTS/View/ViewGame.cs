using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTS.View
{
    /// <summary>
    /// viewGame
    /// Base:View
    /// 
    /// This class will handle graphical contents of all game components regardless of user's interaction with the unit.
    /// </summary>
    public class ViewGame:ViewAbstract, ViewScreenConvert
    {

        

        // Scrolling Offset values
        private int offsetX, offsetY;               // Offset for scrolling

        // Temporary Location value (optimization)
        private Microsoft.Xna.Framework.Vector2 tempLoc;
        private ZRTSModel.GameWorld.Cell[,] tempCell;

        private bool isScroll = true;               // Flag for checking if the view has been scrolled at all

        /// Graphic contents
        private SpriteSheet buildingSheet;          // Spritesheet of building
        private SpriteSheet unitSheet;              // Spritesheet of units

        // Model content to be extracted
        protected ZRTSModel.GameWorld.GameWorld WorldMap;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ViewGame()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Widht of the View screen</param>
        /// <param name="height">Height of the view screen</param>
        public ViewGame(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Microsoft.Xna.Framework.Vector2 convertScreenLocToGameLoc(int x, int y)
        {
            return Microsoft.Xna.Framework.Vector2.Zero;
        }

        /// <summary>
        /// Load Gameworld model
        /// </summary>
        /// <param name="map">Gameworld map</param>
        public void loadGameWorld(ZRTSModel.GameWorld.GameWorld map)
        {
            WorldMap = map;
        }


        /// <summary>
        /// Load terrain spritesheet
        /// </summary>
        /// <param name="sheet"></param>
        public override void loadSheet(SpriteSheet sheet)
        {
            base.loadSheet(sheet);
        }

        /// <summary>
        /// Load building spritesheet
        /// </summary>
        /// <param name="sheet"></param>
        public void loadBuildingSheet(SpriteSheet sheet)
        {
            this.buildingSheet = sheet;
        }

        /// <summary>
        /// Load Units spritesheet
        /// </summary>
        /// <param name="sheet"></param>
        public void loadUnitSheet(SpriteSheet sheet)
        {
            this.unitSheet = sheet;
        }

        public void scrollUp()
        {
            isScroll = true;
            //TODO: Implement this
        }

        public void scrollDown()
        {
            isScroll = true;
            //TODO: Implement this
        }

        public void scrollLeft()
        {
            isScroll = true;
            //TODO: Implement this
        }

        public void scrollRight()
        {
            isScroll = true;
            //TODO: Implement this
        }

        /// <summary>
        /// Draw all terrains of the view of the map
        /// </summary>
        private void drawTerrains()
        {
            if (isScroll)   // Optimization (prevent new allocation everytime getCells() is called)
            {
                tempCell = this.WorldMap.map.getCells(0, 0, this.WorldMap.map.width, this.WorldMap.map.height);
                //WARNING!!!!!!!!!!!!!! getCells use game Location not screen Location!!!!!!!!!!!!!!

            }

            for (int row = 0; row < this.tempCell.GetLength(1); ++row)
            {
                for (int col = 0; col < this.tempCell.GetLength(0); ++col)
                {
                    tempLoc.X = translateXScreen(col);
                    tempLoc.Y = translateYScreen(row);

                    // TODO: Check for tile type instead of checking for valid tile
                    if (tempCell[col, row].isValid == true)
                    {
                        this.sheet.drawAtIndex(0, 0, tempLoc);
                    }
                    else
                    {
                        this.sheet.drawAtIndex(1, 0, tempLoc);
                    }

                }//for 
            }//for

            isScroll = false;
        }


        /// <summary>
        /// Getting all units in the gameworld map
        /// </summary>
        /// <returns>List of entities in the map</returns>
        private List<ZRTSModel.Entities.Unit> AllUnits()
        {
            return this.WorldMap.getUnits();
        }

        /// <summary>
        /// Draw all units of the view
        /// </summary>
        private void drawEntities()
        {
            foreach (ZRTSModel.GameWorld.Cell u in tempCell)
            {
                if (u.getUnit() != null)
                {
                    tempLoc.X = translateXScreen(u.getUnit().x);
                    tempLoc.Y = translateYScreen(u.getUnit().y);

                    this.unitSheet.drawAtIndex(0, 0, tempLoc);
                }

            }
        }

        /// <summary>
        /// Draw all buildings of the view
        /// </summary>
        private void drawBuildings()
        {
            //Todo
        }

        /// <summary>
        /// Draw all game contents within View's boundary
        /// </summary>
        public override void Draw()
        {
           
            drawTerrains();
            drawBuildings();
            drawEntities();
        }
    }
}
