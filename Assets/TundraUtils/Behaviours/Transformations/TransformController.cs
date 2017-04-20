//
//  TransformController.cs
//  TundraUtils
//
//  Created on 07/12/2016.
//  Copyright © 2017 Tundra Mobile. All rights reserved.
//
using System;
using System.Collections.Generic;

using UnityEngine;

using TundraUtils.Actions;


namespace TundraUtils.Behaviours.Transformations
{
    public class TransformController : MonoBehaviour
    {
        public event Action OnAllTransformationsEnded;
        public List<ITAction> _actions = new List<ITAction>(); 

        public float TimeScale = 1;
        private bool _paused = false; 

        public bool Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }

        public bool Empty
        {
            get
            { 
                return _actions.Count == 0;
            }
        }

        public bool Running
        {
            get { return !Paused && !Empty; }
        }

        public float Progress
        {
            get
            {
                if (_actions.Count > 0)
                    return _actions[0].ElapsedToDuration;
                return 0;
            }
        }

        private void OnDestroy()
        {
            OnAllTransformationsEnded = null;
        }

        public void Update()
        {
            bool wasEmpty = Empty;
            if (Paused || wasEmpty)
                return;

            float dt = TimeScale * Time.deltaTime;
            this.UpdateActions(dt); 

            if (!wasEmpty && Empty)
            {
                if (null != OnAllTransformationsEnded)
                {
                    OnAllTransformationsEnded();
                }
            }
        }  
         
        public void PlaySequence(List<ITAction> acts, bool append = false)
        { 
            if (null != acts)
            {
                if (!append)
                {
                    _actions.Clear();
                }
                _actions.AddRange(acts);
            }
        } 

        public void ForceStop()
        {
            while (_actions.Count > 0)
            {
                UpdateActions(0.1f);
            } 
        } 

        public void PlayAction(ITAction pact)
        {
            PlaySequence(new List<ITAction> { pact });
        }

        private void UpdateActions(float dt)
        {
            if (_actions.Count > 0)
            {
                _actions[0].Update(dt);

                if (_actions[0].Completed)
                {
                    _actions.RemoveAt(0);
                }
            }
        }
    }
}
