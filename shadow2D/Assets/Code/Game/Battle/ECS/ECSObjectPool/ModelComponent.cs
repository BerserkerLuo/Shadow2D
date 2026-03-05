using System.Collections;
using UnityEngine;

namespace ECS
{
    public class ModelComponent<T> where T : UnityEngine.Component
    {
        private Transform transform = null;
        private T instance = default;
        private bool isGet = false;
        private bool canAdd = true;

        public T Instance{
            get {
                if (isGet)
                    return instance;

                instance = transform.GetComponent<T>();
                if (instance == null && canAdd)
                    instance = transform.gameObject.AddComponent<T>();
                return instance;
            }
        }

        public ModelComponent(Transform transform,bool canAdd = true) {
            this.transform = transform;
            this.canAdd = canAdd;
        }
    }
}