using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor
{
    /// <summary>
    /// A class that commits all of the changes made in the PlayerForm to the model.  This command ensures that player names do not conflict with
    /// one another, and that all individual players are left in valid state.
    /// </summary>
    public class PlayerDataGridAdapterCommitter : MapEditorCommand
    {
        private List<PlayerDataGridAdapter> adapters;
        private PlayerList playerList;

        /// <summary>
        /// Ensures that a list of adapters are given to the committer.
        /// </summary>
        private PlayerDataGridAdapterCommitter()
        {
        }

        public PlayerDataGridAdapterCommitter(List<PlayerDataGridAdapter> adapters, PlayerList playerList)
        {
            this.adapters = adapters;
            this.playerList = playerList;
        }

        /// <summary>
        /// Delegates the command to the adapters.
        /// </summary>
        public void Do()
        {
            if (CanBeDone())
            {
                foreach (PlayerDataGridAdapter adapter in adapters)
                {
                    adapter.Do();
                }
                playerList.FireChangedEvent();
            }
        }

        /// <summary>
        /// This command is not meant to be used with the CommandStack, and cannot be undone.
        /// </summary>
        public void Undo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that each adapter can be committed, and that each player name is not in conflict with any others.
        /// </summary>
        /// <returns></returns>
        public bool CanBeDone()
        {
            bool canBeDone = true;

            // A dictionary to keep track of all names seen so far.
            Dictionary<string, Boolean> names = new Dictionary<string, Boolean>();
            
            // A tally of the number of removed players.
            int removed = 0;

            foreach (PlayerDataGridAdapter adapter in adapters)
            {
                if (adapter.RemovedMember)
                {
                    removed++;
                }
                else
                {
                    // Ensure that the player is left in good state in and of itself.
                    canBeDone = canBeDone && adapter.CanBeDone();
                    if (!canBeDone)
                    {
                        break;
                    }
                    // Add the name to the dictionary.
                    names[adapter.Player_Name] = true;
                }
            }
            if (canBeDone)
            {
                // Ensure that the number of names in the dictionary is 1 for each not removed player.  If there are less, then 
                // there exists a name conflict.
                canBeDone = (names.Count == (adapters.Count - removed));
            }
            return canBeDone;
        }
    }
}
