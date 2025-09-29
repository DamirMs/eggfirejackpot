using PT.Logic.Dependency;
using UnityEngine;
using Zenject;

namespace PT.Tools.ObjectPool
{
    public class MonoBehPool<T> : PoolBase<T> where T : MonoBehaviour
    {
        private readonly IFactoryZenject _factory; // <-- added

        public MonoBehPool(IFactoryZenject factory) 
        {
            _factory = factory;
        }

        protected override T CreateObject()
        {
            return _factory.InstantiateForComponent<T>(_prefab, _parent);
        }

        protected override void OnGet(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        protected override void OnSet(T obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}