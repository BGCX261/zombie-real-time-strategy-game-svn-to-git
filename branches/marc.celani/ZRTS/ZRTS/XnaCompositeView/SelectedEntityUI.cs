using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework;
using ZRTSModel.EventHandlers;
using ZRTS.XnaCompositeView.UIEventHandlers;
using ZRTS.InputEngines;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{

    /// <summary>
    /// Selected Game Entity by the user
    /// This class will keep track of unit(s) and building(s) being selected by the user on the screen.
    /// </summary>
    public class SelectedEntityUI : XnaUIComponent
    {
        private UnitComponent unit;
        private Building building;


        /// <summary>
        /// Constructor for selected units
        /// </summary>
        /// <param name="game"></param>
        /// <param name="unit"></param>
        public SelectedEntityUI(Game game, UnitComponent unit)
            : base(game)
        {
            this.unit = unit;
            unit.HPChangedEventHandlers += UpdateHPBar;
            this.SizeChanged += onResize;
            this.OnClick += selectUnit;
        }

        /// <summary>
        /// Constructor for selected building
        /// </summary>
        /// <param name="game"></param>
        /// <param name="building"></param>
        public SelectedEntityUI(Game game, Building building)
            : base(game)
        {
            this.building = building;
            this.SizeChanged += onResize;
            this.OnClick += selectBuilding;
        }

        /// <summary>
        /// Trigger when the health information of the unit has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void UpdateHPBar(Object sender, UnitHPChangedEventArgs args)
        {
            HPBar hpBar = getHPBar();
            hpBar.CurrentHP = args.NewHP;
        }

        private HPBar getHPBar()
        {
            HPBar hpBar = null;
            foreach (XnaUIComponent child in GetChildren())
            {
                if (child is HPBar)
                {
                    hpBar = (HPBar)child;
                    break;
                }
            }
            return hpBar;
        }

        /// <summary>
        /// Remove HP bar information and unregister event handler
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (unit != null)
                {
                    unit.HPChangedEventHandlers -= UpdateHPBar;
                }
                else if (building != null)
                {

                }
            }
            base.Dispose(disposing);
        }

        protected override void onDraw(XnaDrawArgs e)
        {
			Texture2D pixel = new Texture2D(e.SpriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			pixel.SetData(new[] { Color.White });
            //e.SpriteBatch.Draw(pixel, e.Location, new Rectangle(0, 0, 1, 1), Color.Teal);
        }

        private void onResize(Object sender, UISizeChangedEventArgs e)
        {
            // Update the position of the scrollbar.
            HPBar hpBar = getHPBar();
            if (hpBar != null)
            {
                int hpBarHeight = hpBar.DrawBox.Height;
                int hpBarMargin = 3;
                int pictureBoxDimension = e.DrawBox.Height - hpBarHeight - 3 * hpBarMargin;
                hpBar.DrawBox = new Rectangle(hpBar.DrawBox.X, e.DrawBox.Height - hpBarHeight - hpBarMargin, e.DrawBox.Width - (2 * hpBar.DrawBox.X), hpBarHeight);
                // Get picturebox
                TestUIComponent pictureBox = null;
                foreach (XnaUIComponent component in GetChildren())
                {
                    if (component is TestUIComponent)
                    {
                        pictureBox = (TestUIComponent)component;
                        break;
                    }
                }
                if (pictureBox != null)
                {
                    pictureBox.DrawBox = new Rectangle((e.DrawBox.Width - pictureBoxDimension) / 2, 3, pictureBoxDimension, pictureBoxDimension);
                }
            }
        }

        private void selectUnit(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled)
            {
                List<ModelComponent> entities = new List<ModelComponent>();
                entities.Add(unit);
                ((XnaUITestGame)Game).Controller.SelectEntities(entities);
                e.Handled = true;
            }
        }

        private void selectBuilding(Object sender, XnaMouseEventArgs e)
        {
            if (e.Bubbled)
            {
                List<ModelComponent> entities = new List<ModelComponent>();
                entities.Add(building);
                ((XnaUITestGame)Game).Controller.SelectEntities(entities);
                e.Handled = true;
            }
        }
    }
}
