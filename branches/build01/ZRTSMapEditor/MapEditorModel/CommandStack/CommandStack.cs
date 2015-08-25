using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;

namespace ZRTSMapEditor.MapEditorModel
{
    /// <summary>
    /// A model component responsible for executing MapEditorCommands, undoing them, and redoing them.
    /// </summary>
    public class CommandStack : MapEditorModelComponent
    {
        private Stack<MapEditorCommand> done = new Stack<MapEditorCommand>();
        private Stack<MapEditorCommand> undone = new Stack<MapEditorCommand>();

        /// <summary>
        /// Executes a command, placing it on the stack.  This empties the Redo stack, losing those commands permanently.  This function does not check if a command
        /// can be done, so all code using the CommandStack should call CanBeDone() on the command before passing it to the CommandStack.
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(MapEditorCommand command)
        {
            command.Do();
            done.Push(command);
            undone.Clear();
        }

        /// <summary>
        /// Undoes the last command, popping it from the done stack and pushing it onto the undone stack.
        /// </summary>
        public void UndoLastCommand()
        {
            try
            {
                MapEditorCommand command = done.Pop();
                command.Undo();
                undone.Push(command);
            }
            catch (InvalidOperationException)
            {
                // The stack was empty, do nothing.
            }
        }

        /// <summary>
        /// Redoes the last undone command, popping it from the undone stack and pushing it onto the done stack.
        /// </summary>
        public void RedoLastUndoneCommand()
        {
            try
            {
                MapEditorCommand command = undone.Pop();
                command.Do();
                done.Push(command);
            }
            catch (InvalidOperationException)
            {
                // The stack was empty, do nothing.
            }
        }

        public bool CanUndo()
        {
            return (done.Count == 0);
        }

        public bool CanRedo()
        {
            return (undone.Count == 0);
        }

        public override void Accept(MapEditorModelVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal void EmptyStacks()
        {
            done.Clear();
            undone.Clear();
        }
    }
}
