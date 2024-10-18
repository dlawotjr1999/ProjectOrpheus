using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    private static readonly Dictionary<int, ObjectPool<GameObject>> m_pools = new();
    private readonly Dictionary<int, GameObject> m_objectList = new();

    private const int DEFAULT_ITEM_NUM = 8;
    private const int DEFAULT_SKILL_NUM = 4;
    private const int DEFAULT_MONSTER_NUM = 16;
    private const int DEFAULT_EFFECT_NUM = 4;
    private const int DEFAULT_SE_NUM = 128;


    [SerializeField]
    private Transform m_poolListTransform;

    private static int CurHash { get; set; }

    public static GameObject GetObject(GameObject _obj)
    {
        int hash = _obj.GetHashCode();
        if (!m_pools.ContainsKey(hash)) { Debug.LogError($"풀에 추가되지 않은 {_obj.name}"); return null; }
        CurHash = hash;
        GameObject pooled = m_pools[hash].Get();
        return pooled;
    }

    private void CreatePools(GameObject[] _items, GameObject[] _skills, GameObject[] _monsters, GameObject[] _effects, GameObject _se)
    {
        // 아이템
        int itemNum = (int)EItemType.LAST + (int)EThrowItemName.LAST;
        for (int i = 0; i<itemNum; i++)
        {
            if (_items[i] == null) { continue; }
            InitPool(_items[i], DEFAULT_ITEM_NUM);
        }
        // 스킬
        for(int i = 0; i<(int)EPowerName.LAST; i++)
        {
            if (_skills[i] == null) { continue; }
            InitPool(_skills[i], DEFAULT_SKILL_NUM);
        }
        // 몬스터
        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            if (_monsters[i] == null) { continue; }
            InitPool(_monsters[i], DEFAULT_MONSTER_NUM);
        }
        // 이펙트
        for (int i = 0; i<(int)EEffectName.LAST; i++)
        {
            if (_effects[i] == null) { continue; }
            InitPool(_effects[i], DEFAULT_EFFECT_NUM);
        }
        // 소리
        InitPool(_se, DEFAULT_SE_NUM);
    }

    private void InitPool(GameObject _obj, int _num)
    {
        int hash = _obj.GetHashCode();
        CurHash = hash;
        m_pools[CurHash] = new(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy, true, _num, _num);
        m_objectList[CurHash] = _obj;
        for (int i = 0; i<_num; i++)
        {
            GameObject newItem = OnPoolCreate();
            newItem.GetComponent<IPoolable>().OriginalPool.Release(newItem);
        }
    }


    private GameObject OnPoolCreate()
    {
        GameObject item = Instantiate(m_objectList[CurHash]);
        item.GetComponent<IPoolable>().SetPool(m_pools[CurHash]);
        return item;
    }
    private void OnPoolGet(GameObject _item)
    { 
        _item.SetActive(true);
        _item.transform.SetParent(null); 
        _item.GetComponent<IPoolable>().OnPoolGet();
    }
    private void OnPoolRelease(GameObject _item) 
    {
        _item.transform.SetParent(m_poolListTransform);
        _item.SetActive(false); 
    }
    private void OnPoolDestroy(GameObject _item) { Destroy(_item); }


    public void SetManager(GameObject[] _items, GameObject[] _skills, GameObject[] _monsters, GameObject[] _effects, GameObject _se)
    {
        CreatePools(_items, _skills, _monsters, _effects, _se);
    }
}
