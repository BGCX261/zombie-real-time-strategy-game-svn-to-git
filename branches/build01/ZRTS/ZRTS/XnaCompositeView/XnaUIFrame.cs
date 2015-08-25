using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// XNAUIFrame
    /// 
    /// This class represents the main user interface of the game.
    /// </summary>
    public class XnaUIFrame : XnaUIComponent
    {
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">game object</param>
        public XnaUIFrame(Game game) : base(game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            ((XnaUITestGame)game).Model.VictoryStateChangedHandler += onVictory;

        }

        /// <summary>
        /// Trigger event when victor state has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void onVictory(object sender, ZRTSModel.EventHandlers.GameVictoryStateChangeEventArgs e)
        {
            if (e.victoryState == ZRTSModel.GameModel.GameModel.GameVictoryState.PlayerWin)
            {
                ((XnaUITestGame)Game).state = XnaUITestGame.gameState.Win;
                AudioManager.play("music", "victory");
            }
            else if (e.victoryState == ZRTSModel.GameModel.GameModel.GameVictoryState.PlayerLost)
            {
                ((XnaUITestGame)Game).state = XnaUITestGame.gameState.Lose;
                AudioManager.play("music", "victory");
            }
        }

        public override SpriteBatch GetSpriteBatch(XnaUIComponent requester)
        {
            return spriteBatch;
        }

        protected override void onDraw(XnaDrawArgs e)
        {
        }

    }
}
