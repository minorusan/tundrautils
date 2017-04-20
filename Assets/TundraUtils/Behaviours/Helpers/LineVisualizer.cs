//
//  LineVisualizer.cs
//  TundraUtils
//
//  Created on 10/11/2016.
//  Copyright © 2016 Tundra Mobile. All rights reserved.
//
using UnityEngine;

using TundraUtils.LineStyles;


namespace TundraUtils.Behaviours.Helpers
{
    public class LineVisualizer : MonoBehaviour
    {
        public Transform TransformA;
        public Transform TransformB;
        public Transform Point;
        public float Offset;

        private void OnDrawGizmos()
        {
            if (!TransformA || !TransformB || !Point) return;

                PerformDraw(); 
        } 

        private void PerformDraw()
        {
            var A = TransformA.position;
            var B = TransformB.position;
            var mid = (B + A) / 2;

            Line src = new Line(A, B);
            var Point = this.Point.position;
            Vector3 origProj = src.VectorProjection(Point);
            Vector3 origDelta = origProj - mid;

            float scale = Offset * 2 / src.Length;
            scale = 1 - scale;
            Vector3 delta = origDelta * scale;
            Vector3 maxDelta = (B - A) * scale * 0.5f;

            float r = 0.12f;

            Line dst = new Line(mid + maxDelta, mid - maxDelta);
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(A, r);
            Gizmos.DrawWireSphere(B, r);

            Gizmos.DrawLine(Point, origProj);
            Gizmos.DrawLine(A, B);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Point, r);
            Gizmos.DrawWireCube(dst.A, new Vector3(r, r, r) * .8f);
            Gizmos.DrawWireCube(dst.B, new Vector3(r, r, r) * .8f);

            var projected = delta + mid;
            Gizmos.DrawWireSphere(projected, r);

            Gizmos.color = Color.cyan;

            Gizmos.DrawLine(Point, projected);
        }

        public Vector3 Project(Vector3 point, Line source, Line destination)
        {
            float v = source.RelativeDistance(point);
            var dbg = destination.RelPosition(v);
            return dbg;
        }
    }
}
