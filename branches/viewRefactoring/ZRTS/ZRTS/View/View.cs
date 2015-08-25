using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ZRTSModel.Entities;


namespace ZRTS
{
    /// <summary>
    /// View class
    ///     This class will handle all view-aspects of the game. 
    ///     This class will extract each game object and translate each object's game location into a screen location
    ///     as well as interpret which frame (or animation) to be displayed (or played).
    /// 
    /// Author Nattapol Kemavaha
    /// </summary>
    public class Viewer
    {
        private int cameraWidth, cameraHeight;      // Focused width and height
        private int totalWidth, totalHeight;        // Total width and height of the map
        private float curTime;

        // Drag box
        private float firstCornerX;
        private float firstCornerY;
        private bool isDragging;

        // Graphical/Rendering contents
        private SpriteBatch bufferScreen;           // Where all images are drawn on
        private SpriteSheet spriteTiles;            // Spritesheet of tiles
        private SpriteSheet spriteUnits;            // Spritesheet of units
        private SpriteSheet spriteUtil;             // Spritesheet of utilities and misc. stuff
        private SpriteSheet spriteBuildings;

        // Temporary variable
        private int x, y;
        private Rectangle dragBox;


        // Model content to be extracted
        protected ZRTSModel.GameWorld.GameWorld WorldMap;
        protected ZRTSModel.Scenario.Scenario scenario;


        /// TODO: Implement camera and spritesheet management

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="height">Height of view area</param>
        /// <param name="width">Width of view area</param>
        /// <param name="buffer_Screen">Buffer Screen</param>
        public Viewer(int width, int height, SpriteBatch buffer_Screen)
        {
            this.cameraWidth = width;
            this.cameraHeight = height;
            this.bufferScreen = buffer_Screen;

        }

        /// <summary>
        /// Default constructor (for automated test only)
        /// </summary>
        /// <param name="width">Width of view area</param>
        /// <param name="height">Height of view area</param>
        public Viewer(int width, int height)
        {
            this.cameraHeight = height;
            this.cameraWidth = width;
            this.bufferScreen = null;
        }

        /// <summary>
        /// Getter and setter for isDragging
        /// </summary>
        public bool IsDragging
        {
            get { return this.isDragging; }
            set { this.isDragging = value; }
        }

        /// <summary>
        /// Loading game world object for process
        /// </summary>
        /// <param name="world">target gameworld object</param>
        public void LoadMap(ZRTSModel.GameWorld.GameWorld world)
        {
            this.WorldMap = world;
            this.totalWidth = world.map.width;
            this.totalHeight = world.map.height;
        }

        /// <summary>
        /// Loading game scenario object for process
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScenario(ZRTSModel.Scenario.Scenario scene)
        {
            this.scenario = scene;
        }

        /// <summary>
        /// Load a tile spritesheet
        /// </summary>
        /// <param name="spriteTile">Tile spritesheet</param>
        public void LoadSpriteSheet(SpriteSheet spriteTile)
        {
            this.spriteTiles = spriteTile;
        }

        /// <summary>
        /// Load a unit spritesheet
        /// </summary>
        /// <param name="unitSheet"></param>
        public void LoadUnitsSpriteSheet(SpriteSheet unitSheet)
        {
            this.spriteUnits = unitSheet;
        }

        /// <summary>
        /// Load buildings spritesheet
        /// </summary>
        /// <param name="sheet"></param>
        public void LoadBuildingSpriteSheet(SpriteSheet sheet)
        {
            spriteBuildings = sheet;
        }

        /// <summary>
        /// Load a misc. and utiltity spritesheet
        /// </summary>
        /// <param name="utilSheet"></param>
        public void LoadUtilitySpriteSheet(SpriteSheet utilSheet)
        {
            this.spriteUtil = utilSheet;
            this.dragBox = new Rectangle(-1, -1, 0, 0);
        }
        /// <summary>
        /// Rendering Map's terrain corresponding to gameWorld's terrain tile
        /// </summary>
        private void DrawTerrain()
        {
            for (int row = 0; row < this.WorldMap.map.height; ++row)
            {
                for (int col = 0; col < this.WorldMap.map.width; ++col)
                {

                    // getCells(0,0,w,h)[,] IS working
                    if (this.WorldMap.map.getCells(0, 0, this.WorldMap.map.width, this.WorldMap.map.height)[col, row].isValid == true)

                    // Alternative solution
                    // if it passable tile
                    //if (this.WorldMap.map.getCell(col,row).isValid == true)
                    {
                        this.spriteTiles.drawAtIndex(0, 0, new Vector2(col * GameConfig.TILE_WIDTH, row * GameConfig.TILE_HEIGHT));
                    }
                    else
                    {
                        this.spriteTiles.drawAtIndex(1, 0, new Vector2(col * GameConfig.TILE_WIDTH, row * GameConfig.TILE_HEIGHT));
                    }

                }//for 
            }//for
        }

