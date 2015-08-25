using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework;
using ZRTSModel.GameModel;
using ZRTS.XnaCompositeView;
using ZRTSModel.Factories;
using ZRTSModel;
using ZRTSModel.Trigger;


namespace ZRTS
{
    public class ZRTSController : GameComponent
    {
		Game game;
        public ZRTSController(Game game)
            : base(game)
        {
			this.game = game;
        }
        /// <summary>
        /// Selects the entities in the list of model components.
        /// </summary>
        /// <param name="?"></param>
        public void SelectEntities(List<ModelComponent> EntityList)
        {
            // Filter out zombie unit from player's selected list
            for (int i = 0; i < EntityList.Count; ++i )
            {
                if (EntityList[i] is UnitComponent)
                {
                    if (((UnitComponent)EntityList[i]).IsZombie)
                    {
                        EntityList.Remove(EntityList[i]);
                    }
                }
            }

            SelectionState selectionState = getGameModel().GetSelectionState();
            selectionState.ClearSelectionState();

			bool hasUnits = false;
			bool hasPlayerEntities = false;

            PlayerComponent player = (PlayerComponent) getPlayerList().GetChildren()[0];
			
			foreach (ModelComponent entity in EntityList)
			{
				if (entity is UnitComponent)
				{
					hasUnits = true;

					if (player.GetUnitList().GetChildren().Contains(entity))
					{
						hasPlayerEntities = true;
					}
				}
				else if (entity is Building)
				{
					if (player.BuildingList.GetChildren().Contains(entity))
					{
						hasPlayerEntities = true;
					}
				}
			}

            foreach (ModelComponent entity in EntityList)
            {
				if (hasUnits && hasPlayerEntities)
				{
					if (entity is UnitComponent && player.GetUnitList().GetChildren().Contains(entity))
					{
						selectionState.SelectEntity(entity);
					}
				}
				else if (hasPlayerEntities)
				{
					if (entity is Building && player.BuildingList.GetChildren().Contains(entity))
					{
						selectionState.SelectEntity(entity);
					}
				}
				else if (hasUnits)
				{
					if (entity is UnitComponent)
					{
						selectionState.SelectEntity(entity);
					}
				}
				else
				{
					selectionState.SelectEntity(entity);
				}
            }
        }

        /// <summary>
        /// Gets the GameModel.
        /// </summary>
        /// <returns>The GameModel</returns>
        private GameModel getGameModel()
        {
            return ((XnaUITestGame)Game).Model;
        }

        /// <summary>
        /// Gets the Player List.
        /// </summary>
        /// <returns>The Player List</returns>
        private PlayerList getPlayerList()
        {
            return getGameModel().GetScenario().GetGameWorld().GetPlayerList();
        }

        /// <summary>
        /// Gets the Map.
        /// </summary>
        /// <returns>The Map</returns>
        private Map getMap()
        {
            return getGameModel().GetScenario().GetGameWorld().GetMap();
        }

        /// <summary>
        /// Give selected units a move command
        /// </summary>
        /// <param name="point">Destination</param>
        public void MoveSelectedUnitsToPoint(PointF point)
        {
            List<ModelComponent> selectedEntities = ((XnaUITestGame)Game).Model.GetSelectionState().SelectedEntities;
            // Ensure each component is a Unit.
            bool allAreUnits = true;
			bool playerEntities = false;
            foreach (ModelComponent component in selectedEntities)
            {
				if (entityBelongsToPlayer(component))
				{
					playerEntities = true;
					if (!(component is UnitComponent))
					{
						allAreUnits = false;
						break;
					}
				}
            }
            if (allAreUnits && playerEntities)
            {
                foreach (UnitComponent unit in selectedEntities)
                {
                    MoveAction moveAction = new MoveAction(point.X, point.Y,getMap() , unit);
                    ActionQueue aq = unit.GetActionQueue();
                    aq.GetChildren().Clear();
                    aq.AddChild(moveAction);
                }
            }
        }


        /// <summary>
        /// Give selected units an attack command
        /// </summary>
        /// <param name="unit">target unit</param>
        public void TellSelectedUnitsToAttack(UnitComponent unit)
        {
            List<ModelComponent> selectedEntities = getGameModel().GetSelectionState().SelectedEntities;
            bool canAttack = true;
			bool playerEntities = false;
            foreach (ModelComponent entity in selectedEntities)
            {
				if (entityBelongsToPlayer(entity))
				{
					playerEntities = true;
					if (!(entity is UnitComponent))
					{
						canAttack = false;
						break;
					}
					else
					{
						UnitComponent u = entity as UnitComponent;
						if (!u.CanAttack)
						{
							canAttack = false;
							break;
						}
					}
				}
            }
            if (canAttack && playerEntities)
            {
                foreach (UnitComponent u in selectedEntities)
                {
                    u.GetActionQueue().GetChildren().Clear();
                    u.GetActionQueue().AddChild(new AttackAction(u, unit, getGameModel().GetScenario().GetGameWorld()));
                }
            }
        }

