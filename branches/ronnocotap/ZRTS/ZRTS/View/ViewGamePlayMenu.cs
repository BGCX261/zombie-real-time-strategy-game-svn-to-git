using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTS;

namespace ZRTS
{
    public class ViewGamePlayMenu
    {

        private SpriteSheet gamePlaySprite;
        private SpriteSheet gamePlayIconSprite;

        protected ZRTSModel.Scenario.Scenario scenario;

        private int width, height;                  // Width and Height of the screen
        private Microsoft.Xna.Framework.Vector2 location;
        private Microsoft.Xna.Framework.Vector2 iconLocation;
        int buttonWidth;
        int buttonHeight;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public ViewGamePlayMenu(int width, int height, SpriteSheet sheet)
        {
            this.width = width;
            this.height = height;
            location = Microsoft.Xna.Framework.Vector2.Zero;
            loadGamePlaySprite(sheet);
            location.X = width - gamePlaySprite.frameDimX;
            location.Y = height - gamePlaySprite.frameDimY;
            
        }

        /// <summary>  //this probably isn't in the right place, it is used in control
        /// returns 0-3 if the mouse is on one of the buttons
        /// -1 if not
        /// </summary>
        /// <param name="x">mouse x pos</param>
        /// <param name="y">mouse y pos</param>
        /// <returns></returns>
        public int onButton(int x, int y)
        {
            if (scenario.getPlayer().SelectedEntities.Count != 0)  //change this if there are different menus for different entity types
            {
                if (y < iconLocation.Y || y > iconLocation.Y + buttonHeight)
                    return -1;
                if (x > iconLocation.X && x < iconLocation.X + buttonWidth)
                    return 0;
                if (x > iconLocation.X + buttonWidth && x < iconLocation.X + 2 * buttonWidth)
                    return 1;
                if (x > iconLocation.X + 2 * buttonWidth && x < iconLocation.X + 3 * buttonWidth)
                    return 2;
                if (x > iconLocation.X + 3 * buttonWidth && x < iconLocation.X + 4 * buttonWidth)
                    return 3;
            }
            return -1;
        }

		public bool containsPoint(int x, int y)
		{
			return (x >= location.X && x < location.X + width && y >= location.Y && y < location.Y + height);
		}

        /// <summary>
        /// Load sprite sheet of the menu
        /// </summary>
        /// <param name="sheet"></param>
        public void loadGamePlaySprite(SpriteSheet sheet)
        {
            gamePlaySprite = sheet;

            iconLocation = Microsoft.Xna.Framework.Vector2.Zero;           
            iconLocation.X = width - (.91f*gamePlaySprite.frameDimX);
            iconLocation.Y = height - (.82f*gamePlaySprite.frameDimY);
            
        }

        /// <summary>
        /// Load sprite sheet for all icons corresponding to all type of entities
        /// </summary>
        /// <param name="sheet"></param>
        public void loadIconSprite(SpriteSheet sheet)
        {
            gamePlayIconSprite = sheet;
            buttonWidth = gamePlayIconSprite.frameDimX / 4;
            buttonHeight = gamePlayIconSprite.frameDimY;
        }

        /// <summary>
        /// Activate Entity Menu (based on entity type)
        /// NEED TO REIMPLEMENT FOR THE FUTURE TO ACCOMODATE ENTITY TYPE
        /// </summary>
        public void activateEntityMenu()
        {
            
        }

        public void deactivateEntityMenu()
        {

        }

        /// <summary>
        /// Return a command corresponding to the clicked icon
        /// </summary>
        /// <returns></returns>
        public PlayerCommand getCommand()
        {
            return PlayerCommand.MOVE;
        }


        /// <summary>
        /// Loading game scenario object for process
        /// </summary>
        /// <param name="scene"></param>
        public void LoadScenario(ZRTSModel.Scenario.Scenario scene)
        {
            // Need not to load this scenario into View
            // Basically just return a command corresponding to the clicked icon
            // i.e. click at attack icon, will return attack command and then process the game logic in the gameloop inside ZRTS update();



            this.scenario = scene;
        }


        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            gamePlaySprite.drawAtCurrentIndex(location);
            if(scenario.getPlayer().SelectedEntities.Count!=0)
                gamePlayIconSprite.drawAtCurrentIndex(iconLocation);
        }

    }
}
