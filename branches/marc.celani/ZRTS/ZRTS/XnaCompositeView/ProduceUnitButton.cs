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
	public class ProduceUnitButton : PictureBox
	{
		Building building;
		UnitStats unitStats;
		TextBox unitTypeTextBox;
		TextBox unitCountTextBox;
		SameSizeChildrenFlowLayout layout;

		public ProduceUnitButton(Game game, Rectangle sourceRect, string text) : base(game, sourceRect)
		{
			layout = new SameSizeChildrenFlowLayout(game);
			unitTypeTextBox = new TextBox(game, text, "left", Color.Red);
			unitTypeTextBox.DrawBox = new Rectangle(0, 0, 76, 20);
			AddChild(unitTypeTextBox);

			//unitCountTextBox = new TextBox(game);
			//AddChild(unitCountTextBox);

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

		public void handleClick(object sender, XnaMouseEventArgs agrs)
		{
			((XnaUITestGame)Game).Controller.TellSelectedBuildingToBuild();
		}
		
	}
}