        /// <summary>
        /// change Location of a main unit (This function will be eliminated once Unit object is well defined.)
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public void changeLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Convert screen location of mouse click to gane location
        /// </summary>
        /// <param name="mouseX">X-mouse click</param>
        /// <param name="mouseY">y-mouse click</param>
        /// <returns>Return (X,Y) game location </returns>
        public Vector2 convertScreenLocToGameLoc(int mouseX, int mouseY)
        {

            float gameLocX = (float)((double)mouseX / GameConfig.TILE_WIDTH);
            float gameLocY = (float)((double)mouseY / GameConfig.TILE_HEIGHT);
            return new Vector2((gameLocX), (gameLocY));
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
        /// Draw all game entities
        /// </summary>
        private void DrawEntities()
        {
            foreach (ZRTSModel.Entities.Unit u in AllUnits())
            {
                //if (isUnitBeingSelected(u))
                //{
                // Draw a highlight unit
                //  this.spriteUtil.drawAtIndex(0,0, new Vector2(translateXScreen(u.x), translateYScreen(u.y)));
                //}
                //this.spriteUnits.drawByAction(0, new Vector2(u.x, u.y));
                this.spriteUnits.drawAtIndex(0, 0, new Vector2(translateXScreen(u.x), translateYScreen(u.y)));

            }
        }

        /// <summary>
        /// Checking if unit is being selected
        /// </summary>
        /// <param name="u">Unit object</param>
        /// <returns>True if it is being selected</returns>
        private bool isUnitBeingSelected(ZRTSModel.Entities.Unit u)
        {
            return this.scenario.getPlayer().SelectedEntities.Count > 0 && this.scenario.getPlayer().SelectedEntities.Contains(u);
        }

        /// <summary>
        /// Translate game Location to screen location for X
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float translateXScreen(float x)
        {
            return x * GameConfig.TILE_WIDTH - (this.spriteUnits.frameDimX / 2);
        }

        /// <summary>
        /// Translate game Location to screen location for Y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private float translateYScreen(float y)
        {
            return y * GameConfig.TILE_HEIGHT - (this.spriteUnits.frameDimY + 5 - GameConfig.TILE_HEIGHT);
        }

        /// <summary>
        /// Translate game location to screen location for x (Building Only) 
        /// Need to move this to dedicated build view
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float translateXScreenBuild(float x)
        {
            return x * GameConfig.TILE_WIDTH;
        }

        private float translateYScreenBuild(float y)
        {
            return y * GameConfig.TILE_HEIGHT;
        }



        /// <summary>
        /// Draw buildings
        /// </summary>
        public void DrawBuildings()
        {
            //System.Console.Out.WriteLine("Building in the world " + this.WorldMap.getBuildings().Count);
            if (this.WorldMap.getBuildings().Count > 0)
            {
                foreach (ZRTSModel.Entities.Building u in this.WorldMap.getBuildings())
                {
                    //System.Console.Out.WriteLine("Draw" + translateXScreen(u.x) + ":" + translateYScreen(u.y));
                    spriteBuildings.drawAtCurrentIndex(new Vector2(translateXScreenBuild(u.orginCell.Xcoord), translateYScreenBuild(u.orginCell.Ycoord)));
                }
            }
        }


        /// <summary>
        /// Setter for firstCorner of the drag box
        /// </summary>
        /// <param name="corner">The screen location of the first click (start of the drag box)</param>
        public void setFirstCornerOfDragBox(float cornerX, float cornerY)
        {
            firstCornerX = cornerX;
            firstCornerY = cornerY;
        }

        /// <summary>
        /// Sets the Drawing Area for the dragBox
        /// </summary>
        /// <param name="x2">x coord of 2nd point</param>
        /// <param name="y2">y coord of 2nd point</param>
        public void setDragBox(float x2, float y2)
        {
            dragBox = new Rectangle(
                (int)Math.Min(firstCornerX, x2),
                (int)Math.Min(firstCornerY, y2),
                (int)(Math.Max(firstCornerX, x2) - Math.Min(firstCornerX, x2)),
                (int)(Math.Max(firstCornerY, y2) - Math.Min(firstCornerY, y2))
                );
        }

        public void resetDragBox()
        {
            dragBox.Width = 0;
            dragBox.Height = 0;
        }

        /// <summary>
        /// Draws the drag box for selection
        /// </summary>
        private void DrawDragBox()
        {
            if (isDragging)
            {
                this.spriteUtil.drawAtIndex(GameConfig.IMG_DRAGBOX, 0, dragBox);
            }
        }


        /// <summary>
        /// Draw everything to the screen
        /// </summary>
        public void Draw()
        {
            bufferScreen.Begin();
            DrawTerrain();
            DrawBuildings();
            DrawEntities();
            DrawDragBox();
            //DrawSelected();
            //bufferScreen.End();
        }

        /// <summary>
        /// Update Time for animation
        /// </summary>
        /// <param name="time">game time</param>
        public void updateTime(float time)
        {
            // TODO: Implement this for future iteration
            this.curTime = time;
        }

    }
}
