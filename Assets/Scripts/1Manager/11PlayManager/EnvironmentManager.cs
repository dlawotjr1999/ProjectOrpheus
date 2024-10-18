using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour, IHaveData
{
    [SerializeField]
    private Transform[] m_mapPositioner = new Transform[2];     // 축척용
    public Vector3 MapLB { get { return m_mapPositioner[0].position; } }
    public Vector3 MapRT { get { return m_mapPositioner[1].position; } }
    public float MapWidth { get { return MapRT.x - MapLB.x; } }
    public float MapHeight { get { return MapRT.z - MapLB.z; } }

    [SerializeField]
    private GameObject m_mapObject;

    [SerializeField]
    private NPCScript[,] m_npcList;                             // NPC 목록

    public OasisNPC[] OasisList { get { return FunctionDefine.GetRow<NPCScript, OasisNPC>(m_npcList, ((int)ENPCType.OASIS)); } }
    public AltarScript[] AltarList { get { return FunctionDefine.GetRow<NPCScript, AltarScript>(m_npcList, ((int)ENPCType.ALTAR)); } }
    public SlateScript[] SlateList { get { return FunctionDefine.GetRow<NPCScript, SlateScript>(m_npcList, ((int)ENPCType.SLATE)); } }


    public void UnlockDialogue(NPCDialogue _dial)
    {
        SNPC npc = _dial.NPC;
        NPCScript script = m_npcList[(int)npc.Type, npc.Idx];
        script.UnlockDialogue(_dial.Idx);
    }



    [SerializeField]
    private MonsterSpawnPoint[] m_spawnPointList;               // 몬스터 스폰 장소
    public MonsterSpawnPoint[] SpawnPointList { get { return m_spawnPointList; } }

    private bool[] m_monsterKilled;
    private bool[] m_oasisVisited;
    
    public bool[] OasisVisited { get { return m_oasisVisited; } }
    public void VisitOasis(EOasisName _oasis) 
    {
        if (OasisVisited[(int)_oasis]) { return; } OasisVisited[(int)_oasis] = true;
        string name = GameManager.GetNPCData(new(ENPCType.OASIS, (int)_oasis)).NPCName;
        PlayManager.AddIngameAlarm($"{name} 방문 완료!");
    }

    public void MonsterKilled(EMonsterName _monster, EMonsterDeathType _type)           // 첫 킬, 퀘스트 확인
    {
        int idx = (int)_monster;
        if (!m_monsterKilled[idx]) { FirstKillMonster(_monster); }
        List<QuestInfo> infos = PlayManager.QuestInfoList;

        EQuestType questType = EQuestType.LAST;
        if (_type == EMonsterDeathType.BY_PLAYER) { questType = EQuestType.KILL; }
        else if (_type == EMonsterDeathType.PURIFY) { questType = EQuestType.PURIFY; }
        if (questType == EQuestType.LAST) { return; }

        foreach (QuestInfo quest in infos)
        {
            if (quest.State != EQuestState.ACCEPTED || quest.QuestContent.Type != questType
                || quest.QuestContent.Monster != _monster) { continue; }

            float prog = quest.QuestProgress + 1;
            PlayManager.SetQuestProgress(quest.QuestName, prog);
        }
    }
    private void FirstKillMonster(EMonsterName _monster)
    {
        MonsterScriptable data = GameManager.GetMonsterData(_monster);
        int point = data.FirstKillStat;

        PlayManager.AddStatPoint(point);
        PlayManager.AddIngameAlarm($"{data.MonsterName} 최초 처치로 {point} 능력치 점수 획득");

        m_monsterKilled[(int)_monster] = true;
    }


    public void LoadData()
    {
        GameManager.RegisterData(this);
        m_monsterKilled = new bool[(int)EMonsterName.LAST];
        m_oasisVisited = new bool[(int)EOasisName.LAST];
        if (PlayManager.IsNewData) { return; }

        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i < (int)EMonsterName.LAST; i++)
        {
            m_monsterKilled[i] = data.MonsterKilled[i];
        }
        for (int i = 0; i<(int)EOasisName.LAST; i++)
        {
            m_oasisVisited[i] = data.OasisVisited[i];
        }
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        for (int i = 0; i < (int)EMonsterName.LAST; i++)
        {
            data.MonsterKilled[i] = m_monsterKilled[i];
        }
        for (int i = 0; i<(int)EOasisName.LAST; i++)
        {
            data.OasisVisited[i] = m_oasisVisited[i];
        }
    }

    private readonly int[] NPCNum = new int[(int)ENPCType.LAST] { (int)EOasisName.LAST, (int)EAltarName.LAST, (int)ESlateName.LAST, 0 };



    public static ERegion Oasis2Region(EOasisName _oasis)
    {
        if(_oasis <= EOasisName.START2) { return ERegion.START; }
        else if(_oasis <= EOasisName.LIFE2) { return ERegion.LIFE; }
        else if(_oasis <= EOasisName.GOD) { return ERegion.GOD; }
        else if(_oasis <= EOasisName.WATER2) { return ERegion.WATER; }
        else if(_oasis <= EOasisName.CRYSTAL2) { return ERegion.CRYSTAL; }
        return ERegion.LAST;
    }

    public static string Region2String(ERegion _region)
    {
        return _region switch
        {
            ERegion.START => "저승 초입",
            ERegion.LIFE => "스펜탐 요그누의 영역",
            ERegion.GOD => "힘을 잃은 자의 영역",
            ERegion.WATER => "하지르바타트의 영역",
            ERegion.CRYSTAL => "오샤트라 바이르만의 영역",

            _ => ""
        };
    }


    public void TempSetNPCs(NPCScript[] _npcs)
    {
        int[] nums = new int[(int)ENPCType.LAST];

        foreach (NPCScript npc in _npcs)
        {
            ENPCType type = npc.NPC.Type;
            int idx = (int)type;
            m_npcList[idx, nums[idx]++] = npc;
        }
    }

    public void SetManager()
    {
        m_npcList = new NPCScript[(int)ENPCType.LAST, ValueDefine.MAX_NPC_NUM];
        m_monsterKilled = new bool[(int)EMonsterName.LAST];

        if (!GameManager.IsInGame) { return; }

        int[] cnt = new int[(int)ENPCType.LAST];
        NPCScript[] list = m_mapObject.GetComponentsInChildren<NPCScript>();
        for (int i = 0; i < list.Length; i++)
        {
            ENPCType type = list[i].NPC.Type;
            int idx = (int)type;
            if (cnt[idx] > NPCNum[idx]) { continue; }
            m_npcList[idx, cnt[idx]++] = list[i];
        }

        LoadData();
    }
}
