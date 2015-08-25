using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;
namespace ZRTSModel
{
    /// <summary>
    /// An abstact representation of a piece of model, following the composite pattern.  Exposes a common interface for each piece of model.
    /// Each piece of model is observable, and can be visited.  When children are added, the component notifies its observers.
    /// 
    /// Whenever a component notifies its observers, it tells its parent to notify its observers as well.
    /// </summary>
    [Serializable()]
    public abstract class ModelComponent
    {
        // Composite Pattern members
        private ModelComponent container = null;
		public ModelComponentSelectedHandler SelectHandler;

        public ModelComponent Parent
        {
            get { return container; }
        }
		private bool selected;

		public bool Selected
		{
			get { return selected; }
			set { 
				selected = value;
				if (selected)
				{
					this.OnSelect();
				}
				else
				{
					this.OnDeselect();
				}
			}
		}
        
        private List<ModelComponent> children = new List<ModelComponent>();
        
        // Composite Pattern Interface
        public ModelComponent GetContainer()
        {
            return container;
        }

        public void SetContainer(ModelComponent composite)
        {
            if (container != null)
            {
                container.GetChildren().Remove(this);
            }
            container = composite;
            
        }

        public virtual List<ModelComponent> GetChildren()
        {
            return children;
        }

        public virtual void AddChild(ModelComponent child)
        {
            if (child != null)
            {
                child.SetContainer(this);
                children.Add(child);
            }
        }

        public virtual void RemoveChild(ModelComponent child)
        {
            child.SetContainer(null);
        }

        // Visitor Pattern Interfaces
        public abstract void Accept(ModelComponentVisitor visitor);



        public void AddChildAt(ModelComponent child, int p)
        {
            if (child != null)
            {
                AddChild(child);
                // Some classes may override add child to only accept certain children.  Ensure that it got added before placing it in the correct location.
                if (GetChildren().Contains(child))
                {
                    GetChildren().Remove(child);
                    GetChildren().Insert(0, child);
                }
            }
        }

		public void OnSelect()
		{
			if (SelectHandler != null)
			{
				SelectHandler(this, true);
			}
		}

		public void OnDeselect()
		{

			if (SelectHandler != null)
			{
				SelectHandler(this, false);
			}
		}
    }
}
