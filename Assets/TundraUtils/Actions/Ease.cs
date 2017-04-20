//
//  Ease.cs
//  TundraUtils
//
//  Created on 08/03/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using UnityEngine;


namespace TundraUtils.Actions
{ 
    public enum EEaseType
    {
        EaseIn,
        EaseOut,
        InOut
    }

    public static class Ease
    {
        private static float TransformReativeTime(EEaseType Type, float rt)
        {
            if (Type == EEaseType.EaseIn)
            {
                rt *= 0.5f;
            }
            else if (Type == EEaseType.EaseOut)
            {
                rt = (rt * 0.5f) + 0.5f;
            }

            return rt;
        }

        public static Vector3 Interpolate(EEaseType Type, Vector3 start, Vector3 end, float t01)
        {
            Vector3 delta = end - start;
            t01 = Mathf.Clamp01(t01);
            float relT = TransformReativeTime(Type, t01);
            float c = Type != EEaseType.InOut ? 2 : 1;

            if (Type == EEaseType.EaseOut)
            {
                delta.x *= ((float)InOut(relT) - 0.5f) * c;
                delta.y *= ((float)InOut(relT) - 0.5f) * c;
                delta.z *= ((float)InOut(relT) - 0.5f) * c;
            }
            else
            {
                delta.x *= (float)InOut(relT) * c;
                delta.y *= (float)InOut(relT) * c;
                delta.z *= (float)InOut(relT) * c;
            }

            return delta;
        }

        public static float Interpolate(EEaseType Type, float start, float end, float t01)
        {
            float delta = end - start;
            t01 = Mathf.Clamp01(t01);
            float relT = TransformReativeTime(Type, t01);
            float c = Type != EEaseType.InOut ? 2 : 1;

            if (Type == EEaseType.EaseOut)
            {
                delta *= ((float)InOut(relT) - 0.5f) * c; 
            }
            else
            {
                delta *= (float)InOut(relT) * c; 
            }

            return delta;
        }

        public static double InOut(float t)
        {
            if (t <= 0.5f)
                return 2.0f * (t * t);
            t -= 0.5f;
            return 2.0f * t * (1.0f - t) + 0.5f;
        }
    }
}
