//
//  Line.cs
//  TundraUtils
//
//  Created on 09/05/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using System;

using UnityEngine;


namespace TundraUtils.LineStyles
{
    public struct LineWithNrm
    {
        public Line Edge;
        public Vector3 Normal;
    }

    public struct Line
    {
        public Vector3 A;
        public Vector3 B;

        public Vector3 Normal;

        public float Length
        {
            get { return (B - A).magnitude; }
        }

        public Vector3 Midpoint
        {
            get
            {
                return (A + B) / 2;
            }
        }

        public Vector3 AtoB
        {
            get { return A - B; }
        }

        public bool IsHorizontal
        {
            get
            {
                return Math.Abs(B.x - A.x) > Math.Abs(B.y - A.y);
            }
        }

        public Line(Vector3 a, Vector3 b) : this(a, b, Vector3.one)
        { 
        }

        internal void DrawDebug(bool normal = false)
        {
            Gizmos.DrawLine(A, B);
            if (normal) Gizmos.DrawLine(Midpoint, Midpoint + Normal.normalized);
        }

        public Line(Vector3 a, Vector3 b, Vector3 norm) 
        {
            A = a;
            B = b;
            Normal = norm;
        }

        public Vector3 VectorProjection(Vector3 v)
        {
            var v1 = v - A; // A as coord sys start
            var ab = B - A;

            var nm = ab.normalized;
            float dot = Vector3.Dot(nm, v1);
            dot = dot < 0 ? 0 : dot; // clamping if opposite
            var relP = dot * nm; //relative projections

            relP = Vector3.ClampMagnitude(relP, Length);

            return A + relP;//add offset
        }

        public float RelativeDistance(Vector3 pointOnLine)
        {
            var proj = VectorProjection(pointOnLine);

            float l = Length;
            float segLen = (proj - A).magnitude;

            return segLen / l;
        }

        public Vector3 RelPosition(float v, bool AtoB = true)
        {
            v = Mathf.Clamp01(v);
            v = AtoB ? v : 1 - v;

            return Vector3.Lerp(A, B, v);
        }

        internal Line ShrinkBy(float padding)
        {
            float percent = padding / (Length * 2);
            percent = Mathf.Clamp01(percent);

            var dir = B - A;

            var rv = new Line(A + dir * percent, B - dir * percent, this.Normal);
            return rv;
        }

        internal Line CutOut(Vector3 b, Vector2 normal)
        {
            Vector3 P = this.VectorProjection(b);

            if (P == A || P == B)
                return new Line(A, B, Normal);

            if (Vector2.Dot(P - A, normal) > 0)
            {
                return new Line(P, B, Normal);
            }
            else
            {
                return new Line(A, P, Normal);
            }
        }
    }
}
