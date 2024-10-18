#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDebugger : MonoBehaviour
{
    private static BattleDebugger Inst;

    [SerializeField]
    private bool m_ableDebug = true;
    [SerializeField]
    private bool m_hideHitInfo = false;
    [SerializeField]
    private EPowerName[] m_powerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT];
    [SerializeField]
    private List<MonsterScript> m_monsterList = new();
    [SerializeField]
    private MonsterSpawnPoint m_spawnPoint;

    public static bool HideHitInfo { get { if (Inst == null) { return false; } return Inst.m_hideHitInfo; } }

    private void DrawDebugs()                               // 오브젝트별 디버깅
    {
        if (!m_ableDebug) { return; }
        for (int i = 0; i<m_monsterList.Count; i++)
        {
            MonsterScript monster = m_monsterList[i];
            if (monster == null || monster.IsDead) { m_monsterList.Remove(monster); continue; }
            DrawView(monster);
            DrawTarget(monster);
        }
    }

    private void DrawView(MonsterScript _monster)             // 시야각 표시
    {
        Vector3 position = _monster.transform.position;
        float range = _monster.ViewRange;
        Gizmos.DrawWireSphere(position, range);

        float deg = _monster.ViewAngle;
        float dir = _monster.Direction;
        Vector3 right = FunctionDefine.AngleToDir(dir + deg * 0.5f);
        Vector3 left = FunctionDefine.AngleToDir(dir - deg * 0.5f);
        Vector3 look = FunctionDefine.AngleToDir(dir);

        Debug.DrawRay(position, right * range, Color.blue);
        Debug.DrawRay(position, left * range, Color.blue);
        Debug.DrawRay(position, look * range, Color.cyan);
    }
    private void DrawTarget(MonsterScript _monster)           // 공격 대상 표시
    {
        if (!m_ableDebug) { return; }
        ObjectScript target = _monster.CurTarget;
        if (target == null) { return; }

        float range = _monster.ViewRange;
        Vector3 pos = _monster.transform.position;
        float deg = _monster.ViewAngle;
        float rot = _monster.Direction;
        Vector3 look = FunctionDefine.AngleToDir(rot);

        Collider[] targets = Physics.OverlapSphere(pos, range);

        if (targets.Length == 0) return;
        foreach (Collider col in targets)
        {
            if (!col.TryGetComponent<PlayerController>(out var player)) { continue; }
            Vector3 targetPos = col.transform.position;
            Vector3 targetDir = (targetPos - pos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(look, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= deg * 0.5f && !Physics.Raycast(pos, targetDir, range))
            {
                Debug.DrawLine(pos, targetPos, Color.red);
            }
        }
    }


    private bool ShowingCreateMonster { get; set; }
    private void ShowCreateMonster()
    {
        if (GUI.Button(new Rect(20, 50, 280, 50), "취소"))
        {
            ShowingCreateMonster = false;
        }

        for (int i = 0; i<MonsterCount; i++)
        {
            int xPos = i%2 == 0 ? 30 : 165;
            int yPos = 128 + 32 * (i / 2);
            if (GUI.Button(new Rect(xPos, yPos, 125, 30), $"{m_monsterNames[i]}"))
            {
                CreateMonster(i);
            }
        }
    }


    private void CreateMonster(int _idx)
    {
        GameObject prefab = GameManager.GetMonsterObj((EMonsterName)_idx);
        if (prefab == null) { Debug.Log("몬스터 미완성"); return; }

        Vector3 point;
        if (m_spawnPoint != null) { point = m_spawnPoint.SpawnPosition; }
        else { point = Vector3.zero; }

        prefab.transform.SetParent(null);
        prefab.transform.position = point;
        prefab.transform.localEulerAngles = new(0, Random.Range(-180f, 180f), 0);
        MonsterScript script = prefab.GetComponent<MonsterScript>();
        m_monsterList.Add(script);
        Debug.Log($"{script.ObjectName} 생성됨");
    }


    private const int MonsterCount = (int)EMonsterName.LAST;
    private readonly string[] m_monsterNames = new string[MonsterCount];

    private void SetTempPower()
    {
        for (int i = 0; i<(int)EPowerName.LAST; i++)
        {
            PlayManager.ObtainPower((EPowerName)i);
        }
        for (int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++)
        {
            PlayManager.RegisterPowerSlot(m_powerSlot[i], i);
        }
    }



    public void StartDebugger()
    {
        m_monsterList.Clear();
        MonsterScript[] monsters = FindObjectsOfType<MonsterScript>();
        foreach (MonsterScript monster in monsters) { m_monsterList.Add(monster); }

        SetTempPower();
    }
    private void SetInfos()
    {
        for (int i = 0; i<MonsterCount; i++)
        {
            m_monsterNames[i] = GameManager.GetMonsterInfo((EMonsterName)i).MonsterName;
        }
    }

    private void Awake()
    {
        Inst = this;
        SetInfos();
    }
    private void Start()
    {
        StartDebugger();
    }
    private void OnDrawGizmos()
    {
        DrawDebugs();
    }
    private void OnGUI()
    {
        if (ShowingCreateMonster)
        {
            ShowCreateMonster();
        }
        else
        {
            if (GUI.Button(new Rect(20, 50, 150, 50), "몬스터 생성"))
            {
                ShowingCreateMonster = true;
            }
        }
    }
}
#endif