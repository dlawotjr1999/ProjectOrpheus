using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    public MonsterScriptable MonsterData { get; private set; }
    public string MonsterName { get { return MonsterData.MonsterName; } }
    public string MonsterDescription { get { return MonsterData.Description; } }
    public bool Cleared { get; private set; }
    public void ClearMonster() { Cleared = true; }
    public MonsterInfo(MonsterScriptable _scriptable)
    {
        MonsterData = _scriptable;
        Cleared = false;
    }
}

public class MonsterManager : MonoBehaviour
{
    private readonly MonsterInfo[] m_monsterInfo = new MonsterInfo[(int)EMonsterName.LAST];
    public MonsterInfo GetMonsterInfo(EMonsterName _monster) { return m_monsterInfo[(int)_monster]; }

    [SerializeField]
    private MonsterScriptable[] m_monsterData;
    public MonsterScriptable[] MonsterData { get { return m_monsterData; } }
    public GameObject[] MonsterArray { get {
            GameObject[] array = new GameObject[m_monsterData.Length];
            for(int i = 0; i<array.Length; i++) { array[i] = m_monsterData[i].MonsterPrefab; }
            return array; ; } }

    public void SetMonsterData(List<MonsterScriptable> _mons)
    {
        m_monsterData = new MonsterScriptable[(int)EMonsterName.LAST];
        for(int i = 0; i<_mons.Count; i++) { m_monsterData[i] = _mons[i]; }
    }
    public MonsterScriptable GetMonsterData(EMonsterName _monster)
    {
        return m_monsterData[(int)_monster];
    }
    public GameObject GetMonsterObj(EMonsterName _monster)
    {
        GameObject obj = PoolManager.GetObject(m_monsterData[(int)_monster].MonsterPrefab);
        return obj;
    }

    [SerializeField]
    private GameObject m_wolfPeckPrefab;
    public GameObject GetWolfPeckPrefab(Vector3 _pos) 
    {
        GameObject peck = Instantiate(m_wolfPeckPrefab, _pos, Quaternion.identity);
        return peck;
    }


    public bool CheckNClearMonster(EMonsterName _monster)
    {
        int idx = (int)_monster;
        bool cleared = m_monsterInfo[idx].Cleared;
        if(!cleared) { m_monsterInfo[idx].ClearMonster(); }
        return cleared;
    }


    public static EMonsterName ID2Monster(string _id)
    {
        int.TryParse(_id[1..], out int code);
        int area = code / 100;
        int idx = code % 100 - 1;
        int monsterIdx = Code2Idx(area, idx);
        return monsterIdx > 0 ? (EMonsterName)monsterIdx : EMonsterName.LAST;
    }

    public static int Code2Idx(int _area, int _idx)
    {
        switch (_area)
        {
            case 1: return _idx;
            case 2: return _idx + (int)EMonsterName.MISERABLE_HHM;
            case 3: return _idx + (int)EMonsterName.ARROGANT_HHM;
            case 5: return _idx + (int)EMonsterName.SKURRABY_LIFE;
        }
        return -1;
    }


    public void SetManager()
    {
        for (int i = 0; i<(int)EMonsterName.LAST; i++)
        {
            m_monsterInfo[i] = new MonsterInfo(GameManager.GetMonsterData((EMonsterName)i));
        }
    }
}
