using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZRTS.XnaCompositeView.UIEventHandlers;
using ZRTS.InputEngines;

namespace ZRTS.XnaCompositeView
{
    public abstract class XnaUIComponent : DrawableGameComponent
    {
        public event DrawBoxChanged SizeChanged;
        public event ClickEventHandler OnClick;
        public event ClickEventHandler OnMouseDown;
        public event ClickEventHandler OnMouseUp;
        public event EventHandler OnMouseEnter;
        public event EventHandler OnMouseLeave;

        //public event MouseOverEventHandler OnOver;

        // UI members and fields
        private Rectangle drawBox = new Rectangle(0, 0, 0, 0);
        private int scrollX;
        private int scrollY;

        public Rectangle DrawBox
        {
            get { return drawBox; }
            set 
            { 
                drawBox = value;
                if (SizeChanged != null)
                {
                    UISizeChangedEventArgs e = new UISizeChangedEventArgs();
                    e.DrawBox = drawBox;
                    SizeChanged(this, e);
                }
            }
        }
        public int ScrollX
        {
            get { return scrollX; }
            set { scrollX = value; }
        }
        public int ScrollY
        {
            get { return scrollY; }
            set { scrollY = value; }
        }

        // Composite pattern members
        private XnaUIComponent container = null;

        public XnaUIComponent Parent
        {
            get { return container; }
        }
        private List<XnaUIComponent> components = new List<XnaUIComponent>();


        public XnaUIComponent(Game game)
            : base(game)
        {
        }

