//
//  TransformAction.cs
//  TundraUtils
//
//  Created on 01/04/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using System;
using System.Collections.Generic;

using UnityEngine;


namespace TundraUtils.Actions
{
    public enum EAKey
    { 
        Position,
        Rotation,
        Scale
    }

    public interface ITAction
    {
        void Update(float dt);
        bool Completed { get; } 
        float Duration { get; }
        float ElapsedToDuration { get; }
    }

    public class Reversed : AAction
    {
        private ITAction _inner = null;

        public Reversed(ITAction action) : base(action.Duration)
        {
            _inner = action;
        }

        protected override void OnStart()
        {
            _timePassed = Duration - 0.01f;
        }

        protected override void UpdateImpl(float dt)
        { 
            _inner.Update(-dt);
        }
    }

    public abstract class AAction : ITAction
    {
        protected float _duration = -1;
        protected float _timePassed = 0;
        private bool _started = false; 

        public virtual bool Completed
        {
            get { return _timePassed >= _duration; }
        }

        public bool Started
        {
            get { return _started; }
        }

        public float Duration { get { return _duration; } }
        public float TimePassed { get { return _timePassed; } }

        public AAction(float duration)
        {
            _duration = duration;
            Debug.Assert(duration != 0);
        }

        public void Update(float dt)
        {
            if (Completed)
                return;
            if (!_started)
            {
                _started = true;
                OnStart();
            }

            _timePassed += dt;
            UpdateImpl(dt);
        }

        public float ElapsedToDuration
        {
            get { return _timePassed / _duration; }
        }
        protected abstract void UpdateImpl(float dt);
        protected abstract void OnStart();
    }

    public class CallFunc : ITAction
    {
        public bool Completed { get { return _callback == null; } } 

        public float Duration { get { return -1; } }
        public float ElapsedToDuration {  get { return 1; } }
        private Action _callback = null;

        public CallFunc(Action callback)
        {
            _callback = callback;
        }

        public void Update(float dt)
        {
            if (Completed)
                return;

            _callback.Invoke();
            _callback = null;
        }
    }

    public class Dealy : AAction
    {
        public Dealy(float duration) : base(duration)
        {
        } 

        protected override void OnStart()
        { 
        }

        protected override void UpdateImpl(float dt)
        {
        }
    }

    public class MoveTo : AAction
    {
        private Transform _object;
        private Vector2 _destination;

        private Vector3 _origin;

        public MoveTo(Transform obj, Vector2 dest, float duration) : base(duration)
        {
            _object = obj;
            _destination = dest;
        } 

        protected override void UpdateImpl(float dt)
        {
            Vector3 pos = Vector2.Lerp(_origin, _destination, ElapsedToDuration);
            pos.z = _origin.z;

            _object.position = pos;
        }

        protected override void OnStart()
        {
            _origin = _object.position;
        }
    }

    public class MoveBy : AAction
    {
        private Transform _object;
        private Vector2 _by;

        private Vector3 _origin;
        private Vector2 _prev = Vector2.zero;

        public MoveBy(Transform obj, Vector2 by, float duration) : base(duration)
        {
            _object = obj;
            _by = by;
        } 

        protected override void UpdateImpl(float dt)
        {
            var cur = Vector2.Lerp(Vector2.zero, _by, ElapsedToDuration);
            Vector3 step = cur - _prev;
            _prev = cur;

            _object.position = _object.position + step;
        }

        protected override void OnStart()
        {
            _origin = _object.position;
        }
    }

    public class RotateTo : AAction
    {
        private Transform _object;
        private Vector3 _targetRotation;
        private Vector3 _originRotation;

        public RotateTo(Transform obj, float toAngle, float duration) : base(duration)
        {
            _object = obj;
            _targetRotation = new Vector3(0, 0, toAngle);
        } 

        protected override void UpdateImpl(float dt)
        {
            _object.eulerAngles = Vector3.Lerp(_originRotation, _targetRotation, ElapsedToDuration);
        }

        protected override void OnStart()
        {
            _originRotation = _object.eulerAngles;
        }
    }

    public class RotateBy : AAction
    {
        private RotateTo _inner;

        public RotateBy(Transform obj, float byAngle, float duration) : base(duration)
        {
            _inner = new RotateTo(obj, obj.eulerAngles.z + byAngle, duration);
        } 

        protected override void OnStart()
        { 
        }

        protected override void UpdateImpl(float dt)
        {
            _inner.Update(dt);
        }
    }

    public class EaseMove : AAction
    {
        private Transform _object;
        private Vector3 _targetPos;
        private Vector3 _startPos;
        private EEaseType _type; 

        public EaseMove(Transform obj, Vector3 target, EEaseType type, float duration) : base(duration)
        {
            _targetPos = target;
            _object = obj;
        } 

        protected override void OnStart()
        {
            _startPos = _object.position;   
        }

        protected override void UpdateImpl(float dt)
        { 
            _object.position = Ease.Interpolate(_type, _startPos, _targetPos, ElapsedToDuration);
        }
    }

    public class MoveWithAcceleration : AAction
    {
        private int _index = 0;
        private float _target = 0; 

        private Transform _object; 

        private Vector3 _start; 
        private Vector3 _speed;
        private Vector3 _acc;

        public MoveWithAcceleration(Transform obj, Vector3 deltaDist, Vector2 deltaV, float duration) : base(duration)
        {
            _object = obj;  
            _speed = deltaDist / duration;
            _acc = deltaV / duration;
        } 

        protected override void OnStart()
        {
            _start = _object.position;
        }

        protected override void UpdateImpl(float dt)
        { 
            var linearDelta = Vector3.zero;
            var at2_by2 = (_acc * TimePassed * TimePassed * 0.5f);
            linearDelta = _speed * TimePassed + at2_by2;

            _object.position = _start + linearDelta; 
        }
    }

    public class AcceleratedRotation : AAction
    {  
        private Vector3 _originRotation;
        private float _speed;
        private float _acc;

        private Transform _object;
        private EEaseType _type;

        private float _steadyPart = 0; 

        public AcceleratedRotation(Transform obj, float speed, float acc, float duration) : base(duration)
        {
            _object = obj;
            _speed = speed;
            _acc = acc;
        }

        protected override void UpdateImpl(float dt)
        { 
            var at2_by2 = (_acc * TimePassed * TimePassed * 0.5f);
            float linearDelta = _speed * TimePassed + at2_by2;

            _object.eulerAngles = _object.eulerAngles + new Vector3(0, 0, linearDelta);
        }

        protected override void OnStart()
        {
            _originRotation = _object.eulerAngles;
            _speed = _originRotation.z + _speed;
        } 
    }

    public class ParallelActions : AAction
    {
        private List<ITAction> _actions;
        public ParallelActions(List<ITAction> actions, float duration = -1) : base(duration)
        {
            if (duration < 0)
            {
                foreach (var act in actions)
                {
                    duration = Mathf.Max(duration, act.Duration);
                }
                _duration = duration;
            }
            _actions = actions;
        } 

        protected override void OnStart() {  }

        protected override void UpdateImpl(float dt)
        { 
            foreach (var act in _actions)
            {
                act.Update(dt);
            }
        }
    } 
}
