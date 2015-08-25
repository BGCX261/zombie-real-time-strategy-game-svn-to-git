using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// This class represents health state of the unit.
    /// </summary>
    public class HPBar : XnaUIComponent
    {
        private int maxHP;
        private int currentHP;

        public int MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public int CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        public HPBar(Game game)
            : base(game)
        {
        }

        protected override void onDraw(XnaDrawArgs e)
        {
            e.SpriteBatch.Draw(((XnaUITestGame)Game).SpriteSheet, e.Location, new Rectangle(0, 0, 1, 1), Color.Green);
        }

        

    }
}
