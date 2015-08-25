using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ZRTSModel.Scenario;
using ZRTSLogic;
namespace ZRTS
{
	/// <summary>
	/// This class will be used to keep track of the "state" of the user's input. Based on this state, it will select/deselect entities and 
	/// give commands to units.
	/// </summary>
	class InputHandler
	{
		private static InputHandler instance;

		public enum PlayerCommand
		{
			BUILD,
			ATTACK,
			SELECT,
			MOVE,
			CANCEL,
		}
		PlayerCommand currentPlayerCommand = PlayerCommand.CANCEL;
		MouseState prevInput;
		float commandX, commandY;
		float selectX, selectY;

		public static InputHandler Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new InputHandler();
				}
				return instance;
			}
		}

		public void updateInput(MouseState input, Controller testGameController, View gameView, ViewGamePlayMenu gamePlayMenu)
		{
			// Right click to give a command
			if (input.RightButton == ButtonState.Pressed && prevInput.RightButton == ButtonState.Released)
			{
				giveCommand(input, gameView, testGameController);	
			}


			if(gamePlayMenu.containsPoint(input.X, input.Y))
			{
				if (input.LeftButton == ButtonState.Pressed)
				{
					/** Handle Menu Input **/
					int button = gamePlayMenu.onButton(input.X, input.Y);
					//button corespons with the button pressed 0 to 3 left to right

                    if (button == 0)
                    {
                        currentPlayerCommand = PlayerCommand.CANCEL;
                    }
                    if (button == 1)
                    {
                        currentPlayerCommand = PlayerCommand.BUILD;
                    }
                    if (button == 2)
                    {
                        currentPlayerCommand = PlayerCommand.MOVE;
                    }
                    if (button == 3)
                    {
                        currentPlayerCommand = PlayerCommand.ATTACK;
                    }

				}
			}
			else
			{
				handleSelecting(input, testGameController, gameView, gamePlayMenu);
			}

			prevInput = input;
		}

		private void giveCommand(MouseState input, View gameView, Controller testGameController)
		{
			commandX = gameView.convertScreenLocToGameLoc(input.X, input.Y).X;
			commandY = gameView.convertScreenLocToGameLoc(input.X, input.Y).Y;


			if (testGameController.isWithinGameBound(commandX, commandY))
			{
				foreach (ZRTSModel.Entities.Entity entity in testGameController.scenario.getPlayer().SelectedEntities)
				{
					switch (currentPlayerCommand)
					{
						// Move command
						case PlayerCommand.MOVE:
							testGameController.giveActionCommand(entity,
						new ZRTSLogic.Action.MoveAction(commandX, commandY, testGameController.gameWorld, entity));
							break;


						// Cancel command
						case PlayerCommand.CANCEL:
                            // TODO: Cancel current action
							break;

						// Attack command
						case PlayerCommand.ATTACK:
							if (entity.entityType == ZRTSModel.Entities.Entity.EntityType.Unit)
							{
								ZRTSModel.Entities.Entity temp = testGameController.scenario.getUnit((int)commandX, (int)commandY);
								if (temp != null)
								{
									System.Console.Out.WriteLine("Selected Attack Unit at " + commandX + ":" + commandY);
									testGameController.giveActionCommand(entity,
											 new ZRTSLogic.Action.SimpleAttackAction((ZRTSModel.Entities.Unit)entity, temp));
								}
							}
							break;

						// Build command
						case PlayerCommand.BUILD:
							if (testGameController.makeUnitBuild(entity,
						new ZRTSModel.Entities.Building(testGameController.scenario.getPlayer(), new ZRTSModel.Entities.BuildingStats()),
						testGameController.gameWorld.map.getCell((int)commandX, (int)commandY)))
							{
								System.Console.Out.WriteLine("Building at " + commandX + ":" + commandY);
							}
							else
							{
								System.Console.Out.WriteLine("Can't place a building at " + commandX + ":" + commandY);
							}
							break;
					}

				}
			}
		}

		private void handleSelecting(MouseState input, Controller testGameController, View gameView, ViewGamePlayMenu gamePlayMenu)
		{
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
					testGameController.scenario.getUnits(
						(int)Math.Min(pressX, releaseX),
						(int)Math.Min(pressY, releaseY),
						(int)(Math.Max(pressX, releaseX) - Math.Min(pressX, releaseX)),
						(int)(Math.Max(pressY, releaseY) - Math.Min(pressY, releaseY))
					);
				}

				gameView.IsDragging = false;
				//gameView.resetDragBox();

			}

		}
	}
}
