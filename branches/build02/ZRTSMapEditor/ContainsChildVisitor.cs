using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel;
using ZRTSMapEditor.MapEditorModel;

namespace ZRTSMapEditor
{
    /// <summary>
    /// A visitor that visits the a type given at construction that searches the children of the accepting component
    /// for a given child.  It is only meant to evaluate a single accepting class.
    /// </summary>
    public class ContainsChildVisitor : NoOpMapEditorModelVisitor
    {
        private ModelComponent child = null;
        private Type modelType = null;
        private bool evaluated = false;
        private bool containsChild = false;

        private ContainsChildVisitor()
        { }

        public ContainsChildVisitor(ModelComponent targetChild, Type type)
        {
            child = targetChild;
            modelType = type;
        }

        /// <summary>
        /// Checks the accepting component to see if it is the type we are looking for, and if so, checks for the child given at construction.
        /// Updates the state of the visitor and ensures that it will not be evaluated again.
        /// </summary>
        /// <param name="component"></param>
        public override void Visit(ModelComponent component)
        {
            if (modelType.GetType().Equals(component.GetType()) && !evaluated)
            {
                foreach (ModelComponent mc in component.GetChildren())
                {
                    if (mc == child)
                    {
                        containsChild = true;
                        break;
                    }
                }
                evaluated = true;
            }
        }

        public bool FoundChild()
        {
            return containsChild;
        }
        public bool WasEvaled()
        {
            return evaluated;
        }
    }
}
