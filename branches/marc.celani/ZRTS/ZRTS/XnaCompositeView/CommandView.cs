using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTSModel.Factories;
using System.Collections;
using ZRTS.InputEngines;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{

    /// <summary>
    /// CommandView:
    /// This class will act as a command menu manager for each game unit/building.
    /// </summary>
    public class CommandView : XnaUIComponent
    {
        private SameSizeChildrenFlowLayout mainPanel;
        private SameSizeChildrenFlowLayout workerPanel;
        private PictureBox backgroundPanel;
        //private PictureBox mainBgPanel;
        private PictureBox stopButton;
        private PictureBox moveButton;
        private PictureBox attackButton;
        private PictureBox mainStopButton;
        private PictureBox mainMoveButton;
        private PictureBox mainAttackButton;
        private PictureBox buildButton;
        private PictureBox harvestButton;
        private Hashtable uiToBuildingType = new Hashtable();
		private Texture2D pixel;
		private Color color;

        private SameSizeChildrenFlowLayout buildPanel;

		private SameSizeChildrenFlowLayout barracksPanel;
        private SameSizeChildrenFlowLayout housePanel;
        
		private ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
		long lastClick = 0;
		const int MSECONDS_PER_CLICK = 100;

        public CommandView(Game game)
            : base(game)
        {
            this.DrawBox = new Rectangle(0, 0, 275, 275);


            // Background Panel
            backgroundPanel = new PictureBox(game, new Rectangle(GameConfig.BUILDPANEL_START_X, GameConfig.BUILDING_START_Y, 275, 275));
            backgroundPanel.DrawBox = new Rectangle(0, 0, 275, 275);
            AddChild(backgroundPanel);

            // buildPanel
            buildPanel = new SameSizeChildrenFlowLayout(game);
            buildPanel.Visible = false;
            buildPanel.DrawBox = new Rectangle(10, 10, 255, 255);
            buildPanel.SpacingBetween = 7;
            AddChild(buildPanel);

            // mainPanel
            mainPanel = new SameSizeChildrenFlowLayout(game);
            mainPanel.DrawBox = new Rectangle(10, 10, 255, 255);
            mainPanel.SpacingBetween = 7;
            AddChild(mainPanel);

            // WorkerPanel
            workerPanel = new SameSizeChildrenFlowLayout(game);
            workerPanel.DrawBox = new Rectangle(10, 10, 255, 255);
            workerPanel.SpacingBetween = 7;
            AddChild(workerPanel);


			// Barrack building commandPanel
			barracksPanel = new SameSizeChildrenFlowLayout(game);
			barracksPanel.DrawBox = new Rectangle(10, 10, 255, 255);
			barracksPanel.Visible = false;
            barracksPanel.SpacingBetween = 7;
			AddChild(barracksPanel);

            // HouseBuilding commandPanel
            housePanel = new SameSizeChildrenFlowLayout(game);
            housePanel.DrawBox = new Rectangle(10, 10, 255, 255);
            housePanel.SpacingBetween = 7;
            housePanel.Visible = false;
            AddChild(housePanel);



            ZRTSCompositeViewUIFactory factory = ZRTSCompositeViewUIFactory.Instance;
            moveButton = factory.BuildPictureBox("button", "move");
            moveButton.DrawBox = new Rectangle(0, 0, GameConfig.BUTTON_DIM , GameConfig.BUTTON_DIM );
            moveButton.OnClick += handleMoveButtonClick;
            moveButton.OnMouseEnter += handleMoveButtonOver;
            moveButton.OnMouseLeave += handleMoveButtonAway;
            moveButton.OnMouseDown += handleMoveButtonDown;
            moveButton.OnMouseUp += handleMoveButtonUp;
            workerPanel.AddChild(moveButton);

            mainMoveButton = factory.BuildPictureBox("button", "move");
            mainMoveButton.DrawBox = new Rectangle(0, 0, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            mainMoveButton.OnClick += handleMoveButtonClick;
            mainMoveButton.OnMouseEnter += handleMoveButtonOver;
            mainMoveButton.OnMouseLeave += handleMoveButtonAway;
            mainMoveButton.OnMouseDown += handleMoveButtonDown;
            mainMoveButton.OnMouseUp += handleMoveButtonUp;
            mainPanel.AddChild(mainMoveButton);

            stopButton = factory.BuildPictureBox("button", "stop");
            stopButton.DrawBox = new Rectangle(0, 0, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            stopButton.OnClick += handleStopButtonClick;
            stopButton.OnMouseEnter += handleStopButtonOver;
            stopButton.OnMouseLeave += handleStopButtonAway;
            stopButton.OnMouseDown += handleStopButtonDown;
            stopButton.OnMouseUp += handleStopButtonUp;
            workerPanel.AddChild(stopButton);

            mainStopButton = factory.BuildPictureBox("button", "stop");
            mainStopButton.DrawBox = new Rectangle(0, 0, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            mainStopButton.OnClick += handleStopButtonClick;
            mainStopButton.OnMouseEnter += handleStopButtonOver;
            mainStopButton.OnMouseLeave += handleStopButtonAway;
            mainStopButton.OnMouseDown += handleStopButtonDown;
            mainStopButton.OnMouseUp += handleStopButtonUp;
            mainPanel.AddChild(mainStopButton);

            buildButton = factory.BuildPictureBox("button", "build");
            buildButton.DrawBox = new Rectangle(GameConfig.BUTTON_BUILD * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            buildButton.OnClick += handleBuildButtonClick;
            buildButton.OnMouseEnter += handleBuildButtonOver;
            buildButton.OnMouseLeave += handleBuildButtonAway;
            buildButton.OnMouseDown += handleBuildButtonDown;
            buildButton.OnMouseUp += handleBuildButtonUp;
            workerPanel.AddChild(buildButton);

            attackButton = factory.BuildPictureBox("button", "attack");
            attackButton.DrawBox = new Rectangle(GameConfig.BUTTON_ATTACK * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            attackButton.OnClick += handleAttackButtonClick;
            attackButton.OnMouseEnter += handleAttackButtonOver;
            attackButton.OnMouseLeave += handleAttackButtonAway;
            attackButton.OnMouseDown += handleAttackButtonDown;
            attackButton.OnMouseUp += handleAttackButtonUp;
            workerPanel.AddChild(attackButton);

            
            harvestButton = factory.BuildPictureBox("button", "harvest");
            harvestButton.DrawBox = new Rectangle(GameConfig.BUTTON_ATTACK * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            harvestButton.OnClick += handleHarvestButtonClick;
            harvestButton.OnMouseEnter += handleharvestButtonOver;
            harvestButton.OnMouseLeave += handleharvestButtonAway;
            harvestButton.OnMouseDown += handleharvestButtonDown;
            harvestButton.OnMouseUp += handleharvestButtonUp;
            workerPanel.AddChild(harvestButton);
            
            mainAttackButton = factory.BuildPictureBox("button", "attack");
            mainAttackButton.DrawBox = new Rectangle(GameConfig.BUTTON_ATTACK * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM);
            mainAttackButton.OnClick += handleAttackButtonClick;
            mainAttackButton.OnMouseEnter += handleAttackButtonOver;
            mainAttackButton.OnMouseLeave += handleAttackButtonAway;
            mainAttackButton.OnMouseDown += handleAttackButtonDown;
            mainAttackButton.OnMouseUp += handleAttackButtonUp;
            mainPanel.AddChild(mainAttackButton);

            mainPanel.Visible = false;
            workerPanel.Visible = false;

            // Individual building buttons in the build panel

            List<String> buildingKeys = BuildingFactory.Instance.getBuildingTypes();
            foreach (String key in buildingKeys)
            {
                PictureBox buildingButton = factory.BuildPictureBox("building", key);
                buildingButton.OnClick += handleBuildingButtonClick;
                buildingButton.DrawBox = new Rectangle(0, 0, 85, 85);
                buildPanel.AddChild(buildingButton);
                uiToBuildingType.Add(buildingButton, key);
            }

            // Get unit's information 
			List<String> unitKeys = UnitFactory.Instance.getPrefixes();

			foreach (String key in buildingKeys)
			{
                PictureBox unitButton = null;
                if (key.Equals("barracks"))
                {
                    unitButton = factory.BuildPictureBox("unitBuild", "soldier");
                    unitButton.OnClick += handleUnitProduceButtonClick;
                    unitButton.DrawBox = new Rectangle(0, 0, 85, 85);
                    barracksPanel.AddChild(unitButton);
                }
                else if (key.Equals("house"))
                {
                    unitButton = factory.BuildPictureBox("unitBuild", "worker");
                    unitButton.OnClick += handleUnitProduceButtonClick;
                    unitButton.DrawBox = new Rectangle(0, 0, 85, 85);
                    housePanel.AddChild(unitButton);
                }

                uiToBuildingType.Add(unitButton, key);
			}


			pixel = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
			color = new Color(100, 60, 88);
            
        }

        /// <summary>
        /// hide all command buttons
        /// </summary>
        public void disableButtons()
        {
            mainPanel.Visible = false;
            workerPanel.Visible = false;
        }

        /// <summary>
        /// Show all command buttons
        /// </summary>
        public void activateButtons(bool nonbuilders)
        {
            if (!nonbuilders)
                mainPanel.Visible = true;
            else
                workerPanel.Visible = true;
        }

		public void activateProduceUnitButtons(string type)
		{
            if (type.Equals("barracks"))
                barracksPanel.Visible = true;
            else if (type.Equals("house"))
                housePanel.Visible = true;
		}

		public void deactivateProduceUnitButtons()
		{
           
            barracksPanel.Visible = false;
            housePanel.Visible = false;
		}


        private void handleBuildingButtonClick(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled)
            {
                string buildingType = uiToBuildingType[sender] as string;
                ((XnaUITestGame)Game).Controller.OnSelectBuildingToBuild(buildingType);
                workerPanel.Visible = true;
                buildPanel.Visible = false;
                e.Handled = true;
            }
        }

        private void handleBuildButtonClick(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled)
            {
                mainPanel.Visible = false;
                workerPanel.Visible = false;
                buildPanel.Visible = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Trigger action when stop button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleStopButtonClick(object sender, XnaMouseEventArgs e)
        {
            //System.Console.Out.WriteLine("STOP BUTTON IS CLICKED!");
            if (e.Bubbled && !e.Handled)
            {
                e.Handled = true;
                ((XnaUITestGame)Game).Controller.OnSelectUnitToStop();
            }
        }

        /// <summary>
        /// Trigger attack when attack button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleAttackButtonClick(object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled)
            {
                //System.Console.Out.WriteLine("Attack button is clicked!");
                e.Handled = true;
            }

        }

        /// <summary>
        /// Trigger move command when move button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleMoveButtonClick(object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled)
            {
                //System.Console.Out.WriteLine("Move button is clicked!");
                e.Handled = true;
            }
        }

        // Move Button
        private void handleMoveButtonOver(object sender, EventArgs e)
        {
            moveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainMoveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleMoveButtonAway(object sender, EventArgs e)
        {
            moveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainMoveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleMoveButtonDown(object sender, EventArgs e)
        {
            moveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainMoveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleMoveButtonUp(object sender, EventArgs e)
        {
            moveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainMoveButton.setPicturebox(new Rectangle((GameConfig.BUTTON_MOVE + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        
        // Stop Button
        private void handleStopButtonOver(object sender, EventArgs e)
        {
            stopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainStopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleStopButtonAway(object sender, EventArgs e)
        {
            stopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainStopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleStopButtonDown(object sender, EventArgs e)
        {
            stopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainStopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleStopButtonUp(object sender, EventArgs e)
        {
            stopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainStopButton.setPicturebox(new Rectangle((GameConfig.BUTTON_STOP + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        // Build Button
        private void handleBuildButtonOver(object sender, EventArgs e)
        {
            buildButton.setPicturebox(new Rectangle((GameConfig.BUTTON_BUILD + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleBuildButtonAway(object sender, EventArgs e)
        {
            buildButton.setPicturebox(new Rectangle((GameConfig.BUTTON_BUILD) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleBuildButtonDown(object sender, EventArgs e)
        {
            buildButton.setPicturebox(new Rectangle((GameConfig.BUTTON_BUILD + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleBuildButtonUp(object sender, EventArgs e)
        {
            buildButton.setPicturebox(new Rectangle((GameConfig.BUTTON_BUILD + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        // Harvest Button
        private void handleHarvestButtonClick(object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled)
            {
                //System.Console.Out.WriteLine("Harvest button is clicked!");
                e.Handled = true;
                // TODO: Change this to reflect the mouseclick location where the unit needs to move and harvest
                ((XnaUITestGame)Game).Controller.OnSelectedUnitsToHarvest();
            }
        }

        private void handleharvestButtonOver(object sender, EventArgs e)
        {
            harvestButton.setPicturebox(new Rectangle((GameConfig.BUTTON_HARVEST + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        private void handleharvestButtonAway(object sender, EventArgs e)
        {
            harvestButton.setPicturebox(new Rectangle(GameConfig.BUTTON_HARVEST * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        private void handleharvestButtonDown(object sender, EventArgs e)
        {
            harvestButton.setPicturebox(new Rectangle((GameConfig.BUTTON_HARVEST + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        private void handleharvestButtonUp(object sender, EventArgs e)
        {
            harvestButton.setPicturebox(new Rectangle((GameConfig.BUTTON_HARVEST + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y_SECOND, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }


        // Attack Button
        private void handleAttackButtonOver(object sender, EventArgs e)
        {
            attackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainAttackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleAttackButtonAway(object sender, EventArgs e)
        {
            attackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainAttackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleAttackButtonDown(object sender, EventArgs e)
        {
            attackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainAttackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_PRESS) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }
        private void handleAttackButtonUp(object sender, EventArgs e)
        {
            attackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
            mainAttackButton.setPicturebox(new Rectangle((GameConfig.BUTTON_ATTACK + GameConfig.BUTTON_MOUSE_OVER) * GameConfig.BUTTON_DIM, GameConfig.BUTTON_START_Y, GameConfig.BUTTON_DIM, GameConfig.BUTTON_DIM));
        }

        private void handleUnitProduceButtonClick(object sender, XnaMouseEventArgs e)
        {
            if (Math.Abs(e.time) - lastClick > MSECONDS_PER_CLICK)
            {
                ((XnaUITestGame)Game).Controller.TellSelectedBuildingToBuild();
                e.Handled = true;
                lastClick = e.time;
            }
        }

        protected override void onDraw(XnaDrawArgs e)
        {

            //e.SpriteBatch.Draw(pixel, e.Location, new Rectangle(0, 0, 1, 1), color);
        }
    }
}
