using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Player;
using ZRTSModel.GameWorld;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Represents everything needed to define a "level" or instance of a game. Includes Players, the GameWorld, and
    /// Triggers.
    /// </summary>
    [Serializable()]
    public class Scenario:Observable
    {
        public  const byte WORLD_PLAYER = 0;
        public  const byte PLAYER = 1;
        public  const byte ZOMBIE_PLAYER = 2;

        Player.Player player;       // Human Player
        Player.Player worldPlayer;  // "World" Player (Neutral)
        Player.Player zombiePlayer; // Zombie Player (enemy)

        GameWorld.GameWorld gameWorld;

        // Dimensions of the game space.
        int width, height;

        public Scenario(int width, int height)
        {
            this.width = width;
            this.height = height;
            worldPlayer = new Player.Player(WORLD_PLAYER);
            player = new Player.Player(PLAYER);
            zombiePlayer = new Player.Player(ZOMBIE_PLAYER);
            gameWorld = new GameWorld.GameWorld(width, height);
        }

        public Player.Player getPlayer() { return this.player; }
        public Player.Player getWorldPlayer() { return this.worldPlayer; }
        public Player.Player getZombiePlayer() { return this.zombiePlayer; }
        public GameWorld.GameWorld getGameWorld() { return this.gameWorld; }

        /// <summary>
        /// Getting all units in scenario
        /// </summary>
        /// <returns>List of all units</returns>
        public List<Entities.Unit> getUnits()
        {
            return this.gameWorld.getUnits();
        }

      
        /// <summary>
        /// Return a single unit at a particular cell on the map
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>Unit object</returns>
        public Entities.Entity getUnit(int col, int row)
        {
            return this.gameWorld.map.getCell(col, row).getUnit();
        }

        /// <summary>
        /// Return all units within a boundary
        /// </summary>
        /// <param name="s_col">Starting col</param>
        /// <param name="s_row">Starting row</param>
        /// <param name="xoffset">Column offset</param>
        /// <param name="yoffset">Row offset</param>
        /// <returns>List of all units within that boundary</returns>
        public void getUnits(int s_col, int s_row, int xoffset, int yoffset)
        {
            // Create a temporary selected list
            List<ZRTSModel.Entities.Entity> unitList = new List<ZRTSModel.Entities.Entity>();

            for (int row = s_row; row <= s_row + yoffset; ++row)
            {
                for (int col = s_col; col <= s_col + xoffset; ++col)
                {
                    if (this.getUnit(col, row) != null)
                    {
                        unitList.Add(this.getUnit(col, row));
                    }
                }//for
            }//for

            // Only change the selection if there are new units to be selected.  The only way to actually be selecting
            // nothing is if the selected units die or if the game has just started.
            if (unitList.Count > 0)
            {
                viewSelectObserver.getSelectedUnits = unitList;
                player.selectEntities(viewSelectObserver.getSelectedUnits);
            }

            this.notify();
        }

        /// <summary>
        /// Insert an entity into a player
        /// </summary>
        /// <param name="entity">Added entity</param>
        public void insertEntityIntoPlayer(Entities.Entity entity)
        {
            Player.Player owner = entity.getOwner();
            if (owner == worldPlayer)
            {
                this.worldPlayer.insertEntity(entity);
            }
            else if (owner == player)
            {
                this.player.insertEntity(entity);
            }
            else if (owner == zombiePlayer)
            {
                this.zombiePlayer.insertEntity(entity);
            }
        }

        /// <summary>
        /// Remove entity from a player
        /// </summary>
        /// <param name="entity">Removed entity</param>
        public void removeEntityFromPlayer(Entities.Entity entity)
        {
            Player.Player owner = entity.getOwner();
            if (owner == worldPlayer)
            {
                this.worldPlayer.removeEntity(entity);
            }
            else if (owner == player)
            {
                this.player.removeEntity(entity);
            }
            else if (owner == zombiePlayer)
            {
                this.zombiePlayer.removeEntity(entity);
            }

        }
    }
}
