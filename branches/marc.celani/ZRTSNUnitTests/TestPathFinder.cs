using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSModel;
using ZRTSModel.GameModel;
using Pathfinder;


namespace ZRTSNUnitTests
{
    [TestFixture]
    class TestPathFinder
    {
        GameModel model;

        private void setUpModel()
        {
            model = new GameModel();
            ScenarioComponent scenario = new ScenarioComponent(20, 20); // 20 x 20 Gameworld.
            Building obstruction = new Building();

            // Add grass cells at each cell.
            ZRTSModel.Map map = scenario.GetGameWorld().GetMap();
            for (int i = 0; i < map.GetWidth(); i++)
            {
                for (int j = 0; j < map.GetHeight(); j++)
                {
                    CellComponent cell = new CellComponent();
                    cell.AddChild(new Sand());
                    cell.X = i;
                    cell.Y = j;
                
                    if (i >= 2 && i <= 10 && j >= 2 && j <= 10)
                        cell.AddEntity(obstruction);

                    if (i >= 15 && i <= 18 && j >= 15 && j <= 18)
                        cell.AddEntity(obstruction);
                    if (i == 16 && j == 16)
                        cell.RemoveEntity(obstruction);

                    map.AddChild(cell);
                    

                }
            }
            model.AddChild(scenario);

        }


        /// <summary>
        /// The path should go around the building from (2,2) to 
        /// (10,10), not through it
        /// </summary>
        [Test]
        public void testMoveAroundBuilding()
        {
            setUpModel();
            List<CellComponent> path;
            Map map = model.GetScenario().GetGameWorld().GetMap();

            path = Pathfinder.FindPath.between(map, map.GetCellAt(1, 1), map.GetCellAt(11, 11));
             
            Assert.Contains(map.GetCellAt(1,1), path);
            Assert.Contains(map.GetCellAt(11,11), path);
            for (int i = 0; i < path.Count; i++)
                Assert.True(path[i].X >= 2 && path[i].X <= 10 && path[i].Y >= 2 && path[i].Y <= 10);

        }

        /// <summary>
        /// The destination is enclosed on all sides by obstructions
        /// The PathFinder should return no path
        /// </summary>
        [Test]
        public void testEnclosedDestination()
        {
            setUpModel();
            List<CellComponent> path;
            Map map = model.GetScenario().GetGameWorld().GetMap();

            path = Pathfinder.FindPath.between(map, map.GetCellAt(1, 1), map.GetCellAt(16, 16));
            Assert.IsEmpty(path);
        }

        /// <summary>
        /// The intended destination is the edge of a building.
        /// the given destination should be a valid cell surrounding the 
        /// intended cell
        /// </summary>
        [Test]
        public void testObstructedDestination()
        {
            setUpModel();
            List<CellComponent> path;
            Map map = model.GetScenario().GetGameWorld().GetMap();

            path = Pathfinder.FindPath.between(map, map.GetCellAt(1, 1), map.GetCellAt(10, 10));
            Assert.IsNotEmpty(path);
            Assert.True(path[path.Count - 1].X >= 9 && path[path.Count - 1].X <= 11
                && path[path.Count - 1].Y >= 9 && path[path.Count - 1].Y <= 11);
            Assert.True(path[path.Count - 1].GetTile().Passable());
        }

        /// <summary>
        /// A unit obstructs the destination, so the path is a valid cell adjacent
        /// </summary>
        [Test]
        public void testUnitObstruction()
        {
            setUpModel();
            List<CellComponent> path;
            UnitComponent soldier = new UnitComponent();
            Map map = model.GetScenario().GetGameWorld().GetMap();
            map.GetCellAt(14, 14).AddEntity(soldier);


            path = Pathfinder.FindPath.between(map, map.GetCellAt(1, 1), map.GetCellAt(14, 14));
            Assert.IsNotEmpty(path);
            Assert.True(path[path.Count - 1].X >= 13 && path[path.Count - 1].X <= 15
                && path[path.Count - 1].Y >= 13 && path[path.Count - 1].Y <= 15);
            Assert.True(path[path.Count - 1].GetTile().Passable());
        }
    }
}