        /// <summary>
        /// Update all players' units
        /// </summary>
        /// <param name="players">List of all players</param>
        private void UpdateAllPlayersUnits(PlayerList players)
        {
            foreach (PlayerComponent player in players.GetChildren())
            {
                UnitList units = player.GetUnitList();
                // Make a copy of the list that will not be tampered with by the attack action.
                List<UnitComponent> unitList = new List<UnitComponent>();
                foreach (UnitComponent unit in units.GetChildren())
                {
                    unitList.Add(unit);
                }
                foreach (UnitComponent unit in unitList)
                {
                    if (unit.State != ZRTSModel.UnitComponent.UnitState.DEAD)
                        unit.GetActionQueue().Work();
                }

                List<ModelComponent> buildings = player.BuildingList.GetChildren();

                foreach (Building b in buildings)
                {
                    b.BuildingActionQueue.Work();
                }
            }
        }

        /// <summary>
        /// Update all trigger events in the game
        /// </summary>
        /// <param name="triggers">List of trigger events</param>
        private void UpdateAllTriggers(List<Trigger> triggers)
        {
            List<Trigger> removeList = new List<Trigger>();
            foreach (Trigger t in triggers)
            {
                if (t.Eval())
                {
                    t.PerformActions();
                    removeList.Add(t);
                }
            }

            foreach (Trigger t in removeList)
            {

                triggers.Remove(t);
            }
        }



        /// <summary>
        /// Update units that belong to the player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            PlayerList players = getPlayerList();
            UpdateAllPlayersUnits(players);

			List<Trigger> triggers = getGameModel().GetScenario().triggers;
            UpdateAllTriggers(triggers);
			
            base.Update(gameTime);
        }

        internal void OnSelectBuildingToBuild(string buildingType)
        {
            changeMapViewLeftClickStrategyToBuild(buildingType);
        }

        internal void OnSelectedUnitsToHarvest()
        {
            MapView mapView = ((XnaUITestGame)Game).View.GetChildren()[0] as MapView;
            //###########################################################################
            // TODO: get the mouse Click Location and pass to TellSelectedUnitToHarvest
            //#########################################################################3
            //TellSelectedUnitsToHarvest(#### Mouse Click Location #####);
        }

        /// <summary>
        /// Insert building's action queue to build something
        /// </summary>
		public void TellSelectedBuildingToBuild()
		{
			List<ModelComponent> selectedEntities = getGameModel().GetSelectionState().SelectedEntities;
			if (selectedEntities.Count > 0 && selectedEntities[0] is Building)
			{
				Building b = (Building)selectedEntities[0];
                ProduceUnit produceAction= null;
                if (b.Type.Equals("house"))
                {
                    produceAction = new ZRTSModel.ProduceUnit(b, "worker");
                }
                else
                {
                    produceAction = new ZRTSModel.ProduceUnit(b, "soldier");
                }
				 
				b.BuildingActionQueue.AddChild(produceAction);
			}
		}


        /// <summary>
        /// select unit to stop
        /// </summary>
        internal void OnSelectUnitToStop()
        {
           List<ModelComponent> selectedEntities = ((XnaUITestGame)Game).Model.GetSelectionState().SelectedEntities;
           foreach (ModelComponent e in selectedEntities)
           {
               if (e is UnitComponent)
               {
                   UnitComponent unit = (UnitComponent)e;
                   unit.GetActionQueue().GetChildren().Clear();
                   unit.State = UnitComponent.UnitState.IDLE;
                   
               }
           }
        }

        /// <summary>
        /// Notifies the map view of the selection so that it may change its strategy for handling mouse messages in order to show the
        /// building image before placing as well as to build the building on placement.
        /// </summary>
        /// <param name="buildingType"></param>
        private void changeMapViewLeftClickStrategyToBuild(string buildingType)
        {
            // HACK: Assumes that the map view is the first child.
            // Solution: Create a ZRTSView child to the frame, that has members for the map view, minimap, selection view, command view, and any menus.
            MapView mapView = ((XnaUITestGame)Game).View.GetChildren()[0] as MapView;
            mapView.LeftButtonStrategy = new BuildBuildingMapViewLeftClickStrategy(mapView, buildingType);
        }

        /// <summary>
        /// Checking if a particular region of map (cells) are empty
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal bool CellsAreEmpty(int x, int y, int width, int height)
        {
            Map map = getGameModel().GetScenario().GetGameWorld().GetMap();
            bool areEmpty = (x + width < map.GetWidth()) && (y + height < map.GetHeight());
            if (areEmpty)
            {

                for (int i = x; (i < x + width) && areEmpty; i++)
                {
                    for (int j = y; (j < y + height) && areEmpty; j++)
                    {
                        areEmpty = map.GetCellAt(i, j).EntitiesContainedWithin.Count == 0;
                    }
                }
            }
            return areEmpty;
        }

