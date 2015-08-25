using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSLogic;
using ZRTSModel;
using NUnit.Framework;

namespace ZRTS.UnitTest
{
    [TestFixture]
    class AddBuildingTest
    {

        ZRTSModel.Scenario.Scenario testScenario;
        ZRTSLogic.Controller testGameController;


        [Test]
        public void testAddNewBuilding()
        {
            initialize();
            ZRTSModel.Entities.Building b = new ZRTSModel.Entities.Building(testScenario.getPlayer(), new ZRTSModel.Entities.BuildingStats());
            
            ///Test building at location (8,8). Should fail because of impassible tile
            ///at (9,9).
            bool success = testGameController.addEntity(b, 8, 8);
            Assert.False(success);
            ///Test building ontop of unit at location (17,17). Should fail, because there is a unit there
            success = testGameController.addEntity(b, 17, 17);
            Assert.False(success);
            ///Test building at location(0,0). Should succeed.
            success = testGameController.addEntity(b, 0, 0);
            Assert.True(success);
            ///Test building at location (12,12). Should fail from lack of resources.
            success = testGameController.addEntity(b, 12, 12);
            Assert.False(success);



        }

        private void initialize()
        {

            // Initialize game world
            testScenario = new ZRTSModel.Scenario.Scenario(20,20);


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

            ///Set controller
            testGameController = new ZRTSLogic.Controller(testScenario);

            ///Add a unit at (17, 17)
            this.testGameController.addUnit(new ZRTSModel.Entities.Unit(testGameController.scenario.getWorldPlayer(), 20), 17, 17);

            ///Initialize Player's resources
            this.testScenario.getPlayer().player_resources[0] = 300;
            this.testScenario.getPlayer().player_resources[1] = 300;
            this.testScenario.getPlayer().player_resources[2] = 300;
            this.testScenario.getPlayer().player_resources[3] = 300;
        }
        
           
    }
}
