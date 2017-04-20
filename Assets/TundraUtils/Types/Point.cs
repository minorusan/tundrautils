//
//  Point.cs
//  TundraUtils
//
//  Created on 09/07/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using UnityEngine;


namespace TundraUtils.Types
{
    public struct Point
    {
        int _x, _y;

        #region Constructors

        public Point(int x = 0, int y = 0)
        {
            _x = x;
            _y = y;
        }

        public Point(float x, float y)
        {
            _x = (int)x;
            _y = (int)y;
        }

        public Point(Vector3 touchPosition)
        {
            _x = Mathf.RoundToInt(touchPosition.x);
            _y = Mathf.RoundToInt(touchPosition.y);
        }

        #endregion

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Vector3 with X and Y coords of current point
        /// </summary>
        public Vector3 Vector3
        {
            get
            {
                return new Vector3(_x, _y);
            }
        }

        /// <summary>
        /// Vector2 with X and Y coords of current point
        /// </summary>
        public Vector2 Vector2
        {
            get
            {
                return new Vector2(_x, _y);
            }
        }

        #region Object and Operators

        public override bool Equals(object obj)
        {
            bool isPoint = obj is Point;
            if (!isPoint)
                return false;

            var other = (Point)obj;
            return _x == other._x && _y == other._y;
        }

        public override int GetHashCode()
        {
            return _x + _y * 2000;
        }

        public Point Clone()
        {
            return new Point(_x, _y);
        }

        public static bool operator ==(Point left, Point right)
        {
            if (object.ReferenceEquals(left, right))
                return true;
            if (right == null || left == null)
                return false;

            return (left.X == right.X) && (left.Y == right.Y);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public static Point operator +(Point pt, Point pt2)
        {
            return new Point(pt._x + pt2._x, pt._y + pt2._y);
        }

        public static Point operator -(Point pt, Point pt2)
        {
            return new Point(pt._x - pt2._x, pt._y - pt2._y);
        }

        public static Point operator *(Point pt, int v)
        {
            return new Point(pt._x * v, pt._y * v);
        }

        #endregion
    }
}