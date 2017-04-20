// This Source Code Form is subject to the terms of the Mozilla
// Public License, v. 2.0. If a copy of the MPL was not distributed
// with this file, You can obtain one at http://mozilla.org/MPL/2.0

using System.Collections.Generic;

using UnityEngine;


namespace TundraUtils.Behaviours.ObjectPooling
{
    public class PoolableGOContainer : MonoBehaviour
    {
        public Transform ContainerRoot;
        public PoolableObject Prototype;

        public int Capacity;

        private List<PoolableObject> _active = new List<PoolableObject>();
        private List<PoolableObject> _reserved = new List<PoolableObject>();

        public int ActiveCount
        {
            get
            {
                return _active.Count;
            }
        }

        public int ReservedCount
        {
            get
            {
                return _reserved.Count;
            }
        }

        public bool Full
        {
            get
            {
                return (_active.Count + _reserved.Count) >= Capacity;
            }
        }

        public void Push(PoolableObject obj)
        {
            int indx = GetActiveIndex(obj);
            if (indx >= 0)
            {
                _active.RemoveAt(indx);
                _reserved.Add(obj);
                obj.transform.parent = ContainerRoot;

                return;
            }

            Debug.Assert(false, "Cannot be pushed");
        }

        public PoolableObject Pull()
        {
            if (_reserved.Count == 0)
            {
                return null;
            }

            PoolableObject obj = _reserved[0];
            _reserved.RemoveAt(0);
            _active.Add(obj);

            obj.OnPulled();

            return obj;
        }

        public void Initiate()
        {
            while (!Full)
            {
                PoolableObject copy = GameObject.Instantiate<PoolableObject>(Prototype);
                copy.OnLoadedToPool();

                copy.SetContainer(this);

                _reserved.Add(copy);
                copy.transform.parent = ContainerRoot;
            }
        }

        public int GetActiveIndex(PoolableObject go)
        {
            int indx = 0;

            for (; indx < _active.Count; ++indx)
            {
                if (go == _active[indx])
                {
                    return indx;
                }
            }

            return -1;
        }
    }
}
