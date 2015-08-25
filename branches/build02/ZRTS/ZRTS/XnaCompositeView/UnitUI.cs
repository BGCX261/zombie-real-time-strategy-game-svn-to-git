using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZRTSModel;
using ZRTSModel.EventHandlers;
using ZRTS.InputEngines;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// This class will represent a view (graphical) represention of the UnitComponent (Unit game model)
    /// 
    /// Event-handling based system/pattern
    /// </summary>
    public class UnitUI : PictureBox
    {
        private UnitComponent unit;
		bool selected = false;

        private int currentFrame = 0;       // Current frame reference
        private float currentElapsedTime = 0f;      // Current time
		Texture2D pixel;
        private int unitType;       // Type frame

        bool playDyingSound = false;

        public UnitComponent Unit
        {
            get { return unit; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">Game component</param>
        /// <param name="unit">Unit Model</param>
        /// <param name="sourceRect">Location of the image representing this unit on the spritesheet</param>
        public UnitUI(Game game, UnitComponent unit, Rectangle sourceRect)
            : base(game, sourceRect)
        {
            this.unit = unit;
            this.OnClick += getAttacked;

			unit.SelectHandler += new ModelComponentSelectedHandler(onSelectChanged);
			unit.UnitStateChangedHandlers += new UnitStateChangedHanlder(onUnitStateChanged);
            unit.UnitAttackedEnemyHanlders += new UnitAttackedEnemyHandler(onAttack);

			pixel = new Texture2D(game.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });

            
            if (unit.IsZombie)
            {
                unitType = GameConfig.ZOMBIE_START_Y;
            }
            else
            {
                if (unit.Type.Equals("soldier"))
                {
                    unitType = GameConfig.SOLDIER_START_Y;
                }
                else
                {
                    unitType = GameConfig.WORKER_START_Y;
                }
            }

        }

        /// <summary>
        /// Triggered if this unit is being attacked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getAttacked(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled && e.ButtonPressed == MouseButton.Right)
            {
                ((XnaUITestGame)Game).Controller.TellSelectedUnitsToAttack(unit);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Triggered if this unit attacks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onAttack(Object sender, UnitAttackedEnemyArgs e)
        {
            if (Unit.Type.Equals("soldier"))
                AudioManager.play("attack", "soldier");
            else if (Unit.Type.Equals("worker"))
                AudioManager.play("attack", "worker");
            else if (Unit.Type.Equals("zombie"))
                AudioManager.play("attack", "zombie");
        }

        /// <summary>
        /// Manually change picture by specified pixel
        /// </summary>
        /// <param name="x">X location on the spritesheet</param>
        /// <param name="y">y location on the spritesheet</param>
        private void changePicture(int x, int y)
        {
            this.SourceRect = new Rectangle(x, y, this.SourceRect.Width, this.SourceRect.Height);
        }

        /// <summary>
        /// Manually change picture by specified a pair of indices
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        private void changePictureByFrame(int col, int row)
        {
            this.SourceRect = new Rectangle(col * this.SourceRect.Width, row * this.SourceRect.Height, this.SourceRect.Width, this.SourceRect.Height);
        }

        /// <summary>
        /// Change picture/image frame based on state and direction
        /// </summary>
        private void changePicture()
        {
            if (unit.State == UnitComponent.UnitState.ATTACKING)
            {
                changePicture(SourceRect.X, unitType + GameConfig.ACTION_ATTACK);
            }
            else if (unit.State == UnitComponent.UnitState.MOVING || unit.State == UnitComponent.UnitState.IDLE)
            {
                changePicture(SourceRect.X, unitType + GameConfig.ACTION_MOVE * GameConfig.UNIT_HEIGHT);
            }
            else if (unit.State == UnitComponent.UnitState.DEAD)
            {
                changePicture(SourceRect.X, unitType + GameConfig.ACTION_DEAD * GameConfig.UNIT_HEIGHT);
                if (Unit.Type.Equals("worker") && playDyingSound == false)
                {
                    AudioManager.play("dead", "worker");
                    playDyingSound = true;
                }
                else if (Unit.Type.Equals("zombie") && playDyingSound == false)
                {
                    AudioManager.play("dead", "zombie");
                    playDyingSound = true;
                }
                else if (Unit.Type.Equals("soldier") && playDyingSound == false)
                {
                    AudioManager.play("dead", "soldier");
                    playDyingSound = true;
                }
            }
            else if (unit.State == UnitComponent.UnitState.HARVESTING)
            {
                // Need differnt type of harvesting 
                changePicture(SourceRect.X, unitType + GameConfig.ACTION_HARVEST * GameConfig.UNIT_HEIGHT);
            }

            changePictureByOrientation();

        }

        /// <summary>
        /// change a picture by orientation
        /// </summary>
        private void changePictureByOrientation()
        {
            if (unit.UnitOrient == UnitComponent.Orient.N)
            {
                changePicture((GameConfig.DIR_N + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.S)
            {
                changePicture((GameConfig.DIR_S + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.E)
            {
                changePicture((GameConfig.DIR_E + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.W)
            {
                changePicture((GameConfig.DIR_W + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.NE)
            {
                changePicture((GameConfig.DIR_NE + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.SE)
            {
                changePicture((GameConfig.DIR_SE + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.NW)
            {
                changePicture((GameConfig.DIR_NW + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
            else if (unit.UnitOrient == UnitComponent.Orient.SW)
            {
                changePicture((GameConfig.DIR_SW + currentFrame) * GameConfig.UNIT_WIDTH, SourceRect.Y);
            }
        }

        /// <summary>
        /// Update animation
        /// </summary>
        private void updateAnimation()
        {

            if (currentFrame > 3)
            {
                currentFrame = 0;   //reset 
            }

            changePicture();

            if ((unit.State != UnitComponent.UnitState.DEAD && unit.CurrentHealth > 0) || (unit.State == UnitComponent.UnitState.DEAD && currentFrame < 3))
            {
                ++currentFrame;
            }
        }

        /// <summary>
        /// Draw stat of this unit if being selected
        /// </summary>
        /// <param name="e"></param>
		protected override void onDraw(XnaDrawArgs e)
		{
			base.onDraw(e);
			if (selected && !unit.IsZombie)
			{
				// Draw a healthbox for the Unit.
				Rectangle healthBG = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height - 8, e.Location.Width, 8);
				e.SpriteBatch.Draw(pixel, healthBG, Color.Black);

				int healthWidth = (int)( e.Location.Width * (1.0 * unit.CurrentHealth / unit.MaxHealth));
				Color healthColor = Color.LimeGreen;

				if (1.0 * unit.CurrentHealth / unit.MaxHealth < 0.25)
				{
					healthColor = Color.Red;
				}
				else if (1.0 * unit.CurrentHealth / unit.MaxHealth < 0.5)
				{
					healthColor = Color.Yellow;
				}
				Rectangle healthRect = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height - 8, healthWidth, 8);
				e.SpriteBatch.Draw(pixel, healthRect, healthColor);

				Color neoGreen = new Color(111, 245, 30);

				// Draw a Rectangle around the unit to show that it is selected.
				Rectangle leftRect = new Rectangle(e.Location.X, e.Location.Y, 2, e.Location.Height);
				Rectangle rightRect = new Rectangle(e.Location.X + e.Location.Width -2, e.Location.Y, 2, e.Location.Height);
				Rectangle topRect = new Rectangle(e.Location.X, e.Location.Y, e.Location.Width, 2);
				Rectangle bottomRect = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height -2, e.Location.Width, 2);
				e.SpriteBatch.Draw(pixel, leftRect, neoGreen);
				e.SpriteBatch.Draw(pixel, topRect, neoGreen);
				e.SpriteBatch.Draw(pixel, rightRect, neoGreen);
				e.SpriteBatch.Draw(pixel, bottomRect, neoGreen);
			}
		}

        /// <summary>
        /// Update this unit's view
        /// </summary>
        /// <param name="gameTime">game time object</param>
        public override void Update(GameTime gameTime)
        {
            currentElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            
            // Update animation every 1/4 second
            if (currentElapsedTime > 100)
            {
                updateAnimation();
                currentElapsedTime = 0;
            }
            base.Update(gameTime);
            
        }

        /// <summary>
        /// Trigger function when user selection has change
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="selected"></param>
		public void onSelectChanged(Object obj, bool selected)
		{
			this.selected = selected;
		}

        /// <summary>
        /// Update unit's image once the state has changed
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
		public void onUnitStateChanged(Object obj, UnitStateChangedEventArgs args)
		{
			currentFrame = 0;
			updateAnimation();
		}

        /// <summary>
        /// Update unit's image once the unit has changed its direction
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
		public void onUnitOrientationChange(Object obj, UnitOrientationChangedEventArgs args)
		{
			currentFrame = 0;
			updateAnimation();
		}
    }
}