        internal bool CellsArePassable(int x, int y, int width, int height)
        {
            Map map = getGameModel().GetScenario().GetGameWorld().GetMap();
            bool arePassable = (x + width < map.GetWidth()) && (y + height < map.GetHeight());
            if (arePassable)
            {

                for (int i = x; (i < x + width) && arePassable; i++)
                {
                    for (int j = y; (j < y + height) && arePassable; j++)
                    {
                        arePassable = map.GetCellAt(i, j).GetTile().Passable();
                    }
                }
            }
            return arePassable;
        }


        /// <summary>
        /// Checking if all selected units have an ability to build
        /// </summary>
        /// <param name="selectedEntities">Selected units</param>
        /// <returns>True if they are all capable of build. otherwise, false is returned!</returns>
        private bool CanAllUnitsBuild(List<ModelComponent> selectedEntities)
        {
            foreach (ModelComponent entity in selectedEntities)
            {
                if (entityBelongsToPlayer(entity))
                {
                    if (!(entity is UnitComponent))
                    {
                        return false;
                    }
                    else
                    {
                        UnitComponent u = entity as UnitComponent;
                        if (!u.CanBuild)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checking if all selected entities belong to the player
        /// </summary>
        /// <param name="selectedEntities">selected entities</param>
        /// <returns>True if they are all owned by the player. False otherwise.</returns>
        private bool AllBelongsToPlayer(List<ModelComponent> selectedEntities)
        {
            foreach (ModelComponent entity in selectedEntities)
            {
                if (!entityBelongsToPlayer(entity))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tell selected units to build a building at the specified location
        /// </summary>
        /// <param name="buildingType">Building Type</param>
        /// <param name="upperLeftCellCoords">Game coordinate</param>
        internal void TellSelectedUnitsToBuildAt(string buildingType, Point upperLeftCellCoords)
        {
            List<ModelComponent> selectedEntities = getGameModel().GetSelectionState().SelectedEntities;
            
            if (CanAllUnitsBuild(selectedEntities) && AllBelongsToPlayer(selectedEntities) && CellsAreEmpty(upperLeftCellCoords.X, upperLeftCellCoords.Y, 6, 6))
            {
                // TODO: Check resources.
                
                // Create the actual building that each action will reference.
                BuildingFactory factory = BuildingFactory.Instance;
                Building buildingToAdd = factory.Build(buildingType, false);
                buildingToAdd.PointLocation = new PointF(upperLeftCellCoords.X, upperLeftCellCoords.Y);
                foreach (UnitComponent u in selectedEntities)
                {
                    u.GetActionQueue().AddChild(new BuildAction(buildingToAdd, getGameModel().GetScenario().GetGameWorld().GetMap()));
                }
            }
        }

        

        /// <summary>
        /// Tell selected units to attack building
        /// </summary>
        /// <param name="building"></param>
        internal void TellSelectedUnitsToAttack(Building building)
        {
            // TODO: Implement
        }

        /// <summary>
        /// Checking if a specific entity is owned by a player
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True if that entity is owned by the player, false otherwise.</returns>
		private bool entityBelongsToPlayer(ModelComponent entity)
		{
			PlayerComponent player = (PlayerComponent)((XnaUITestGame)game).Model.GetScenario().GetGameWorld().GetPlayerList().GetChildren()[0];

			if (entity is UnitComponent)
			{
				return player.GetUnitList().GetChildren().Contains(entity);
			}
			else if (entity is Building)
			{
				return player.BuildingList.GetChildren().Contains(entity);
			}

			return false;
		}

        /// <summary>
        /// Tell selected units to harvest (TODO)
        /// </summary>
        public void TellSelectedUnitsToHarvest(PointF destination)
        {
            List<ModelComponent> selectedEntities = getGameModel().GetSelectionState().SelectedEntities;
            foreach (ModelComponent entity in selectedEntities)
            {
                if (entityBelongsToPlayer(entity))
                {
                    if (!(entity is UnitComponent))
                    {
                        break;
                    }
                    else
                    {
                        UnitComponent u = entity as UnitComponent;
                        if (!u.CanHarvest)
                        {
                            continue;
                        }
                        else
                        {
                            // DO SOMETHING TO HARVEST!!!!!
                            System.Console.Out.WriteLine("NOT IMPLEMENT YET: harvest at " + destination.X + " : " + destination.Y);
                            //u.GetActionQueue().GetChildren().Clear();
                            //u.GetActionQueue().AddChild(new AttackAction
                        }
                    }
                }// if
            }
            //throw new NotImplementedException();
        }
    }
}
