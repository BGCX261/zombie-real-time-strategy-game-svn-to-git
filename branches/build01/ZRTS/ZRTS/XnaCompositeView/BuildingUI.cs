using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework;
using ZRTS.InputEngines;
using Microsoft.Xna.Framework.Graphics;
namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// Building UI (View object for building model)
    /// </summary>
    public class BuildingUI : PictureBox
    {
        private Building building;
		bool selected = false;
		Texture2D pixel;

        public Building Building
        {
            get { return building; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="building">Building model</param>
        /// <param name="sourceRect">Location on the spritesheet</param>
        public BuildingUI(Game game, Building building, Rectangle sourceRect)
            : base(game, sourceRect)
        {
            
            this.building = building;
            //this.OnClick += getAttacked;
			pixel = new Texture2D(game.GraphicsDevice, 1, 1, true, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
			this.building.SelectHandler += new ZRTSModel.EventHandlers.ModelComponentSelectedHandler(onSelectChanged);

            if (building.Type.Equals("barracks"))
            {
                this.SourceRect = new Rectangle(GameConfig.BUILDING_CONSTRUCTION, GameConfig.BUILDING_START_Y, 216, 216);
            }
            else if (building.Type.Equals("hospital"))
            {
                this.SourceRect = new Rectangle(GameConfig.BUILDING_CONSTRUCTION, GameConfig.HOSPITAL_START_Y, 216, 216);
            }
            else if (building.Type.Equals("house"))
            {
                this.SourceRect = new Rectangle(GameConfig.BUILDING_CONSTRUCTION, GameConfig.HOUSE_START_Y, 216, 216);
            }
        }

/**
        private void getAttacked(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled && !e.Handled && e.ButtonPressed == MouseButton.Right)
            {
                ((XnaUITestGame)Game).Controller.TellSelectedUnitsToAttack(building);
                e.Handled = true;
            }
        }
        **/
		private void onSelectChanged(Object obj, bool selected)
		{
			this.selected = selected;
		}


        /// <summary>
        /// Update animation
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (building.CurrentHealth == building.MaxHealth)
            {
                this.SourceRect = new Rectangle(GameConfig.BUILDING_FINISH* GameConfig.BUILDING_DIM, SourceRect.Y , 216, 216);
            }
            else if (building.CurrentHealth > building.MaxHealth / 2)
            {
                this.SourceRect = new Rectangle(GameConfig.BUILDING_HALF_FINISH * GameConfig.BUILDING_DIM, SourceRect.Y , 216, 216);
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw stat of the selected building
        /// </summary>
        /// <param name="e"></param>
		protected override void onDraw(XnaDrawArgs e)
		{
			base.onDraw(e);

			if (selected)
			{
				// Draw a healthbox for the Building.
				Rectangle healthBG = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height - 8, e.Location.Width, 8);
				e.SpriteBatch.Draw(pixel, healthBG, Color.Black);

				int healthWidth = (int)(e.Location.Width * (1.0 * building.CurrentHealth / building.MaxHealth));
				Color healthColor = Color.LimeGreen;

				if (1.0 * building.CurrentHealth / building.MaxHealth < 0.25)
				{
					healthColor = Color.Red;
				}
				else if (1.0 * building.CurrentHealth / building.MaxHealth < 0.5)
				{
					healthColor = Color.Yellow;
				}
				Rectangle healthRect = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height - 8, healthWidth, 8);
				e.SpriteBatch.Draw(pixel, healthRect, healthColor);
				Color neoGreen = new Color(111, 245, 30);

				// Draw a Rectangle around the building to show that it is selected.
				Rectangle leftRect = new Rectangle(e.Location.X, e.Location.Y, 2, e.Location.Height);
				Rectangle rightRect = new Rectangle(e.Location.X + e.Location.Width - 2, e.Location.Y, 2, e.Location.Height);
				Rectangle topRect = new Rectangle(e.Location.X, e.Location.Y, e.Location.Width, 2);
				Rectangle bottomRect = new Rectangle(e.Location.X, e.Location.Y + e.Location.Height - 2, e.Location.Width, 2);
				e.SpriteBatch.Draw(pixel, leftRect, neoGreen);
				e.SpriteBatch.Draw(pixel, topRect, neoGreen);
				e.SpriteBatch.Draw(pixel, rightRect, neoGreen);
				e.SpriteBatch.Draw(pixel, bottomRect, neoGreen);
			}
		}
    }
}
