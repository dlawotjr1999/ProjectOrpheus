using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnerData
{
    public int PointHash;
    public int SpawnerIdx;
    public SpawnerData(MonsterSpawnPoint _point, int _idx)
    {
        PointHash = _point.GetHashCode();
        SpawnerIdx = _idx;
    }
}

public enum EAreaType
{
    AISLE,
    FIELD,


    LAST
}

public class MonsterSpawnPoint : MonoBehaviour, IHaveData
{
    [SerializeField]
    private MonsterSpawner[] m_spawners;

    [SerializeField]
    private EAreaType m_areaType;

    private readonly float[] m_rangeMultiplier = new float[(int)EAreaType.LAST] { 1, 1.5f };

    private bool IsShowingMonster { get; set; }
    public bool IsPlayerNear { get { return PlayerDistance <= NearPlayerDist; } }

    private float PlayerDistance { get { return Vector3.Distance(transform.position, PlayManager.PlayerPos); } }
    private readonly float NearPlayerDist = 100;

    public float RangeMultiplier { get { return m_rangeMultiplier[(int)m_areaType]; } }

    public Vector3 SpawnPosition { get { return transform.position; } }


    [SerializeField]
    private List<MonsterScript> m_spawnedMonsters = new();
    public List<MonsterScript> SpawnedMonsters { get { return m_spawnedMonsters; } }

    public void AddMonster(MonsterScript _monster)
    {
        if (m_spawnedMonsters.Contains(_monster)) { return; }
        m_spawnedMonsters.Add(_monster);
    }


    public readonly List<MonsterSaveData> MonsterDataCache = new();
    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { return; }

        MonsterDataCache.Clear();

        SaveData data = PlayManager.CurSaveData;
        foreach (MonsterSaveData monster in data.MonsterData)
        {
            if (monster.SpawnerData.PointHash != GetHashCode()) { continue; }
            MonsterDataCache.Add(monster);
        }
    }
    private void SpawnMonsters()
    {
        for (int i = 0; i<m_spawners.Length; i++)
        {
            MonsterSpawner spawner = m_spawners[i];
            foreach (MonsterSaveData data in MonsterDataCache)
            {
                if (data.SpawnerData.SpawnerIdx != i) { continue; }
                spawner.SpawnMonster(data);
            }
            spawner.SpawnMonster();
        }
        IsShowingMonster = true;
    }
    private void DespawnMonsters()
    {
        MonsterDataCache.Clear();
        foreach(MonsterSpawner spawner in m_spawners) { spawner.TooFar(); }
        foreach (MonsterScript monster in m_spawnedMonsters)
        {
            MonsterSaveData data = new(monster);
            MonsterDataCache.Add(data);
            monster.DestroyMonster();
        }
        m_spawnedMonsters.Clear();
        IsShowingMonster = false;
    }
    public void MonsterDead(MonsterScript _monster)
    {
        m_spawnedMonsters.Remove(_monster);
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        foreach (MonsterScript monster in m_spawnedMonsters)
        {
            MonsterSaveData save = new(monster);
            data.MonsterData.Add(save);
        }
    }


    private void CheckDistance()
    {
        if(IsShowingMonster && !IsPlayerNear) { DespawnMonsters(); }
        else if(!IsShowingMonster && IsPlayerNear) { SpawnMonsters(); }
    }



    private void SetComps()
    {
        m_spawners = GetComponentsInChildren<MonsterSpawner>();
        for (int i = 0; i<m_spawners.Length; i++)
        {
            MonsterSpawner spawner = m_spawners[i];
            spawner.SetPoint(this, i);
        }
    }

    private void Awake()
    {
        LoadData();
        SetComps();
    }
    private void Start()
    {
        if (IsPlayerNear) { SpawnMonsters(); }
    }

    private void Update()
    {
        CheckDistance();
    }
}
