using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTS;
using ZRTSModel;
using ZRTSModel.GameModel;
using ZRTSModel.Factories;
using NUnit.Framework;
namespace ZRTS
{
    /// <summary>
    /// Testing placing building on the map and tell the selected units to build a building on the map
    /// 
    ///
    /// This testsuite is actually testing the followings:
    /// - Check if the player's unit is selected on the map (simulate)
    /// - Check if the player's unit is given a correct action to build the building (simulate)
    /// - Check if the building can be placed on the map under various conditions
    /// 
    /// This testsuite will simulate the actual gameplay logic and sequence without the View (User Interface) part.
    /// 
    /// Components invovled:
    /// - Map
    /// - CellComponent
    /// - SelectionState
    /// - BuildAction
    /// - PlayerComponent
    /// - ZRTSController
    /// - BuildingList
    /// </summary>
    [TestFixture]
    class TestPlacingBuildingOntheMap
    {
        private GameModel model;
        private PlayerComponent player1, player2, currentPlayer1, currentPlayer2;
        private XnaUITestGame game;
        private List<ModelComponent> unitList;

        /// <summary>
        /// Initialize necessary components
        /// </summary>
        private void initialize()
        {
            game = new XnaUITestGame();
            ZRTSController controller = new ZRTSController(game);
            game.Components.Add(controller);

            model = new GameModel();
            ScenarioComponent scenario = new ScenarioComponent(50, 50);
            player1 = new PlayerComponent();
            player2 = new PlayerComponent();
            player1.Name = "Nate";
            player2.Name = "Smith";


            // Add sand cells at each cell.
            ZRTSModel.Map map = scenario.GetGameWorld().GetMap();
            for (int i = 0; i < map.GetWidth(); i++)
            {
                for (int j = 0; j < map.GetHeight(); j++)
                {
                    CellComponent cell = new CellComponent();
                    cell.AddChild(new Sand());
                    cell.X = i;
                    cell.Y = j;
                    map.AddChild(cell);
                }
            }
            model.AddChild(scenario);
            game.Model = model;  
            game.Model.PlayerInContext = player1;   // Set a main Player

            //Create two players and set them to be enemies.
            game.Model.GetScenario().GetGameWorld().GetPlayerList().AddChild(player1);
            game.Model.GetScenario().GetGameWorld().GetPlayerList().AddChild(player2);

            // Set target enemy
            player1.EnemyList.Add(player2);
            player2.EnemyList.Add(player1);

            game.Controller = controller;

            // Add worker to player
            unitList = new List<ModelComponent>();
            unitList.Add(new UnitComponent());
            ((UnitComponent)unitList[0]).CanBuild = true;
            ((UnitComponent)unitList[0]).Type = "worker";
            ((UnitComponent)unitList[0]).PointLocation = new PointF(20, 20);


            //(1) Get Player#1
            currentPlayer1 = (PlayerComponent)((XnaUITestGame)game).Model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0];

            // check if fetch the correct player
            Assert.AreEqual("Nate", currentPlayer1.Name);

            // (2) Add the actual player's unit list
            currentPlayer1.GetUnitList().AddChild(unitList[0]);

            
        }

        /// <summary>
        /// Test if correct player's unit is selected.
        /// </summary>
        [Test]
        public void TestSelectCorrectUnit()
        {
            initialize();
            // (3) Simulate the player's selected unit on the screen
            game.Controller.SelectEntities(unitList);

            // Check if select the correct unit
            Assert.AreEqual(unitList[0], game.Model.GetSelectionState().SelectedEntities[0], "Select correct unit type");
            Assert.AreEqual(1, game.Model.GetSelectionState().SelectedEntities.Count, "the number of selected units");
            Assert.AreEqual(true, currentPlayer1.GetUnitList().GetChildren().Contains(unitList[0]), "Check the ownership of the selected unit");
        }


