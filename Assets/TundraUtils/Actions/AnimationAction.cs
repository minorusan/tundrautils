//
//  AnimationAction.cs
//  TundraUtils
//
//  Created on 10/08/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using System.Collections.Generic;

using UnityEngine;


namespace TundraUtils.Actions
{
    public enum EAnimType
    {
        Linear,
        EaseIn,
        EaseOut,
        Spline
    }

    public struct AnimKey
    {
        public float Mark;
        public Vector3 Val;
        public EAnimType Type;

        public static AnimKey Zero = new AnimKey(0, Vector3.zero);

        public AnimKey(float mark, Vector3 val, EAnimType lt = EAnimType.Linear)
        {
            Mark = mark;
            Val = val;
            Type = lt;
        }
    }

    public class AnimationAction : AAction
    {
        private Dictionary<EAKey, List<AnimKey>> _tracks = new Dictionary<EAKey, List<AnimKey>>();
        private Transform _target;

        public AnimationAction(Transform target) : base(100)
        {
            _target = target;

            var posSK = new AnimKey(0, _target.position);
            var rotSK = new AnimKey(0, _target.eulerAngles);
            var sclSK = new AnimKey(0, _target.localScale);

            _tracks.Add(EAKey.Position, new List<AnimKey> { posSK });
            _tracks.Add(EAKey.Rotation, new List<AnimKey> { rotSK });
            _tracks.Add(EAKey.Scale,    new List<AnimKey> { sclSK });
        } 

        protected override void OnStart()
        {
            foreach (var kv in _tracks)
            {
                SortAnimKeys(kv.Value);
            }
            _duration = TotalDuration();
            NormalizeKeys(_duration);
        }

        private void SortAnimKeys(List<AnimKey> keys)
        {
            keys.Sort((x, y) => x.Mark.CompareTo(y.Mark));
        }

        private void NormalizeKeys(float dur)
        {
            foreach (var kv in _tracks)
            {
                for (int i = 0; i < kv.Value.Count; ++i)
                {
                    var akey = kv.Value[i];
                    akey.Mark = akey.Mark / dur;

                    kv.Value[i] = akey;
                }
            }
        }

        private float TotalDuration()
        {
            float maxKeyTime = 0;
            foreach (var kv in _tracks)
            {
                foreach (var akey in kv.Value)
                {
                    maxKeyTime = Mathf.Max(maxKeyTime, akey.Mark);
                }
            }
            return maxKeyTime;
        }

        public void Insert(EAKey key, AnimKey val)
        {
            Debug.Assert(!Started,
                "Adding keys after animation have started is illegal, you know. So don't do that, it's bad.");

            _tracks[key].Add(val);
        }

        private Vector3 Interpolate(float relT, List<AnimKey> keys)
        {
            if (keys.Count == 1)
                return keys[0].Val;

            AnimKey current = AnimKey.Zero;
            AnimKey next = AnimKey.Zero; 
            bool nextFound = false;

            for (int i = 0; i < keys.Count; ++i)
            {
                if (keys[i].Mark <= relT)
                {
                    current = keys[i];
                    bool last = (i + 1) >= keys.Count;

                    if (!last && keys[i + 1].Mark >= relT)
                    {
                        nextFound = true;
                        next = keys[i + 1];
                        break;
                    }
                }
            } 
            if (!nextFound)
                return current.Val;

            float localTime = (relT - current.Mark) / (next.Mark - current.Mark);
            if (current.Type == EAnimType.Linear)
                return InterpolateLinear(current.Val, next.Val, localTime);
            else
                Debug.LogError("..not yet implemented..");

            return Vector3.one;
        }

        private Vector3 InterpolateLinear(Vector3 val1, Vector3 val2, float relT)
        {
            return Vector3.Lerp(val1, val2, relT);
        }

        protected override void UpdateImpl(float dt)
        {
            float relDT = ElapsedToDuration;

            var tpos = Interpolate(relDT, _tracks[EAKey.Position]);
            var trot = Interpolate(relDT, _tracks[EAKey.Rotation]);
            var tscl = Interpolate(relDT, _tracks[EAKey.Scale]);

            Debug.Assert(!CheckNaN(tpos));
            Debug.Assert(!CheckNaN(trot));
            Debug.Assert(!CheckNaN(tscl));

            _target.position = tpos;
            _target.eulerAngles = trot;
            _target.localScale = tscl;
        }

        private bool CheckNaN(Vector3 v)
        {
            if (float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z))
                return true;

            return false;
        }
    }
}
