using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using ZRTSModel.GameWorld;

namespace ZRTS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZRTSGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState input;
        MouseState prevInput;
        SpriteSheet sample_image , sample_tile, sample_util;
        View gameView;
        ViewSelect gameSelectView;

        SpriteFont Font1;

        // x and y coords in game
        float commandX, commandY;
        float selectX, selectY;

        ZRTSModel.Scenario.Scenario testScenario;
        ZRTSLogic.Controller testGameController;
        
        /// This is how to use object from other project (SPIKE)
        Map testMap = new Map(20, 20);

        public ZRTSGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Testing GameWorld
        /// (This function will be used for testing purpose only.)
        /// </summary>
        private void createTestGameWorld()
        {
            // Initialize game world
            for (int row = 0; row < this.testScenario.getGameWorld().map.height; ++row)
            {
                for (int col = 0; col < this.testScenario.getGameWorld().map.width; ++col)
                {
                    if (row % 2 == 0)
                    {
                        this.testScenario.getGameWorld().map.getCell(col, row).isValid = false;
                    }
                    else
                    {
                        this.testScenario.getGameWorld().map.getCell(col, row).isValid = true;
                    }

                    if ((col + row)%2 == 0)
                        this.testScenario.getGameWorld().map.getCell(col, row).isValid = true;

                }
            }
        }

        private void createEmptyTestGameWorld()
        {
            // Initialize game world
            for (int row = 0; row < this.testScenario.getGameWorld().map.height; ++row)
            {
                for (int col = 0; col < this.testScenario.getGameWorld().map.width; ++col)
                {
                        this.testScenario.getGameWorld().map.getCell(col, row).isValid = true;

                }
            }
        }

        private void createTestGameWorldSimulateBuildings()
        {
            // Initialize game world
            for (int row = 0; row < this.testScenario.getGameWorld().map.height; ++row)
            {
                for (int col = 0; col < this.testScenario.getGameWorld().map.width; ++col)
                {
                    this.testScenario.getGameWorld().map.getCell(col, row).isValid = true;

                }
            }

            this.testScenario.getGameWorld().map.getCell(9, 9).isValid = false;
            this.testScenario.getGameWorld().map.getCell(9, 10).isValid = false;
            this.testScenario.getGameWorld().map.getCell(9, 11).isValid = false;
            this.testScenario.getGameWorld().map.getCell(10, 9).isValid = false;
            this.testScenario.getGameWorld().map.getCell(10, 10).isValid = false;
            this.testScenario.getGameWorld().map.getCell(10, 11).isValid = false;
            this.testScenario.getGameWorld().map.getCell(11, 9).isValid = false;
            this.testScenario.getGameWorld().map.getCell(11, 10).isValid = false;


            this.testScenario.getGameWorld().map.getCell(14, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(14, 5).isValid = false;
            this.testScenario.getGameWorld().map.getCell(14, 6).isValid = false;
            this.testScenario.getGameWorld().map.getCell(15, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(15, 5).isValid = false;
            this.testScenario.getGameWorld().map.getCell(15, 6).isValid = false;
            this.testScenario.getGameWorld().map.getCell(16, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(16, 5).isValid = false;

            this.testScenario.getGameWorld().map.getCell(4, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(4, 5).isValid = false;
            this.testScenario.getGameWorld().map.getCell(4, 6).isValid = false;
            this.testScenario.getGameWorld().map.getCell(5, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(5, 5).isValid = false;
            this.testScenario.getGameWorld().map.getCell(5, 6).isValid = false;
            this.testScenario.getGameWorld().map.getCell(6, 4).isValid = false;
            this.testScenario.getGameWorld().map.getCell(6, 5).isValid = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            Font1 = Content.Load<SpriteFont>("SpriteFont1");
            // Create Scenario
            this.testScenario = new ZRTSModel.Scenario.Scenario(20, 20);


            // The most challenging obstacles
            //createTestGameWorld();

            // Empty Map
            //createEmptyTestGameWorld(); 

            // Simulating gameplay
            createTestGameWorldSimulateBuildings();

            // Controller
            this.testGameController = new ZRTSLogic.Controller(this.testScenario);

            // Dummy unit
            //this.testScenario.getGameWorld().getUnits().Add(new ZRTSModel.Entities.Unit(0, 20, 100, 50, 0));
            /** NOTE: Adding Entities should be done through the Controller from now on **/
            this.testGameController.addUnit(new ZRTSModel.Entities.Unit(testGameController.scenario.getWorldPlayer(), 20, 100, 50, 0), 5, 10);
            this.testGameController.addUnit(new ZRTSModel.Entities.Unit(testGameController.scenario.getWorldPlayer(), 20, 100, 50, 0), 10, 5);
            this.testGameController.addUnit(new ZRTSModel.Entities.Unit(testGameController.scenario.getWorldPlayer(), 20, 100, 50, 0), 15, 10);

            input = new MouseState();
            prevInput = input;
            gameSelectView = new ViewSelect();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //21/35
            sample_image = new SpriteSheet(Content.Load<Texture2D>("sprites/commandos"), spriteBatch, 21, 35);
            sample_tile = new SpriteSheet(Content.Load<Texture2D>("sprites/green_tile20x20"), spriteBatch, 20, 20);
            sample_util = new SpriteSheet(Content.Load<Texture2D>("sprites/util_misc20x20"), spriteBatch, 20, 20);

            gameSelectView.loadSpriteSheet(sample_util);

            gameView = new View(40, 40, spriteBatch);
            gameView.LoadScenario(this.testGameController.scenario);
            gameView.LoadMap(this.testGameController.gameWorld);
            

            gameView.LoadSpriteSheet(sample_tile);
            gameView.LoadUnitsSpriteSheet(sample_image);
            gameView.LoadUtilitySpriteSheet(sample_util);
            testGameController.registerObserver(gameSelectView);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Checking if mouse is clicking within the valid game locations
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        /// <returns>True if it is valid</returns>
        private Boolean isWithInBound(float x, float y)
        {
            return x < testScenario.getGameWorld().map.width && x >= 0 && y < testScenario.getGameWorld().map.height && y >= 0;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.input = Mouse.GetState();      // Receive input from mouse

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Right click to give a command
            if (input.RightButton == ButtonState.Pressed && prevInput.RightButton == ButtonState.Released)
            {
                commandX = gameView.convertScreenLocToGameLoc(input.X, input.Y).X;
                commandY = gameView.convertScreenLocToGameLoc(input.X, input.Y).Y;


                if (testGameController.isWithinGameBound(commandX, commandY))
                {
                    foreach (ZRTSModel.Entities.Entity entity in this.testGameController.scenario.getPlayer().SelectedEntities)
                    {
                        this.testGameController.giveActionCommand(
                            entity,
                            new ZRTSLogic.Action.MoveAction(commandX, commandY, this.testGameController.gameWorld, entity)
                        );
                    }
                }
            }

            /* Left click to select units */

            // Store the first corner of the "drag box"
            if (input.LeftButton == ButtonState.Pressed && prevInput.LeftButton == ButtonState.Released)
            {
                if (testGameController.isWithinGameBound(selectX, selectY))
                {
                    selectX = gameView.convertScreenLocToGameLoc(input.X, input.Y).X;
                    selectY = gameView.convertScreenLocToGameLoc(input.X, input.Y).Y;

                    gameView.setFirstCornerOfDragBox(input.X, input.Y);
                    gameView.IsDragging = true;
                }
            }

            // While dragging, update the view to draw the box
            if (input.LeftButton == ButtonState.Pressed)
            {
                gameView.setDragBox(input.X, input.Y);
            }

            // "Drag box" is created, select all units within box
            if (input.LeftButton == ButtonState.Released && prevInput.LeftButton == ButtonState.Pressed)
            {
                float releaseX = gameView.convertScreenLocToGameLoc(input.X, input.Y).X;     // coords of release location
                float releaseY = gameView.convertScreenLocToGameLoc(input.X, input.Y).Y;
                float pressX = selectX;  // coords of press location
                float pressY = selectY;

                if (testGameController.isWithinGameBound(releaseX, releaseY) && testGameController.isWithinGameBound(pressX, pressY))
                {
                    /*
                     * Retrieve all units within the drag box - Use Min and Max to find the topleft and
                     * bottomright corner
                     */
                    this.testGameController.scenario.getUnits(
                        (int)Math.Min(pressX, releaseX),
                        (int)Math.Min(pressY, releaseY),
                        (int)(Math.Max(pressX, releaseX) - Math.Min(pressX, releaseX)),
                        (int)(Math.Max(pressY, releaseY) - Math.Min(pressY, releaseY))
                    );

                    // TODO: remove later (DEBUGGING INFORMATION ONLY)
                    Console.WriteLine("(pressX, pressY) = (" + pressX + "," + pressY + ")");
                    Console.WriteLine("topleft = (" + (int)Math.Min(pressX, releaseX) + "," + (int)Math.Min(pressY, releaseY) + ")");
                    Console.WriteLine("(releaseX, releaseY) = (" + releaseX + "," + releaseY + ")");
                    Console.WriteLine("bottomright = (" + (int)Math.Max(pressX, releaseX) + "," + (int)Math.Max(pressY, releaseY) + ")");

                }

                gameView.IsDragging = false;
                
            }

            this.testGameController.updateWorld();

            prevInput = this.input;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			// Can I get the Icon in Cornflower Blue?
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.gameView.Draw();
            gameSelectView.Draw();
            DrawDebugScreen();
            spriteBatch.End();  // remove this after debug is done.
            base.Draw(gameTime);
        }

        /// <summary>
        /// Display debug information
        /// </summary>
        private void DrawDebugScreen()
        {
            spriteBatch.DrawString(Font1, "Clicked at game Location : " + commandX + "," + commandY, new Vector2(500, 200), Color.Black);
            spriteBatch.DrawString(Font1, "Coverted game Location : " + gameView.convertScreenLocToGameLoc(input.X, input.Y).X + "," + gameView.convertScreenLocToGameLoc(input.X, input.Y).Y, new Vector2(500, 300), Color.Black);
            spriteBatch.DrawString(Font1, "Mouse Location : " + input.X + "," + input.Y, new Vector2(500, 250), Color.Black);
            spriteBatch.DrawString(Font1, "Unit Location : " + this.testScenario.getGameWorld().getUnits()[0].x + "," + this.testScenario.getGameWorld().getUnits()[0].y, new Vector2(500, 350), Color.Black);
        }
    }
}