using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSModel;
using ZRTSMapEditor;

namespace ZRTSNUnitTests
{
    [TestFixture]
    class TestPlayerDataGridAdapter
    {
        private PlayerList playerList;
        private PlayerDataGridAdapter adapter;

        private PlayerComponent buildValidPlayer()
        {
            PlayerComponent player = new PlayerComponent();
            player.Name = "Player 2";
            player.Race = "Zombie";
            player.Gold = 100;
            player.Metal = 100;
            player.Wood = 100;
            return player;
        }
        [SetUp]
        public void Init()
        {
            playerList = new PlayerList();
            adapter = new PlayerDataGridAdapter(new PlayerComponent(), playerList);
            adapter.Player_Name = "Player 1";
            adapter.RaceMember = "Human";
            adapter.GoldMember = 10;
            adapter.MetalMember = 20;
            adapter.WoodMember = 0;
            adapter.RemovedMember = false;
            adapter.AddedMember = true;
        }

        [Test]
        public void TestCanDoWithValidInput()
        {
            Assert.AreEqual(true, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize when valid input can be done.");
        }

        [Test]
        public void TestCanDoWithInvalidName()
        {
            // Test empty string is invalid
            adapter.Player_Name = "";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid names cannot be done.");
            
            // Test white space string is invalid.
            adapter.Player_Name = " ";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid names cannot be done.");
          
            // Test tab space string is invalid.
            adapter.Player_Name = "\t";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid names cannot be done.");
          
            // Test mix of many is invalid.
            adapter.Player_Name = "  \t  ";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid names cannot be done.");
          
        }

        [Test]
        public void TestCanDoWithInvalidRace()
        {
            // Test empty string is invalid
            adapter.RaceMember = "";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid races cannot be done.");

            // Test invalid race name
            adapter.RaceMember = "InvalidRaceName";
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid races cannot be done.");
        }

        [Test]
        public void TestCanDoWithInvalidResources()
        {
            // Test negative gold
            adapter.GoldMember = -1;
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid gold cannot be done.");
            adapter.GoldMember = 0;

            // Test negative metal
            adapter.MetalMember = -1;
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid metal cannot be done.");
            adapter.MetalMember = 0;

            // Test negative wood
            adapter.WoodMember = -1;
            Assert.AreEqual(false, adapter.CanBeDone(), "PlayerDataGridAdapter does not recognize that invalid wood cannot be done.");
        }

        [Test]
        public void TestDoAddMemberWithValidInput()
        {
            // Note that typically, Do is called by a PlayerDataGridAdapterCommitter, but for the sake of unit testing, we test only Do() on
            // one adapter.  It is the responsibility of the committer class to ensure that players do not conflict.
            adapter.Do();
            Assert.AreEqual(1, playerList.GetChildren().Count, "PlayerDataGridAdapter does not add a player properly.");

            List<ModelComponent> list = playerList.GetChildren();
            PlayerComponent player = (PlayerComponent)list[0];
            
            Assert.AreEqual(adapter.Player_Name, player.Name, "PlayerDataGridAdapter does not add a player properly - improper name.");
            Assert.AreEqual(adapter.RaceMember, player.Race, "PlayerDataGridAdapter does not add a player properly - improper race.");
            Assert.AreEqual(adapter.GoldMember, player.Gold, "PlayerDataGridAdapter does not add a player properly - improper gold.");
            Assert.AreEqual(adapter.WoodMember, player.Wood, "PlayerDataGridAdapter does not add a player properly - improper wood.");
            Assert.AreEqual(adapter.MetalMember, player.Metal, "PlayerDataGridAdapter does not add a player properly - improper metal.");
        }

        [Test]
        public void TestDoAddMemberAndThenRemovePlayerWithValidInput()
        {
            adapter.RemovedMember = true;
            adapter.Do();
            Assert.AreEqual(0, playerList.GetChildren().Count, "PlayerDataGridAdapter adds a player when it has been marked as both added and removed.");
        }

        [Test]
        public void TestDoRemovePlayerWithValidInput()
        {
            PlayerComponent playerToRemove = buildValidPlayer();
            playerList.AddChild(playerToRemove);
            adapter = new PlayerDataGridAdapter(playerToRemove, playerList);
            adapter.RemovedMember = true;

            adapter.Do();
            Assert.AreEqual(0, playerList.GetChildren().Count, "PlayerDataGridAdapter does not remove properly.");

        }

        [Test]
        public void TestDoUpdatePlayerWithValidInput()
        {
            PlayerComponent playerToUpdate = buildValidPlayer();
            playerList.AddChild(playerToUpdate);
            adapter = new PlayerDataGridAdapter(playerToUpdate, playerList);
            adapter.Player_Name = adapter.Player_Name + "updateAppended";
            adapter.RaceMember = "Human"; // buildValidPlayer initializes as Zombie.
            adapter.GoldMember += 10;
            adapter.MetalMember += 10;
            adapter.WoodMember += 10;

            adapter.Do();
            Assert.AreEqual(adapter.Player_Name, playerToUpdate.Name, "PlayerDataGridAdapter does not update name properly.");
            Assert.AreEqual(adapter.RaceMember, playerToUpdate.Race, "PlayerDataGridAdapter does not update race properly.");
            Assert.AreEqual(adapter.GoldMember, playerToUpdate.Gold, "PlayerDataGridAdapter does not update gold properly.");
            Assert.AreEqual(adapter.WoodMember, playerToUpdate.Wood, "PlayerDataGridAdapter does not update wood properly.");
            Assert.AreEqual(adapter.MetalMember, playerToUpdate.Metal, "PlayerDataGridAdapter does not update metal properly.");
        }

        [Test]
        [ExpectedException("System.NotImplementedException")]
        public void TestUndo()
        {
            adapter.Undo();
        }
        

    }
}