        /// <summary>
        /// Check if the building is placed on the map properly
        /// </summary>
        [Test]
        public void TestPlacingBuildingOnMap()
        {
            TestSelectCorrectUnit();

            Building building2 = new Building();
            building2.PointLocation = new PointF(20, 20);
            building2.Type = "barracks";

            // (4) Simulate telling the player's unit to build the building at 20,20 (mouse click at tile 20,20 on the map)
            game.Controller.TellSelectedUnitsToBuildAt(building2.Type, new Microsoft.Xna.Framework.Point(20, 20));

            // Check to see if you can add the building to the map
            Assert.IsTrue(game.Model.GetScenario().GetGameWorld().GetMap().canAddBuildingToMap(building2));

            // Check if unit is at where it can build the building
            Assert.AreEqual(true, building2.PointLocation.X - 1 < ((UnitComponent)unitList[0]).PointLocation.X && ((UnitComponent)unitList[0]).PointLocation.X < building2.PointLocation.X + building2.Width + 2, "Unit is not in a range of create a building");

            // (5) Simulate unit is building the building on the map
            ((BuildAction)((UnitComponent)(game.Model.GetSelectionState().SelectedEntities[0])).GetActionQueue().GetChildren()[0]).Work();

            // Check if building gets added on the map
            Assert.AreEqual(1, currentPlayer1.BuildingList.GetChildren().Count, "Building should be added!");
            Assert.AreEqual(building2.Type,((Building) currentPlayer1.BuildingList.GetChildren()[0]).Type, "Incorrect type of building");
           

            // Check all cells with the area of building after being placed to see if it is actually placed on the map
            for (int i = 20; i < 20 + building2.Width; ++i)
            {
                for (int j = 20; j < 20 + building2.Height; ++j)
                {
                    Assert.AreEqual(building2.Type, ((Building) game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(i, j).EntitiesContainedWithin[0]).Type, "Should be the same object at " + i + "," + j);
                }
            }
        }

        /// <summary>
        /// Testing that building cannot be placed if one of the available space is occupied
        /// </summary>
        [Test]
        public void TestCannotPlaceBuildingOnOccupiedSpace()
        {
            TestSelectCorrectUnit();

            // Add unit in the space that we are going to place a building
            //(1) Get Player#2
            currentPlayer2 = (PlayerComponent)((XnaUITestGame)game).Model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[1];

            // check if fetch the corret player
            Assert.AreEqual("Smith", currentPlayer2.Name);

            // (2) Add the actual player2's unit list
            List<ModelComponent> unitList2 = new List<ModelComponent>();
            unitList2.Add(new UnitComponent());
            currentPlayer2.GetUnitList().AddChild(unitList2[0]);
            ((UnitComponent)unitList2[0]).CanBuild = true;
            ((UnitComponent)unitList2[0]).Type = "worker";

            // (3) Place player2's unit where it is in between space that we are going to place a building
            ((UnitComponent)unitList2[0]).PointLocation = new PointF(24, 24);
            game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(24, 24).AddEntity(unitList2[0]);
            Assert.AreEqual(unitList2[0], ((UnitComponent)game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(24, 24).EntitiesContainedWithin[0]), "Should be the same object at 24,24");
            Assert.AreEqual(1, game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(24, 24).EntitiesContainedWithin.Count, "Only one unit in the cell");

            Building building2 = new Building();
            building2.PointLocation = new PointF(20, 20);
            building2.Type = "barracks";

            // (4) Simulate telling the player1's unit to build the building at 20,20 (mouse click at tile 20,20 on the map)
            game.Controller.TellSelectedUnitsToBuildAt(building2.Type, new Microsoft.Xna.Framework.Point(20, 20));

            // Check if unit of the player#1 is at where it can build the building
            Assert.AreEqual(true, building2.PointLocation.X - 1 < ((UnitComponent)unitList[0]).PointLocation.X && ((UnitComponent)unitList[0]).PointLocation.X < building2.PointLocation.X + building2.Width + 2, "Unit is not in a range of create a building");

            // (5) Simulate unit is building the building on the map (Player1's unit is about to place the building)
            ((BuildAction)((UnitComponent)(game.Model.GetSelectionState().SelectedEntities[0])).GetActionQueue().GetChildren()[0]).Work();

            // Check if the building is actually placed, it should not
            Assert.AreEqual(0, currentPlayer1.BuildingList.GetChildren().Count, "Building should not be added!");
            Assert.AreEqual(0, game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(20, 20).EntitiesContainedWithin.Count, "This space should be available.");
            
        }


