using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSModel.Entities;
using ZRTS;
using Microsoft.Xna.Framework;
using ZRTS.InputEngines;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// ProduceUnitButton
    /// 
    /// This class creates an image representation of the produce unit button for the game menu.
    /// </summary>
	public class ProduceUnitButton : PictureBox
	{
		TextBox unitTypeTextBox;
		SameSizeChildrenFlowLayout layout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        /// <param name="sourceRect">Location of the button</param>
        /// <param name="text">Text dispalyed</param>
		public ProduceUnitButton(Game game, Rectangle sourceRect, string text) : base(game, sourceRect)
		{
			layout = new SameSizeChildrenFlowLayout(game);
			unitTypeTextBox = new TextBox(game, text, "left", Color.Red);
			unitTypeTextBox.DrawBox = new Rectangle(0, 0, 76, 20);
			AddChild(unitTypeTextBox);

			this.OnClick += new ClickEventHandler(handleClick);
		}

		private string unitType = "";

		public string UnitType
		{
			get { return this.unitType; }
			set
			{
				this.unitType = value;
				unitTypeTextBox.Text = this.unitType;
			}
		}

        /// <summary>
        /// Event listener when the button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="agrs"></param>
		public void handleClick(object sender, XnaMouseEventArgs agrs)
		{
			((XnaUITestGame)Game).Controller.TellSelectedBuildingToBuild();
		}
		
	}
}
