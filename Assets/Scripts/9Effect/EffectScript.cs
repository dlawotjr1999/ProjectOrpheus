using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectScript : MonoBehaviour, IPoolable
{
    public ObjectPool<GameObject> OriginalPool { get; set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }
    public void OnPoolGet() { }
    public void ReleaseToPool() { OriginalPool.Release(gameObject); }


    public void SetDestroyTime(float _time)
    {
        StartCoroutine(ReturnEffect(_time));
    }
    private IEnumerator ReturnEffect(float _time)
    {
        yield return new WaitForSeconds(_time);
        ReleaseToPool();
    }
}
