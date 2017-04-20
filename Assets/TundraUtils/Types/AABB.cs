//
//  AABB.cs
//  TundraUtils
//
//  Created on 07/11/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using System;

using UnityEngine;


namespace TundraUtils.Types
{
    [Serializable]
    public class AABB
    {
        private Point _botLeft;
        private Point _topRight;

        public Point TopRight
        {
            get
            {
                return _topRight.Clone();
            }
        }

        public Point TopLeft
        {
            get
            {
                return new Point(_botLeft.X, _topRight.Y);
            }
        }

        public Point BotLeft
        {
            get
            {
                return _botLeft.Clone();
            }
        }

        internal bool Contains(Vector3 pos)
        {
            return Contains(pos.ToPoint());
        }

        public Point BotRight
        {
            get
            {
                return new Point(_topRight.X, _botLeft.Y);
            }
        }

        public bool Empty
        {
            get
            {
                return _empty;
            }
        }

        private bool _empty = false;

        public AABB() : this(new Point(0, 0), new Point(0, 0))
        {
            _empty = true;
        }

        public AABB(int x0, int y0, int x1, int y1) : this(new Point(x0, y0), new Point(x1, y1))
        {

        }

        public AABB(Point center) : this(center, center)
        {
            _empty = false;
        }

        public AABB(Point a, Point b)
        {
            _topRight = a;
            _botLeft = b;
            _empty = false;

            Normalize();
        }

        private void Normalize()
        {
            int minX = Mathf.Min(_topRight.X, _botLeft.X);
            int minY = Mathf.Min(_topRight.Y, _botLeft.Y);

            int maxX = Mathf.Max(_topRight.X, _botLeft.X);
            int maxY = Mathf.Max(_topRight.Y, _botLeft.Y);

            _topRight = new Point(maxX, maxY);
            _botLeft = new Point(minX, minY);
        }

        public void DebugDraw()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawLine(BotLeft.Vector3, TopLeft.Vector3);
            Gizmos.DrawLine(TopLeft.Vector3, TopRight.Vector3);
            Gizmos.DrawLine(TopRight.Vector3, BotRight.Vector3);
            Gizmos.DrawLine(BotRight.Vector3, BotLeft.Vector3);
        }

        public int Left
        {
            get { return _botLeft.X; }
        }

        public int Right
        {
            get { return _topRight.X; }
        }

        public int Bottom
        {
            get { return _botLeft.Y; }
        }

        public int Top
        {
            get { return _topRight.Y; }
        }

        public static void RunTests()
        {
            AABB a00b33 = new AABB(0, 0, 3, 3);
            AABB a11b22 = new AABB(1, 1, 2, 2);

            AABB a1_1b14 = new AABB(1, -1, 1, 4);

            Point inside = new Point(1, 1);
            Point outside = new Point(5, 5);

            Debug.Assert(a00b33.Intersects(a11b22),
                "First intersection test failed");

            Debug.Assert(a00b33.Intersects(a1_1b14),
                "Second intersection test failed");

            Debug.Assert(a00b33.Contains(inside),
                "Contains check failed for correct point");

            Debug.Assert(!a00b33.Contains(outside),
                "Contains check failed for wrong point (AABB thinks that it is inside)");
        }

        private bool ContainsBorderExcluded(Point p)
        {
            if (p.X > this.Right)
            {
                return false;
            }
            if (p.Y > this.Top)
            {
                return false;
            }
            if (p.X < this.Left)
            {
                return false;
            }
            if (p.Y < this.Bottom)
            {
                return false;
            }

            return true;
        }

        public bool Contains(Point p, bool includeBorder = true)
        {
            if (!includeBorder)
                return ContainsBorderExcluded(p);

            if (p.X >= this.Right)
            {
                return false;
            }
            if (p.Y >= this.Top)
            {
                return false;
            }
            if (p.X <= this.Left)
            {
                return false;
            }
            if (p.Y <= this.Bottom)
            {
                return false;
            }

            return true;
        }

        public void Encapsulate(Point it)
        {
            if (Empty)
            {
                _empty = false;
                return;
            }

            if (Contains(it))
            {
                return;
            }

            int minX = Mathf.Min(_topRight.X, _botLeft.X, it.X);
            int minY = Mathf.Min(_topRight.Y, _botLeft.Y, it.Y);

            int maxX = Mathf.Max(_topRight.X, _botLeft.X, it.X);
            int maxY = Mathf.Max(_topRight.Y, _botLeft.Y, it.Y);

            _topRight = new Point(maxX, maxY);
            _botLeft  = new Point(minX, minY);
        }

        public bool Intersects(AABB other)
        {
            bool x_this = other.Left >= this.Left && other.Right < this.Right
                && other.Top > this.Bottom && other.Bottom < this.Top;

            bool x_other = this.Left > other.Left && this.Right < other.Right
                && this.Top > other.Bottom && this.Bottom < other.Top;

            return x_this || x_other;
        }

        public Point Size()
        {
            return _topRight - _botLeft;
        }
    }
}