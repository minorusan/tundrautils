// This Source Code Form is subject to the terms of the Mozilla
// Public License, v. 2.0. If a copy of the MPL was not distributed
// with this file, You can obtain one at http://mozilla.org/MPL/2.0

using System.Collections.Generic;

using UnityEngine;


namespace TundraUtils.Behaviours.ObjectPooling
{
    public class GOPool : MonoBehaviour
    {
        public PoolableGOContainer[] Containers;

        private Dictionary<string, PoolableGOContainer> _containerDict = new Dictionary<string, PoolableGOContainer>();

        private void Start()
        {
            PreloadObjects();

            foreach (var container in Containers)
            {
                _containerDict.Add(container.Prototype.PoolID, container);
            }
        }

        private void PreloadObjects()
        {
            foreach (var container in Containers)
            {
                container.Initiate();
            }
        }

        public PoolableObject TryGet(string id)
        {
            return _containerDict[id].Pull();
        }
    }
}
