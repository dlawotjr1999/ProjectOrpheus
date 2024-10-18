using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SEPrefab : MonoBehaviour, IPoolable
{
    [SerializeField]
    private AudioSource m_se;

    public ObjectPool<GameObject> OriginalPool { get; private set; }
    public void SetPool(ObjectPool<GameObject> _pool) { OriginalPool = _pool; }

    public void OnPoolGet() { }

    public void PlaySE(AudioClip _clip, float _volume)
    {
        m_se.clip = _clip;
        m_se.volume = _volume;
        m_se.Play();
        StartCoroutine(DestroySound());
    }

    public void ReleaseToPool()
    {
        OriginalPool.Release(gameObject);
        m_se.clip = null;
    }


    private IEnumerator DestroySound()
    {
        while (m_se.isPlaying)
        {
            yield return new WaitForSeconds(0.01f);
        }
        ReleaseToPool();
    }
}
