// This Source Code Form is subject to the terms of the Mozilla
// Public License, v. 2.0. If a copy of the MPL was not distributed
// with this file, You can obtain one at http://mozilla.org/MPL/2.0
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TundraUtils.Behaviours.ObjectPooling
{
    public class PoolableObject : MonoBehaviour
    {
        public string PoolID = "default";

        private PoolableGOContainer _container = null;

        protected bool _active = false;

        public void SetContainer(PoolableGOContainer container)
        {
            _container = container;
        }

        public virtual void Awake()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Deactivate();
        }

        public void Deactivate()
        {
            StopAllCoroutines();
            if (_active)
                _container.Push(this);

            _active = false;
        }

        public virtual void OnPulled()
        {
            _active = true;
        }

        internal void OnLoadedToPool()
        {
        }
    }
}