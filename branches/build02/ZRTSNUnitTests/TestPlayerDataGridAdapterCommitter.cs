using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSMapEditor;
using NUnit.Framework;

namespace ZRTSNUnitTests
{
    [TestFixture]
    class TestPlayerDataGridAdapterCommitter
    {
        private PlayerList playerList;
        private PlayerDataGridAdapterCommitter committer;

        private PlayerDataGridAdapter generateValidAdapter()
        {
            PlayerDataGridAdapter adapter = new PlayerDataGridAdapter(new PlayerComponent(), playerList);
            adapter.Player_Name = "Player 1";
            adapter.RaceMember = "Human";
            adapter.GoldMember = 10;
            adapter.MetalMember = 20;
            adapter.WoodMember = 0;
            adapter.RemovedMember = false;
            adapter.AddedMember = true;
            return adapter;
        }
        [SetUp]
        public void Init()
        {
            playerList = new PlayerList();

            PlayerDataGridAdapter adapter = generateValidAdapter();

            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            adapters.Add(adapter);

            committer = new PlayerDataGridAdapterCommitter(adapters, playerList);
        }

        [Test]
        public void TestCanDoWithSimpleValidAdapters()
        {
            Assert.AreEqual(true, committer.CanBeDone(), "Committer does not realize when the actions can be done.");
        }

        [Test]
        public void TestCanDoWithTwoValidAdapters()
        {
            PlayerDataGridAdapter adapter1 = generateValidAdapter();
            PlayerDataGridAdapter adapter2 = generateValidAdapter();
            
            // Names currently conflict - resolve.
            adapter2.Player_Name = "Player 2";

            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            adapters.Add(adapter1);
            adapters.Add(adapter2);

            committer = new PlayerDataGridAdapterCommitter(adapters, playerList);
            Assert.AreEqual(true, committer.CanBeDone(), "Committer does not realize when the actions can be done.");
        }

        [Test]
        public void TestNameConflict()
        {
            PlayerDataGridAdapter adapter1 = generateValidAdapter();
            PlayerDataGridAdapter adapter2 = generateValidAdapter();

            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            adapters.Add(adapter1);
            adapters.Add(adapter2);

            committer = new PlayerDataGridAdapterCommitter(adapters, playerList);
            Assert.AreEqual(false, committer.CanBeDone(), "Committer does not realize when the names conflict.");
        }

        [Test]
        public void TestNameConflictWhereOneIsRemoved()
        {
            PlayerDataGridAdapter adapter1 = generateValidAdapter();
            PlayerDataGridAdapter adapter2 = generateValidAdapter();
            adapter2.RemovedMember = true;

            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            adapters.Add(adapter1);
            adapters.Add(adapter2);

            committer = new PlayerDataGridAdapterCommitter(adapters, playerList);
            Assert.AreEqual(true, committer.CanBeDone(), "Committer does not realize when the names conflict, but one is removed, that it is okay to Do().");
        }

        [Test]
        public void TestCanBeDoneWithAnInvalidAdapter()
        {
            PlayerDataGridAdapter adapter = generateValidAdapter();
            
            // Invalidate the adapter. 
            adapter.Player_Name = "";
            
            List<PlayerDataGridAdapter> adapters = new List<PlayerDataGridAdapter>();
            adapters.Add(adapter);
            committer = new PlayerDataGridAdapterCommitter(adapters, playerList);

            Assert.AreEqual(false, committer.CanBeDone(), "Committer does not determine that when an adapter cannot be done, the entire committ cannot be done.");

        }

        [Test]
        public void TestDo()
        {
            committer.Do();
            Assert.AreEqual(1, playerList.GetChildren().Count, "Committer did not call Do on all adapters.");
        }

        [Test]
        [ExpectedException("System.NotImplementedException")]
        public void TestUndo()
        {
            committer.Undo();
        }
    }
}
