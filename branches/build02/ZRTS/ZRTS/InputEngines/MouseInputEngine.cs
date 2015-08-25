using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTS.XnaCompositeView;
using Microsoft.Xna.Framework.Input;

namespace ZRTS.InputEngines
{
    /// <summary>
    /// This class, constructed with a given UI Frame, will raise onClick events.
    /// </summary>
    public class MouseInputEngine : GameComponent
    {
        private XnaUIFrame frame;
        private XnaUIComponent leftMouseDownTarget = null;
        private XnaUIComponent rightMouseDownTarget = null;
        private XnaUIComponent mouseHoverTarget = null;

        public MouseInputEngine(Game game, XnaUIFrame frame)
            : base(game)
        {
            this.frame = frame;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            XnaUIComponent currentUIOn = GetTarget(mousePoint);

            // Fire all events for mouse down, mouse up, and clicks
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (leftMouseDownTarget == null)
                {
                    leftMouseDownTarget = GetTarget(mousePoint);
                    XnaMouseEventArgs e = new XnaMouseEventArgs();
                    e.Target = leftMouseDownTarget;
                    e.ClickLocation = mousePoint;
                    e.Handled = false;
                    e.Bubbled = false;
                    e.ButtonPressed = MouseButton.Left;
                    e.SingleTarget = true;
					e.time = System.Environment.TickCount;
                    frame.MouseDown(e);
                }
            }
            else
            {
                if (leftMouseDownTarget != null)
                {
                    XnaUIComponent leftMouseUpTarget = currentUIOn;
                    XnaUIComponent commonAncestor = getCommonAncestor(leftMouseDownTarget, leftMouseUpTarget);
                    if (commonAncestor != null)
                    {
                        XnaMouseEventArgs mouseUpEventArgs = new XnaMouseEventArgs();
                        mouseUpEventArgs.Target = commonAncestor;
                        mouseUpEventArgs.ClickLocation = mousePoint;
                        mouseUpEventArgs.Handled = false;
                        mouseUpEventArgs.Bubbled = false;
                        mouseUpEventArgs.ButtonPressed = MouseButton.Left;
                        mouseUpEventArgs.SingleTarget = (leftMouseDownTarget == leftMouseUpTarget);
						mouseUpEventArgs.time = System.Environment.TickCount;
                        frame.MouseUp(mouseUpEventArgs);

                        XnaMouseEventArgs e = new XnaMouseEventArgs();
                        e.Target = commonAncestor;
                        e.ClickLocation = mousePoint;
                        e.Handled = false;
                        e.Bubbled = false;
                        e.ButtonPressed = MouseButton.Left;
                        e.SingleTarget = (leftMouseDownTarget == leftMouseUpTarget);
						e.time = System.Environment.TickCount;
                        frame.Click(e);
                    }
                    // Reset the state.
                    leftMouseDownTarget = null;
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (rightMouseDownTarget == null)
                {
                    rightMouseDownTarget = currentUIOn;
                    XnaMouseEventArgs e = new XnaMouseEventArgs();
                    e.Target = rightMouseDownTarget;
                    e.ClickLocation = mousePoint;
                    e.Handled = false;
                    e.Bubbled = false;
                    e.ButtonPressed = MouseButton.Right;
                    e.SingleTarget = true;
					e.time = System.Environment.TickCount;
                    frame.MouseDown(e);
                }
            }
            else
            {
                if (rightMouseDownTarget != null)
                {
                    XnaUIComponent rightMouseUpTarget = GetTarget(mousePoint);
                    XnaUIComponent commonAncestor = getCommonAncestor(rightMouseDownTarget, rightMouseUpTarget);

                    if (commonAncestor != null)
                    {
                        XnaMouseEventArgs mouseUpEventArgs = new XnaMouseEventArgs();
                        mouseUpEventArgs.Target = commonAncestor;
                        mouseUpEventArgs.ClickLocation = mousePoint;
                        mouseUpEventArgs.Handled = false;
                        mouseUpEventArgs.Bubbled = false;
                        mouseUpEventArgs.ButtonPressed = MouseButton.Left;
                        mouseUpEventArgs.SingleTarget = (rightMouseDownTarget == rightMouseUpTarget);
						mouseUpEventArgs.time = System.Environment.TickCount;
                        frame.MouseUp(mouseUpEventArgs);

                        XnaMouseEventArgs e = new XnaMouseEventArgs();
                        e.Target = commonAncestor;
                        e.ClickLocation = mousePoint;
                        e.Handled = false;
                        e.Bubbled = false;
                        e.ButtonPressed = MouseButton.Right;
                        e.SingleTarget = (rightMouseDownTarget == rightMouseUpTarget);
						e.time = System.Environment.TickCount;
                        frame.Click(e);
                    }
                    // Reset the state.
                    rightMouseDownTarget = null;
                }
            }

            // Handle mouse enter and exit events.
            if (mouseHoverTarget != currentUIOn)
            {
                // Base case : First time entering.
                if (mouseHoverTarget == null)
                {
                    XnaUIComponent current = currentUIOn;
                    Stack<XnaUIComponent> stack = new Stack<XnaUIComponent>();
                    while (current != null)
                    {
                        stack.Push(current);
                        current = current.Parent;
                    }
                    while (stack.Count > 0)
                    {
                        current = stack.Pop();
                        current.MouseEnter();
                    }
                }
                else
                {
                    // Mouse has moved
                    XnaUIComponent current = mouseHoverTarget;
                    XnaUIComponent ancestor = getCommonAncestor(currentUIOn, mouseHoverTarget);
                    while (current != ancestor)
                    {
                        current.MouseLeave();
                        current = current.Parent;
                    }
                    current = currentUIOn;
                    Stack<XnaUIComponent> stack = new Stack<XnaUIComponent>();
                    while (current != ancestor)
                    {
                        stack.Push(current);
                        current = current.Parent;
                    }
                    while (stack.Count > 0)
                    {
                        current = stack.Pop();
                        current.MouseEnter();
                    }
                }
                mouseHoverTarget = currentUIOn;
            }
            base.Update(gameTime);
        }

