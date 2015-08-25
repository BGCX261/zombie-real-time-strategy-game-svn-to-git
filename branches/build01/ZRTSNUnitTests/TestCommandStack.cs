using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZRTSMapEditor;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSNUnitTests
{
    [TestFixture]
    class TestCommandStack
    {
        private CommandStack commandStack;
        private Counter counter;

        [SetUp]
        public void Init()
        {
            commandStack = new CommandStack();
            counter = new Counter();
        }

        [Test]
        public void TestExecuteSimpleCommand()
        {
            TestIncrementCommand command = new TestIncrementCommand();
            command.Counter = counter;
            commandStack.ExecuteCommand(command);
            Assert.AreEqual(1, counter.CounterMember, "CommandStack does not execute commands.");
        }

        [Test]
        public void TestUndoSimpleCommand()
        {
            TestIncrementCommand command = new TestIncrementCommand();
            command.Counter = counter;

            commandStack.ExecuteCommand(command);
            commandStack.UndoLastCommand();
            Assert.AreEqual(0, counter.CounterMember, "CommandStack does not undo commands.");
        }

        [Test]
        public void TestExecuteCommandClearsRedoStack()
        {
            TestIncrementCommand command = new TestIncrementCommand();
            TestDecrementCommand command2 = new TestDecrementCommand();
            command.Counter = counter;
            command2.Counter = counter;

            commandStack.ExecuteCommand(command);   // counter = 1.
            commandStack.UndoLastCommand();         // counter = 0.
            commandStack.ExecuteCommand(command2);  // counter = -1.

            // Should be a no op, because we have executed since last undoing.
            commandStack.RedoLastUndoneCommand();   // counter = -1.

            Assert.AreEqual(-1, counter.CounterMember, "CommandStack does not clear undo stack after executing a command.");
        }

        [Test]
        public void TestSimpleRedo()
        {
            TestIncrementCommand command = new TestIncrementCommand();
            command.Counter = counter;

            commandStack.ExecuteCommand(command);   // counter = 1.
            commandStack.UndoLastCommand();         // counter = 0.
            commandStack.RedoLastUndoneCommand();   // counter = 1.

            Assert.AreEqual(1, counter.CounterMember, "CommandStack does not redo commands.");
        }

        [Test]
        public void TestInvalidUndosAndRedosDoNotThrowExceptions()
        {
            commandStack.UndoLastCommand();
            commandStack.RedoLastUndoneCommand();
        }

        private class Counter
        {
            private int counter = 0;

            public int CounterMember
            {
                get { return counter; }
                set { counter = value; }
            }
            
            public void increment()
            {
                counter++;
            }
            public void decrement()
            {
                counter--;
            }
        }

        private class TestIncrementCommand : MapEditorCommand
        {
            private Counter counter;

            public Counter Counter
            {
                get { return counter; }
                set { counter = value; }
            }

            public void Do()
            {
                counter.increment();
            }

            public void Undo()
            {
                counter.decrement();
            }

            public bool CanBeDone()
            {
                return true;
            }
        }

        private class TestDecrementCommand : MapEditorCommand
        {
            private Counter counter;

            public Counter Counter
            {
                get { return counter; }
                set { counter = value; }
            }

            public void Do()
            {
                counter.decrement();
            }

            public void Undo()
            {
                counter.increment();
            }

            public bool CanBeDone()
            {
                return true;
            }
        }
    }
}
