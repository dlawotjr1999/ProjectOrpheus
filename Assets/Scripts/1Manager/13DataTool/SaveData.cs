using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // 기본 정보
    public string SavedTime;
    public EOasisName OasisPoint;

    // 플레이어 정보
    public Vector3 PlayerRot;
    public PlayerStatInfo StatInfo;
    public int LeftStatPoint;
    public int UsedStatPoint;

    // 아이템 정보
    public int Soul;
    public int PurifiedSoul;
    public List<EPatternName> HealPatternList;
    public bool[] WeaponObtained;
    public EWeaponName CurWeapon;
    public List<EThrowItemName> ThrowItemList;
    public InventoryElm[] Inventory;

    // 스킬 정보
    public EPowerName[] PowerSlot;
    public bool[] PowerObtained;


    // 스토리 정보
    public List<NPCSaveData> NPCData;
    public QuestInfo[] QuestInfos;
    public bool[] OasisVisited;


    // 몬스터 정보
    public bool[] MonsterKilled;
    public List<MonsterSaveData> MonsterData;

    public SaveData()
    {
        SavedTime = DateTime.Now.ToString();
        OasisPoint = EOasisName.LAST;

        PlayerRot = new(0, 180, 0);
        StatInfo = new();
        PowerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT] { EPowerName.LAST,EPowerName.LAST,EPowerName.LAST};
        PowerObtained = new bool[(int)EPowerName.LAST];

        HealPatternList = new();
        WeaponObtained = new bool[(int)EWeaponName.LAST];
        for(int i=0; i<=(int)InventoryManager.InitialWeapon; i++) { WeaponObtained[i] = true; }
        ThrowItemList = new();
        Inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];
        for(int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { Inventory[i] = new(); }

        NPCData = new();
        QuestInfos = new QuestInfo[(int)EQuestName.LAST];
        for(int i = 0; i<(int)EQuestName.LAST; i++) { QuestInfos[i] = new((EQuestName)i); }
        OasisVisited = new bool[(int)EOasisName.LAST];

        MonsterKilled = new bool[(int)EMonsterName.LAST];
        MonsterData = new();
    }
    public SaveData(SaveData _other)
    {
        SavedTime = DateTime.Now.ToString();
        OasisPoint = _other.OasisPoint;

        PlayerRot = _other.PlayerRot;
        StatInfo = new(_other.StatInfo);
        LeftStatPoint = _other.LeftStatPoint;
        UsedStatPoint = _other.UsedStatPoint;
        PowerSlot = new EPowerName[ValueDefine.MAX_POWER_SLOT];
        for(int i = 0; i<ValueDefine.MAX_POWER_SLOT; i++) { PowerSlot[i] = _other.PowerSlot[i]; }
        PowerObtained = new bool[(int)EPowerName.LAST];
        for(int i = 0; i<(int)EPowerName.LAST; i++) { PowerObtained[i] = _other.PowerObtained[i]; }

        Soul = _other.Soul;
        PurifiedSoul = _other.PurifiedSoul;
        HealPatternList = new();
        foreach(EPatternName pattern in _other.HealPatternList) { HealPatternList.Add(pattern); }
        WeaponObtained = new bool[(int)EWeaponName.LAST];
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { WeaponObtained[i] = _other.WeaponObtained[i]; }
        CurWeapon = _other.CurWeapon;
        ThrowItemList = new();
        foreach(EThrowItemName item in _other.ThrowItemList) { ThrowItemList.Add(item); }
        Inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];
        for(int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { Inventory[i] = new(_other.Inventory[i]); }

        NPCData = new();
        foreach(NPCSaveData npc in _other.NPCData) { NPCData.Add(new(npc)); }
        QuestInfos = new QuestInfo[(int)EQuestName.LAST];
        for(int i = 0; i<(int)EQuestName.LAST; i++) { QuestInfos[i] = new(_other.QuestInfos[i]); }
        OasisVisited = new bool[(int)EOasisName.LAST];
        for(int i = 0; i<(int)EOasisName.LAST; i++) { OasisVisited[i] = _other.OasisVisited[i]; }

        MonsterKilled = new bool[(int)EMonsterName.LAST];
        for(int i = 0; i<(int)EMonsterName.LAST; i++) { MonsterKilled[i] = _other.MonsterKilled[i]; }
        MonsterData = new();
        foreach(MonsterSaveData monster in _other.MonsterData) { MonsterData.Add(monster); }
    }
}