        /// <summary>
        /// Check if building can be placed outside the screen
        /// </summary>
        [Test]
        public void TestCannotPlaceBuildingOutsideMap()
        {
            TestSelectCorrectUnit();
            Building building = new Building();
            building.Type = "hospital";
            building.PointLocation = new PointF(50, 50);

            // set a unit by the edge of the map
            ((UnitComponent)unitList[0]).PointLocation = new PointF(49, 49);    

            bool catchException = false;
            try 
            {
                // Mouse click outside the game screen (Invalid Cell)
                game.Controller.TellSelectedUnitsToBuildAt(building.Type, new Microsoft.Xna.Framework.Point(50, 50));
            } catch (Exception e)
            {
                catchException = true;
                
            }
            Assert.IsTrue(catchException, "Exception should be raised! No command can be given!");

            catchException = false;
            
            try
            {
                // Double check if invalid action is action given to the unit
                ((BuildAction)((UnitComponent)(game.Model.GetSelectionState().SelectedEntities[0])).GetActionQueue().GetChildren()[0]).Work();
            }
            catch (Exception e)
            {
                catchException = true;
            }

            Assert.IsTrue(catchException, "Invalid action will not be executed!");
            Assert.AreEqual(0, currentPlayer1.BuildingList.GetChildren().Count, "Building should not be added!");
        }

        /// <summary>
        /// Testing that building cannot be built if available space is partially occupied by another building
        /// </summary>
        [Test]
        public void TestCannotOverlapPlacingBuilding()
        {
            TestSelectCorrectUnit();

            // (1) First, place a building at a particular location
            Building building1 = new Building();
            building1.Type = "barracks";
            building1.PointLocation = new PointF(20, 20);

            // (2) Place the building on the map
            game.Controller.TellSelectedUnitsToBuildAt(building1.Type, new Microsoft.Xna.Framework.Point(20, 20));
            ((BuildAction)((UnitComponent)(game.Model.GetSelectionState().SelectedEntities[0])).GetActionQueue().GetChildren()[0]).Work();

            // (3) Check if the first building is actually placed on the map
            for (int i = 20; i < 20 + building1.Width; ++i)
            {
                for (int j = 20; j < 20 + building1.Height; ++j)
                {
                    Assert.AreEqual(building1.Type, ((Building)game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(i, j).EntitiesContainedWithin[0]).Type, "Should be the same object at " + i + "," + j);
                }
            }
            Assert.AreEqual(1, currentPlayer1.BuildingList.GetChildren().Count, "Building should have been added!");

            // (4) Try to build another building on an overlapping point
            Building building2 = new Building();
            building2.Type = "house";
            building2.PointLocation = new PointF(15, 15);

            // (5) Set the worker unit by the build location
            ((UnitComponent)unitList[0]).PointLocation = new PointF(14, 14);

            // Select the worker to place another building
            game.Controller.SelectEntities(unitList);

            // (6) Place the 2nd building 
            game.Controller.TellSelectedUnitsToBuildAt(building2.Type, new Microsoft.Xna.Framework.Point(15, 15));
            ((BuildAction)((UnitComponent)(game.Model.GetSelectionState().SelectedEntities[0])).GetActionQueue().GetChildren()[0]).Work();

            // Check building count; should = 1
            Assert.AreEqual(1, currentPlayer1.BuildingList.GetChildren().Count, "The new Building should not be added!");

            // (7) Check if the second building is actually placed on the map, it should not
            for (int i = 15; i < 15 + building2.Width; ++i)
            {
                for (int j = 15; j < 15 + building2.Height; ++j)
                {
                    Assert.AreNotEqual(building2.Type, ((Building)game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(i, j).EntitiesContainedWithin[0]).Type, "Should not be the same object at " + i + "," + j);
                }
            }
        }


        /// <summary>
        /// Testing remove building from the map
        /// </summary>
        [Test]
        public void TestRemoveBuildingFromMap()
        {
            // (1) First, place a building at a particular location
            Building building1 = new Building();
            building1.Type = "barracks";
            building1.PointLocation = new PointF(20, 20);

            // (2) Place the building on the map
            game.Model.GetScenario().GetGameWorld().GetMap().addBuildingToMap(building1);

            // Check to see if building was actually placed
            Assert.AreEqual(1, currentPlayer1.BuildingList.GetChildren().Count, "Building should have been added!");

            // (3) Simulate removing the building from the map
            game.Model.GetScenario().GetGameWorld().GetMap().removeBuildingFromMap(building1);

            // Check all cells with the area of building after being placed to see if it is actually removed from the map
            for (int i = 20; i < 20 + building1.Width; ++i)
            {
                for (int j = 20; j < 20 + building1.Height; ++j)
                {
                    Assert.AreNotEqual(building1.Type, ((Building)game.Model.GetScenario().GetGameWorld().GetMap().GetCellAt(i, j).EntitiesContainedWithin[0]).Type, "Should be the same object at " + i + "," + j);
                }
            }
        }

    }
}
