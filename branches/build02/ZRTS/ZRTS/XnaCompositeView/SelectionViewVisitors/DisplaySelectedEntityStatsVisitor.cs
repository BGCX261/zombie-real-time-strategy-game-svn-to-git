using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView.SelectionViewVisitors
{
    public class DisplaySelectedEntityStatsVisitor : NoOpModelComponentVisitor
    {
        public SameSizeChildrenFlowLayout Layout;
        public XnaUITestGame Game;
        public override void Visit(UnitComponent unit)
        {
            int desiredHeight = (int)Game.Font.MeasureString("anything").Y;
            int desiredWidth = Layout.DrawBox.Width / 2;
            
            TextBox textbox1 = new TextBox(Game, "HP:  ", "right");
            textbox1.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox1);
            TextBox textbox2 = new TextBox(Game, unit.CurrentHealth + " / " + unit.MaxHealth, "left");
            textbox2.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox2);

            TextBox textbox3 = new TextBox(Game, "Attack Damage:  ", "right");
            textbox3.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox3);
            TextBox textbox4 = new TextBox(Game, unit.Attack.ToString(), "left");
            textbox4.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox4);

            TextBox textbox5 = new TextBox(Game, "Attack Range:  ", "right");
            textbox5.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox5);
            TextBox textbox6 = new TextBox(Game, unit.AttackRange.ToString(), "left");
            textbox6.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox6);

            TextBox textbox7 = new TextBox(Game, "Attack Delay:  ", "right");
            textbox7.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox7);
            TextBox textbox8 = new TextBox(Game, unit.AttackTicks.ToString(), "left");
            textbox8.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox8);

            TextBox textbox9 = new TextBox(Game, "Attack Delay:  ", "right");
            textbox9.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox9);
            TextBox textbox10 = new TextBox(Game, unit.AttackTicks.ToString(), "left");
            textbox10.DrawBox = new Rectangle(0, 0, desiredWidth, desiredHeight);
            Layout.AddChild(textbox10);

        }
    }
}
