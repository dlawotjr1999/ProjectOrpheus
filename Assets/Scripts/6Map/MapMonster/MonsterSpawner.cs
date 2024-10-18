using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterSpawnPoint m_point;
    public void SetPoint(MonsterSpawnPoint _point, int _idx) 
    {
        m_point = _point;
        SpawnerIdx = _idx;
        GetComponent<MeshRenderer>().enabled = false;
    }

    [SerializeField]
    private EMonsterName m_spawnMonster;
    [SerializeField]
    private int m_spawnNum = 1;
    [SerializeField]
    private float m_respawnTime = 150;

    private int SpawnedNum { get; set; } = 0;
    public SpawnerData SpawnerData { get { return new(m_point, SpawnerIdx); } }

    private int SpawnerIdx { get; set; }
    public Vector3 SpawnPosition { get { return transform.position; } }
    public float RangeMultiplier { get { return m_point.RangeMultiplier; } }

    private WolfPeckScript CurPeck { get; set; }

    public void SpawnMonster()
    {
        int need = m_spawnNum - SpawnedNum;
        for(int i=0;i<need;i++)
        {
            CreateMonster(m_spawnMonster);
        }
    }
    public void SpawnMonster(MonsterSaveData _data)
    {
        CreateMonster(_data.MonsterEnum);
    }
    private MonsterScript CreateMonster(EMonsterName _monster)
    {
        if(_monster == EMonsterName.WOLF && CurPeck == null)
        {
            GameObject peck = GameManager.GetWolfPeckPrefab(transform.position);
            CurPeck = peck.GetComponent<WolfPeckScript>();
        }

        GameObject monster = GameManager.GetMonsterObj(_monster);

        float dist = Random.Range(1.5f, 2);
        Vector2 dir = dist * FunctionDefine.RotateVector2(Vector2.right, Random.Range(0, 360f));

        Vector3 offset = new(dir.x, 0, dir.y);
        monster.transform.position = transform.position + offset;

        MonsterScript script = monster.GetComponent<MonsterScript>();
        m_point.AddMonster(script);
        script.SetSpawnPoint(this);

        if(_monster == EMonsterName.WOLF) { CurPeck.AddWolf((WolfScript)script); }

        SpawnedNum++;

        return script;
    }

    public void TooFar()
    {
        if (CurPeck != null) { CurPeck.ReleaseWolfs(); }
    }
    public void MonsterDespawned(MonsterScript _monster)
    {
        SpawnedNum--;
        m_point.MonsterDead(_monster);
        StartCoroutine(RespawnCoroutine(_monster.MonsterEnum));
    }
    private IEnumerator RespawnCoroutine(EMonsterName _monster)
    {
        yield return new WaitForSeconds(m_respawnTime);
        if (m_point.IsPlayerNear) { CreateMonster(_monster); }
    }
}
