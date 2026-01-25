using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shpleeble
{
    public class ShpleebleFactory
    {
        private readonly ShpleeblePrefabCache _prefabCache;

        public ShpleebleFactory(ShpleeblePrefabCache prefabCache)
        {
            _prefabCache = prefabCache;
        }

        public ShpleebleHandle Create()
        {
            if (!_prefabCache.IsReady)
            {
                throw new InvalidOperationException("Shpleeble prefab not ready");
            }

            ShpleebleController controller = GameObject.Instantiate(_prefabCache.GetPrefab().gameObject).GetComponent<ShpleebleController>();

            ShpleebleView view = ShpleebleView.FromRoot(controller.transform);
            controller.Initialize(view);
            controller.Activate();

            return new ShpleebleHandle(controller);
        }
    }
}
