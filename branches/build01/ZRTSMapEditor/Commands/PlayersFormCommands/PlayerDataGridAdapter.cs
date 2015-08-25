using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using System.Data;

namespace ZRTSMapEditor
{
    /// <summary>
    /// Adapts PlayerComponent class to a MapEditorCommand, that command being to allow for changes to be made to the player before commiting them on Do().
    /// This class is used by the PlayersForm to make changes to and display a Player before commiting those changes to the model on Submit.
    /// It cannot be undone.
    /// Because this command tends to be run in succession with others (All commit at once), it is difficult to validate on CanBeDone whether or not
    /// the model will become invalidated.  For example, if several will run at once, then it is impossible to test whether or not names will be duplicated.
    /// Therefore, these commands should be run with a PlayerDataGridAdapterCommitter.
    /// </summary>
    public class PlayerDataGridAdapter : MapEditorCommand
    {
        // Data that we represent within our view.  This data is edited within this class, and the changes are made to the model in a single Commit on Do().
        private string name;
        private string race;
        private int gold;
        private int wood;
        private int metal;
        
        // Booleans that mark whether or not this is a new player, or if we are removing this player from the player list.
        private bool added;
        private bool removed;

        // The actual player that we are representing
        private PlayerComponent data;
        
        // The actual player list that we are editing.
        private PlayerList playerList;

        private PlayerDataGridAdapter()
        { }

        public PlayerDataGridAdapter(PlayerComponent player, PlayerList list)
        {
            data = player;
            playerList = list;

            name = player.Name;
            race = player.Race;
            gold = player.Gold;
            wood = player.Wood;
            metal = player.Metal;
            added = false;
        }

        public string Player_Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string RaceMember
        {
            get
            {
                return race;
            }
            set
            {
                race = value;
            }
        }

        public int GoldMember
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
            }
        }

        public int WoodMember
        {
            get
            {
                return wood;
            }
            set
            {
                wood = value;
            }
        }

        public int MetalMember
        {
            get
            {
                return metal;
            }
            set
            {
                metal = value;
            }
        }

        public bool AddedMember
        {
            get
            {
                return added;
            }
            set
            {
                added = value;
            }
        }

        public bool RemovedMember
        {
            get
            {
                return removed;
            }
            set
            {
                removed = value;
            }
        }

        /// <summary>
        /// Updates the actual model to reflect the changes in the UI.
        /// </summary>
        public void Do()
        {
            if (CanBeDone())
            {
                if (!removed)
                {
                    data.Name = name;
                    data.Race = race;
                    data.Gold = gold;
                    data.Wood = wood;
                    data.Metal = metal;
                    if (added)
                    {
                        playerList.AddChild(data);
                    }
                }
                else
                {
                    playerList.RemoveChild(data);
                }
            }
        }

        /// <summary>
        /// This command is not meant to be used with the CommandStack - it is only done, and not undone.
        /// </summary>
        public void Undo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ensures that the player name is valid (in itself - does not ensure that the name is not in conflict with other player names).  Ensures that the
        /// race is valid.  Ensures that all resources are nonnegative.
        /// </summary>
        /// <returns></returns>
        public bool CanBeDone()
        {
            return ((name != null) && !name.Replace(" ","").Replace("\t", "").Equals("") && (gold >= 0) && (wood >= 0) && (metal >= 0) && (race != null) && ((race.Equals("Human")) || (race.Equals("Zombie"))));
        }
    }
}
