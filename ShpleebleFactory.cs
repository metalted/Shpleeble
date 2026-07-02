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
            
            GameObject.DontDestroyOnLoad(controller.gameObject);
            ShpleebleView view = ShpleebleView.FromRoot(controller.transform);
            controller.Initialize(view);
            controller.Activate();

            return new ShpleebleHandle(controller);
        }

        public ShpleeblePhysicsModel CreatePhysicsModel(ShpleebleData data, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if(!_prefabCache.IsReady)
            {
                throw new InvalidOperationException("Shpleeble prefabs not ready");
            }

            GameObject obj = GameObject.Instantiate(_prefabCache.GetPhysicsPrefab().gameObject, position, rotation);
            obj.transform.localScale = scale;
            obj.gameObject.SetActive(true);
            ShpleeblePhysicsModel pModel = obj.GetComponent<ShpleeblePhysicsModel>();
            pModel.SetCosmetics(data);
            pModel.SetInternalScale(scale);

            return pModel;            
        }
    }
}
