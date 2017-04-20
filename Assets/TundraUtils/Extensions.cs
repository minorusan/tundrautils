//
//  Extensions.cs
//  TundraUtils
//
//  Created on 06/20/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using UnityEngine;

using TundraUtils.Types;


namespace TundraUtils
{
    public static class CameraExtension
    {
        public static Vector3 TouchPosition(this Camera camera)
        {
            Vector3 result = Vector3.zero;
#if UNITY_EDITOR || UNITY_STANDALONE
            result = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
#else
                if (!(Input.touchCount > 0)) return Vector3.zero;
                result = Input.GetTouch(0).position;
#endif
            result = camera.ScreenToWorldPoint(result);

            return result;
        }
    }

    public static class BoundsExtension
    {
        public static Vector3 TopLeft(this Bounds self)
        {
            return new Vector3(self.min.x, self.max.y);
        }

        public static Vector3 BotRight(this Bounds self)
        {
            return new Vector3(self.max.x, self.min.y);
        }
    }

    public static class DirectionExtension
    {
        public static Vector2 ToVector2(this EDirection Value)
        {
            if (EDirection.Left == Value)
            {
                return new Vector2(-1, 0);
            }
            if (EDirection.Right == Value)
            {
                return new Vector2(1, 0);
            }
            if (EDirection.Down == Value)
            {
                return new Vector2(0, -1);
            }
            if (EDirection.Up == Value)
            {
                return new Vector2(0, 1);
            }
            return new Vector2(0, 0);
        }

        public static EDirection Opposite(this EDirection Value)
        {
            if (EDirection.Down == Value)
            {
                return EDirection.Up;
            }
            if (EDirection.Right == Value)
            {
                return EDirection.Left;
            }
            if (EDirection.Up == Value)
            {
                return EDirection.Down;
            }
            if (EDirection.Left == Value)
            {
                return EDirection.Right;
            }

            return EDirection.None;
        }

        public static Point ToPoint(this EDirection Value)
        {
            if (EDirection.Left == Value)
            {
                return new Point(-1, 0);
            }
            if (EDirection.Right == Value)
            {
                return new Point(1, 0);
            }
            if (EDirection.Down == Value)
            {
                return new Point(0, -1);
            }
            if (EDirection.Up == Value)
            {
                return new Point(0, 1);
            }
            return new Point(0, 0);
        }

        public static Vector3 ToRotation(this EDirection Value)
        {
            if (EDirection.Left == Value)
            {
                return new Vector3(0, 0);
            }
            if (EDirection.Right == Value)
            {
                return new Vector3(0, 0, 180);
            }
            if (EDirection.Down == Value)
            {
                return new Vector3(0, 0, 90);
            }
            if (EDirection.Up == Value)
            {
                return new Vector3(0, 0, -90);
            }
            return new Vector3(0, 0);
        }

        public static EDirection Rotate(this EDirection Value, int times = 1)
        {
            EDirection r = Value;
            while (times-- > 0)
            {
                if (EDirection.Down == r)
                {
                    r = EDirection.Left;
                }
                else if (EDirection.None != r)
                {
                    r = (EDirection)(1 + (int)r);
                }
            }

            return r;
        }
    }

    public static class VectorExtension
    {
        public static Vector3 RotateBy(this Vector3 self, float rotation)
        {
            float radians = rotation * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);

            float tx = self.x;
            float ty = self.y;

            var vec = new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);

            return vec;
        }

        public static EDirection ToDirection(this Vector2 self)
        {
            float tolerancy = 45.0f;
            if (Vector2.Angle(Vector2.up, self) <= tolerancy)
            {
                return EDirection.Up;
            }
            else if (Vector2.Angle(Vector2.left, self) <= tolerancy)
            {
                return EDirection.Left;
            }
            else if (Vector2.Angle(Vector2.right, self) <= tolerancy)
            {
                return EDirection.Right;
            }
            else if (Vector2.Angle(Vector2.down, self) <= tolerancy)
            {
                return EDirection.Down;
            }

            return EDirection.None;
        }

        public static float GetAngle(this Vector2 self, Vector2 v2)
        {
            var v1 = self;

            var sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
            return Vector2.Angle(v1, v2) * sign;
        }

        public static Point ToPoint(this Vector3 self)
        {
            int x = Mathf.RoundToInt(self.x);
            int y = Mathf.RoundToInt(self.y);

            return new Point(x, y);
        }
    }

    public static class UniversalBoundsExtractor
    {
        /// <summary>
        /// Gets bounds for Circle and Rectangular Collider2D
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Bounds GetBounds(this Collider2D col)
        {
            Bounds aabb = col.bounds;

            CircleCollider2D circle = col as CircleCollider2D;
            if (circle != null)
            {
                Bounds rad = new Bounds(aabb.center, new Vector3(circle.radius * 2, circle.radius * 2, 0));
                return rad;
            }

            return aabb;
        }
    }
}