        // UI Methods
        public override void Draw(GameTime gameTime)
        {
            if (Visible && drawBox.Width > 0 && drawBox.Height > 0)
            {
                Viewport currentViewport = Game.GraphicsDevice.Viewport;
                int viewportMaxX = currentViewport.X + currentViewport.Width;
                int viewportMaxY = currentViewport.Y + currentViewport.Height;

                

                int offsetX = drawBox.X;
                int offsetY = drawBox.Y;
                if (container != null)
                {
                    offsetX -= container.ScrollX;
                    offsetY -= container.ScrollY;
                }

                // Ensure that it is inbounds.
                if (((offsetX + drawBox.Width) > 0) && (offsetX < currentViewport.Width) && ((offsetY + drawBox.Height) > 0) && (offsetY < currentViewport.Height))
                {
                    XnaDrawArgs e = new XnaDrawArgs();
                    e.gameTime = gameTime;
                    e.SpriteBatch = GetSpriteBatch(this);
                    e.Location = new Rectangle(0, 0, drawBox.Width, drawBox.Height);

                    Viewport componentViewport = currentViewport;
                    componentViewport.X = Math.Max(offsetX + currentViewport.X, currentViewport.X);
                    componentViewport.Y = Math.Max(offsetY + currentViewport.Y, currentViewport.Y);

                    // Fix the argument location if it is offset in the negative direction (cropped due to scrolling).
                    if (offsetX < 0)
                    {
                        e.Location.X = offsetX;
                        componentViewport.Width = Math.Max(currentViewport.Width, (offsetX + drawBox.Width));
                    }
                    else
                    {
                        componentViewport.Width = Math.Min(drawBox.Width, (currentViewport.Width - offsetX));
                    }
                    if (offsetY < 0)
                    {
                        e.Location.Y = offsetY;
                        componentViewport.Height = Math.Max(currentViewport.Height, (offsetY + drawBox.Height));
                    }
                    else
                    {
                        componentViewport.Height = Math.Min(drawBox.Height, (currentViewport.Height - offsetY));
                    }
                    // Likewise fix the height

                    Game.GraphicsDevice.Viewport = componentViewport;
                    // Do Draw work.
                    e.SpriteBatch.Begin();
                    onDraw(e);
                    e.SpriteBatch.End();

                    // Call draw on all children
                    foreach (XnaUIComponent child in components)
                    {
                        child.Draw(gameTime);
                    }

                    // Repair the viewport
                    Game.GraphicsDevice.Viewport = currentViewport;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (XnaUIComponent component in components)
            {
                component.Update(gameTime);
            }
        }



        protected abstract void onDraw(XnaDrawArgs e);

        /// <summary>
        /// Returns the sprite batch from the XnaUIFrame that this XnaUIComponent is in.  The frame may
        /// hold on to several sprite batches, and depend on the requester to determine which one to return back
        /// for drawing.
        /// </summary>
        /// <param name="requester"></param>
        /// <returns></returns>
        public virtual SpriteBatch GetSpriteBatch(XnaUIComponent requester)
        {
            SpriteBatch sb = null;
            if (container != null)
            {
                sb = container.GetSpriteBatch(requester);
            }
            return sb;
        }

        /// <summary>
        /// Lays out the view
        /// </summary>
        public virtual void DoLayout()
        {
            // Do nothing.
        }

        // Composite pattern methods

        /// <summary>
        /// Called from AddChild and RemoveChild only.  Updates the child so that it references the parent as its container.
        /// If AddChild is called from a different parent, it should remove its reference to its current container.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(XnaUIComponent parent)
        {
            if (container != null)
            {
                // Remove it from the parent.
                container.GetChildren().Remove(this);
            }
            container = parent;
        }

        /// <summary>
        /// Returns the child list of the component
        /// </summary>
        /// <returns>The child list of the component</returns>
        public List<XnaUIComponent> GetChildren()
        {
            return components;
        }

        /// <summary>
        /// Adds child to the component list, and updates the child's parent.
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(XnaUIComponent child)
        {
            if (child != null)
            {
                child.SetParent(this);
                components.Add(child);
                DoLayout();
            }
        }

        /// <summary>
        /// Removes child from the component list, and updates the child's parent.
        /// </summary>
        /// <param name="child"></param>
        public virtual void RemoveChild(XnaUIComponent child)
        {
            if (child != null)
            {
                // This will remove the child from the component list.
                child.SetParent(null);
                DoLayout();
            }
        }

        /// <summary>
        /// Clears the view, disposing each child component.
        /// </summary>
        public void Clear()
        {
            for (; GetChildren().Count > 0; )
            {
                GetChildren()[0].Dispose();
                GetChildren()[0].Clear();
                RemoveChild(GetChildren()[0]);
            }

        }

        internal void MouseDown(XnaMouseEventArgs e)
        {
            if (!e.Handled)
            {
                if (OnMouseDown != null)
                {
                    OnMouseDown(this, e);
                }
                if (e.Target != this)
                {
                    XnaUIComponent nextTarget = e.Target;
                    while (nextTarget.Parent != this)
                    {
                        nextTarget = nextTarget.Parent;
                    }
                    Point currentPoint = e.ClickLocation;
                    Point offsetPoint = new Point(currentPoint.X, currentPoint.Y);
                    offsetPoint.X -= ScrollX;
                    offsetPoint.X -= nextTarget.drawBox.X;
                    offsetPoint.Y -= ScrollY;
                    offsetPoint.Y -= nextTarget.drawBox.Y;
                    e.ClickLocation = offsetPoint;
                    nextTarget.MouseDown(e);

                    // Repair event
                    e.ClickLocation = currentPoint;
                }
                e.Bubbled = true;
                if (!e.Handled && (OnMouseDown != null))
                {
                    OnMouseDown(this, e);
                }
            }
        }

        internal void MouseUp(XnaMouseEventArgs e)
        {
            if (!e.Handled)
            {
                if (OnMouseUp != null)
                {
                    OnMouseUp(this, e);
                }
                if (e.Target != this)
                {
                    XnaUIComponent nextTarget = e.Target;
                    while (nextTarget.Parent != this)
                    {
                        nextTarget = nextTarget.Parent;
                    }
                    Point currentPoint = e.ClickLocation;
                    Point offsetPoint = new Point(currentPoint.X, currentPoint.Y);
                    offsetPoint.X -= ScrollX;
                    offsetPoint.X -= nextTarget.drawBox.X;
                    offsetPoint.Y -= ScrollY;
                    offsetPoint.Y -= nextTarget.drawBox.Y;
                    e.ClickLocation = offsetPoint;
                    nextTarget.MouseUp(e);

                    // Repair event
                    e.ClickLocation = currentPoint;
                }
                e.Bubbled = true;
                if (!e.Handled && (OnMouseUp != null))
                {
                    OnMouseUp(this, e);
                }
            }
        }

        internal void Click(InputEngines.XnaMouseEventArgs e)
        {
            if (!e.Handled)
            {
                if (OnClick != null)
                {
                    OnClick(this, e);
                }
                if (e.Target != this)
                {
                    XnaUIComponent nextTarget = e.Target;
                    while (nextTarget.Parent != this)
                    {
                        nextTarget = nextTarget.Parent;
                    }
                    Point currentPoint = e.ClickLocation;
                    Point offsetPoint = new Point(currentPoint.X, currentPoint.Y);
                    offsetPoint.X -= ScrollX;
                    offsetPoint.X -= nextTarget.drawBox.X;
                    offsetPoint.Y -= ScrollY;
                    offsetPoint.Y -= nextTarget.drawBox.Y;
                    e.ClickLocation = offsetPoint;
                    nextTarget.Click(e);

                    // Repair event
                    e.ClickLocation = currentPoint;
                }
                e.Bubbled = true;
                if (!e.Handled && (OnClick != null))
                {
                    OnClick(this, e);
                }
            }
        }

        internal void MouseEnter()
        {
            if (OnMouseEnter != null)
            {
                OnMouseEnter(null, null);
            }
        }

        internal void MouseLeave()
        {
            if (OnMouseLeave != null)
            {
                OnMouseLeave(null, null);
            }
        }
    }
}