        private XnaUIComponent GetHoverTarget(Point p)
        {
            XnaUIComponent current = frame;
            bool searching = true;

            while (searching)
            {
                p.X += current.ScrollX;
                p.Y += current.ScrollY;

                searching = false;

                for (int i = current.GetChildren().Count - 1; i >= 0; i--)
                {
                    XnaUIComponent component = current.GetChildren()[i];
                    Rectangle box = new Rectangle(component.DrawBox.X, component.DrawBox.Y, component.DrawBox.Width, component.DrawBox.Height);

                    if (contains(p, box))
                    {
                        current = component;
                        p.X -= component.DrawBox.X;
                        p.Y -= component.DrawBox.Y;
                        searching = true;
                        break;
                    }
                }
            }
            return current;
        }

        private XnaUIComponent getCommonAncestor(XnaUIComponent c1, XnaUIComponent c2)
        {
            List<XnaUIComponent> list1 = new List<XnaUIComponent>();
            XnaUIComponent current = c1;
            while (current != null)
            {
                list1.Add(current);
                current = current.Parent;
            }
            List<XnaUIComponent> list2 = new List<XnaUIComponent>();
            current = c2;
            while (current != null)
            {
                list2.Add(current);
                current = current.Parent;
            }
            int differenceInSize = Math.Abs(list1.Count - list2.Count);
            List<XnaUIComponent> longerList = list2.Count > list1.Count ? list2 : list1;
            while (differenceInSize > 0)
            {
                longerList.RemoveAt(0);
                differenceInSize--;
            }

            XnaUIComponent commonAncestor = null;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == list2[i])
                {
                    commonAncestor = list1[i];
                    break;
                }
            }
            return commonAncestor;
        }

        public XnaUIComponent GetTarget(Point p)
        {
            XnaUIComponent current = frame;
            bool searching = true;

            while (searching)
            {
                p.X += current.ScrollX;
                p.Y += current.ScrollY;

                searching = false;

                for (int i = current.GetChildren().Count - 1; i >= 0; i--)
                {
                    XnaUIComponent component = current.GetChildren()[i];
                    if (component.Visible)
                    {
                        Rectangle box = new Rectangle(component.DrawBox.X, component.DrawBox.Y, component.DrawBox.Width, component.DrawBox.Height);

                        if (contains(p, box))
                        {
                            current = component;
                            p.X -= component.DrawBox.X;
                            p.Y -= component.DrawBox.Y;
                            searching = true;
                            break;
                        }
                    }
                }
            }
            return current;
        }

        private bool contains(Point p, Rectangle box)
        {
            return ((p.X >= box.X) && (p.X < (box.X + box.Width)) && (p.Y >= box.Y) && (p.Y < (box.Y + box.Height)));
        }
    }
}
