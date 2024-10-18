using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IHaveData
{
    // 영혼
    private int m_soulNum = 0;
    public int SoulNum { get { return m_soulNum; } }
    public void AddSoul(int _num)
    {
        m_soulNum += _num;
        PlayManager.AddIngameAlarm($"영혼 {_num}개 획득!");
        PlayManager.UpdateMaterials();
    }
    public void LooseSoul(int _num) { LooseSoul(_num, false); }
    public void LooseSoul(int _num, bool _absorbed)
    {
        if (m_soulNum < _num) { Debug.LogError("보유 영혼보다 많은 개수 사용"); return; }
        m_soulNum -= _num;
        if (_absorbed)
        {
            PlayManager.AddIngameAlarm($"영혼 {_num}개 흡수당함.");
        }
        PlayManager.UpdateMaterials();
    }


    // 성불 영혼
    private int m_purifiedNum = 0;
    public int PurifiedNum { get { return m_purifiedNum; } }
    public void AddPurified(int _num)
    {
        m_purifiedNum += _num;
        PlayManager.AddIngameAlarm($"성불 영혼 {_num}개 획득!");
        PlayManager.UpdateMaterials();
    }
    public void UsePurified(int _num)
    {
        if (m_purifiedNum < _num) { Debug.LogError("보유 성불 영혼보다 많은 개수 사용"); return; }
        m_purifiedNum -= _num;
        PlayManager.UpdateMaterials();
    }

    // 문양
    public int[] PatternNum { get {
            int[] patterns = new int[(int)EPatternName.LAST];
            for (int i = 0; i<(int)EPatternName.LAST; i++) {
                SItem item = new(EItemType.PATTERN, i);
                foreach (InventoryElm inven in m_itemInven.Inventory) {
                    if(inven.Item == item) { patterns[i] += inven.Num; } } }
            return patterns; } }


    // 회복 아이템 (각인 문양?)
    private readonly List<EPatternName> m_healPatternList = new();
    public EPatternName CurHealPattern { get { if (m_healPatternList.Count > 0) return m_healPatternList[0]; return EPatternName.LAST; } }
    public EPatternName[] HealPatternList { get { return m_healPatternList.ToArray(); } }
    public void UseHealItem()
    {
        if (m_healPatternList.Count == 0) { Debug.LogError("회복 아이템 없음"); return; }
        m_healPatternList.RemoveAt(0);
    }
    public void RegisterHealItem(EPatternName _pattern)
    {
        if (m_healPatternList.Count >= ValueDefine.MAX_HEAL_ITEM) { Debug.Log("리스트 꽉 참"); return; }

        if (ChkNUseItem(new(EItemType.PATTERN, (int)_pattern), 1))
        {
            m_healPatternList.Add(_pattern);
        }
    }


    // 장비 인벤토리
    private readonly bool[] m_weaponObtained = new bool[(int)EWeaponName.LAST];
    public bool[] WeaponObatined { get { return m_weaponObtained; } }
    public EWeaponName CurWeapon { get; private set; } = EWeaponName.BASIC_BLADE;    // 장착 중인 무기

    public readonly static EWeaponName InitialWeapon = EWeaponName.BASIC_SCEPTER;

    private void InitWeaponObtained() { for(int i = 0; i<=(int)InitialWeapon; i++) { m_weaponObtained[i] = true; } }
    public void ObtainWeapon(EWeaponName _weapon)
    {
        m_weaponObtained[(int)_weapon] = true;
    }
    public void SetCurWeapon(EWeaponName _weapon)               // 무기 설정
    {
        CurWeapon = _weapon;
    }
    public void EquipWeapon(EWeaponName _weapon)                // 무기 장착
    {
        if (!WeaponObatined[(int)_weapon]) { Debug.LogError("무기 미습득"); return; }
        SetCurWeapon(_weapon);
        PlayManager.SetPlayerWeapon(_weapon);
    }


    // 투척 아이템
    private readonly List<EThrowItemName> m_throwItemList = new();
    public EThrowItemName CurThrowItem { get { if (m_throwItemList.Count > 0) return m_throwItemList[0]; return EThrowItemName.LAST; } }    // 현재 던지기 아이템
    public List<EThrowItemName> ThrowItemList { get { return m_throwItemList; } }       // 추가된 던지기 아이템들
    public void UseThrowItem()                                                          // 던지기 아이템 사용
    {
        if (m_throwItemList.Count == 0) { Debug.LogError("던지기 아이템 없음"); return; }
        m_throwItemList.RemoveAt(0);
    }
    public void AddThrowItem(EThrowItemName _item)                                      // 인벤에서 던지기 아이템 추가
    {
        if (m_throwItemList.Count == ValueDefine.MAX_THROW_ITEM) { Debug.Log("리스트 꽉 참"); return; }

        if (ChkNUseItem(new(EItemType.THROW, (int)_item), 1))
        {
            m_throwItemList.Add(_item);
        }
    }
    public void SetThrowItem(int _idx, EThrowItemName _item)
    {
        if(_idx >= m_throwItemList.Count) { AddThrowItem(_item); return; }
        m_throwItemList.Insert(_idx, _item);
    }
    public void SwapThrowItem(int _idx1, int _idx2)
    {
        if(_idx2 >= m_throwItemList.Count || _idx1 >= m_throwItemList.Count) { return; }
        EThrowItemName item1 = m_throwItemList[_idx1], item2 = m_throwItemList[_idx2];
        m_throwItemList[_idx2] = item1; m_throwItemList[_idx1] = item2;
    }
    public void RemoveThrowItem(int _idx)
    {
        if(_idx >= m_throwItemList.Count) { return; }
        EThrowItemName item = m_throwItemList[_idx];
        m_throwItemList.RemoveAt(_idx);
        AddInventoryItem(new(EItemType.THROW, (int)item), 1);
    }



    // 아이템 인벤토리
    private ItemInventory m_itemInven;                                                  // 아이템 인벤토리
    public InventoryElm[] Inventory { get { return m_itemInven.Inventory; } }           // 아이템 목록
    public void AddInventoryItem(SItem _item, int _num) { AddInventoryItem(_item, _num, false); }                       // 아이템 추가
    public void AddInventoryItem(SItem _item, int _num, bool _isNew)            
    {
        m_itemInven.AddItem(_item, _num);
        if (_isNew) { CheckItemObtained(_item, _num); }
        string itemName = GameManager.GetItemData(_item).ItemName;
        PlayManager.AddIngameAlarm($"{itemName} {_num}개 획득!");
    }
    public void SetInventoryItem(int _idx, SItem _item, int _num) { m_itemInven.SetItem(_idx, _item, _num); }           // idx에 아이템 설정
    public void SwapItemInven(int _idx1, int _idx2) { m_itemInven.SwapItem(_idx1, _idx2); }
    public bool ChkNUseItem(SItem _item, int _num) { return m_itemInven.ChkNUseItem(_item, _num); }                     // 아이템 확인 후 사용
    public void RemoveInventoryItem(int _idx) { m_itemInven.RemoveItem(_idx); }                                         // 아이템 제거

    private void CheckItemObtained(SItem _item, int _num)                   // 아이템 획득 시 퀘스트 확인
    {
        List<QuestInfo> infos = PlayManager.QuestInfoList;

        EQuestType questType = EQuestType.COLLECT;
        if (questType == EQuestType.LAST) { return; }

        foreach (QuestInfo quest in infos)
        {
            if (quest.State != EQuestState.ACCEPTED || quest.QuestContent.Type != questType
                || quest.QuestContent.Item != _item) { continue; }

            float target = quest.QuestContent.Amount;
            float prog = quest.QuestProgress + _num;
            if(prog > target) { prog = target; }
            PlayManager.SetQuestProgress(quest.QuestName, prog);
        }
    }



    public void LoadData()
    {
        GameManager.RegisterData(this);
        if (PlayManager.IsNewData) { m_itemInven = new(); InitWeaponObtained(); return; }

        SaveData data = PlayManager.CurSaveData;

        m_soulNum = data.Soul;
        m_purifiedNum = data.PurifiedSoul;
        foreach(EPatternName pattern in data.HealPatternList) { m_healPatternList.Add(pattern); }
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { m_weaponObtained[i] = data.WeaponObtained[i]; }
        CurWeapon = data.CurWeapon;
        foreach(EThrowItemName throwItem in data.ThrowItemList) { m_throwItemList.Add(throwItem); }
        m_itemInven = new(data.Inventory);
    }
    public void SaveData()
    {
        SaveData data = PlayManager.CurSaveData;

        data.Soul = m_soulNum;
        data.PurifiedSoul = m_purifiedNum;
        foreach (EPatternName pattern in m_healPatternList) { data.HealPatternList.Add(pattern); }
        for(int i = 0; i<(int)EWeaponName.LAST; i++) { data.WeaponObtained[i] = m_weaponObtained[i]; }
        data.CurWeapon = CurWeapon;
        foreach(EThrowItemName throwItem in m_throwItemList) { data.ThrowItemList.Add(throwItem); }
        for (int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { data.Inventory[i].SetItem(Inventory[i]); }
    }



    public void SetManager()
    {
        LoadData();
    }
}
