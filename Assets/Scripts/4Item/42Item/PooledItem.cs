using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledItem : MonoBehaviour, IPoolable
{
    public ObjectPool<GameObject> OriginalPool { get; private set; }

    public virtual void SetPool(ObjectPool<GameObject> _pool)
    {
        OriginalPool = _pool;
    }
    public virtual void OnPoolGet() { }
    public virtual void ReleaseToPool()
    {
        OriginalPool.Release(gameObject);
    }


    public virtual void OnEnable()
    {
        
    }
}
