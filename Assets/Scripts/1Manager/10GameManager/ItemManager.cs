using System;
using System.Collections.Generic;
using UnityEngine;

// 아이템 관련 enum -> ItemEnum에 있음

[Serializable]
public struct SItem             // 아이템 구조체
{
    public EItemType Type;
    public int Idx;

    public readonly bool IsEmpty { get { return Idx == -1; } }
    public static SItem Empty { get { return new SItem(EItemType.LAST, -1); } }
    public SItem(EItemType _type, int _idx) { Type = _type; Idx = _idx; }
    public static bool operator==(SItem _item1, SItem _item2) { return (_item1.Type == _item2.Type && _item1.Idx == _item2.Idx); }
    public static bool operator!=(SItem _item1, SItem _item2) { return !(_item1 == _item2); }
    public readonly override bool Equals(object obj) { return obj is SItem item && Type == item.Type && Idx == item.Idx; }
    public readonly override int GetHashCode() { return HashCode.Combine(Type, Idx); }
}


[Serializable]
public struct DropInfo
{
    public List<SDropItem> Items;
    public int StatPoint;
    public int Soul;
    public int Purified;
    public DropInfo(List<SDropItem> _items, int _stat, int _soul, int _purified) 
    {
        Items = new();
        foreach (SDropItem item in _items) { Items.Add(item); }
        StatPoint = _stat; Soul = _soul; Purified = _purified;
    }
}

[Serializable]
public struct SDropItem         // 드랍 아이템 구조체
{
    public string ID;
    public float Prob;
    public SItem Item { get { return ItemManager.ID2Item(ID); } }
    public SDropItem(string _id, float _prob) { ID = _id; Prob = _prob; }
}

public class ItemInfo
{
    public ItemScriptable ItemData { get; private set; }
    public string ItemID { get { return ItemData.ID; } }                    // 공통 필드
    public string ItemName { get { return ItemData.ItemName; } }
    public EItemType ItemType { get; private set; }
    public string ItemDescription { get { return ItemData.Description; } }
    public SItem Item { get; private set; }
    public bool Obtained { get { if (ItemType != EItemType.WEAPON) { return true; } return PlayManager.WeaponObtained[Item.Idx]; } }

    public ItemInfo(ItemScriptable _scriptable, SItem _item)
    {
        ItemData = _scriptable;
        Item = _item;
        ItemType = _item.Type;
    }
}

public class ItemManager : MonoBehaviour
{
    // 아이템 정보
    private readonly Dictionary<SItem, ItemInfo> m_itemInfo = new();
    public ItemInfo GetItemInfo(SItem _item)
    {
        if (_item.IsEmpty) { Debug.LogError("빈 아이템"); return null; }
        return m_itemInfo[_item];
    }


    [SerializeField]
    private WeaponScriptable[] m_weaponData;
    [SerializeField]
    private PatternScriptable[] m_patternData;
    [SerializeField]
    private ThrowItemScriptable[] m_throwItemData;
    [SerializeField]
    private OtherItemScriptable[] m_othersData;

    public void SetItemData(List<ItemScriptable>[] _datas)
    {
        m_weaponData = new WeaponScriptable[_datas[0].Count];
        for(int i = 0; i<m_weaponData.Length; i++) { m_weaponData[i] = (WeaponScriptable)_datas[0][i]; }
        m_patternData = new PatternScriptable[_datas[1].Count];
        for (int i = 0; i<m_patternData.Length; i++) { m_patternData[i] = (PatternScriptable)_datas[1][i]; }
        m_throwItemData = new ThrowItemScriptable[_datas[2].Count];
        for (int i = 0; i<m_throwItemData.Length; i++) { m_throwItemData[i] = (ThrowItemScriptable)_datas[2][i]; }
        m_othersData = new OtherItemScriptable[_datas[3].Count];
        for (int i = 0; i<m_othersData.Length; i++) { m_othersData[i] = (OtherItemScriptable)_datas[3][i]; }
    }

    public ItemScriptable GetItemData(SItem _item)
    {
        int idx = _item.Idx;
        return _item.Type switch
        {
            EItemType.WEAPON => m_weaponData[idx],
            EItemType.PATTERN => m_patternData[idx],
            EItemType.THROW => m_throwItemData[idx],
            EItemType.OTHERS => m_othersData[idx],
            _ => null
        };
    }


    [SerializeField]
    private GameObject[] m_throwItemPrefabs = new GameObject[(int)EThrowItemName.LAST];     // 투척 아이템
    public GameObject GetThrowItemPrefab(EThrowItemName _item)
    {
        return PoolManager.GetObject(m_throwItemData[(int)_item].ItemPrefab);
    }


    // 아이템 프리펍
    [SerializeField]
    private GameObject[] m_dropItemPrefabs = new GameObject[(int)EItemType.LAST];            // 드랍 아이템
    public GameObject GetDropItemPrefab(EItemType _item)
    {
        return PoolManager.GetObject(m_dropItemPrefabs[(int)_item]);
    }


    public GameObject[] ItemArray { get {                                                   // 전체 아이템
            List<GameObject> list = new();
            list.AddRange(m_dropItemPrefabs);
            list.AddRange(m_throwItemPrefabs);
            return list.ToArray(); } }


    public static EItemType IDToItemType(string _id)
    {
        return _id[0] switch
        {
            ValueDefine.WEAPON_CODE => EItemType.WEAPON,
            ValueDefine.PATTERN_CODE => EItemType.PATTERN,
            ValueDefine.THROW_ITEM_CODE => EItemType.THROW,
            ValueDefine.OTHER_ITEM_CODE => EItemType.OTHERS,
            _ => EItemType.LAST
        };
    }
    public static EWeaponType IDToWeaponType(string _id)
    {
        return _id[1] switch
        {
            ValueDefine.BLADE_CODE => EWeaponType.BLADE,
            ValueDefine.SWORD_CODE => EWeaponType.SWORD,
            ValueDefine.SCEPTER_CODE => EWeaponType.SCEPTER,
            _ => EWeaponType.LAST
        };
    }
    public static SItem ID2Item(string _id)
    {
        if (_id == "") { return SItem.Empty; }
        EItemType type = IDToItemType(_id);
        int.TryParse(_id[1..], out int code);
        int idx = ID2Idx(type, code);
        return idx != -1 ? new SItem(type, idx) : SItem.Empty;
    }

    private static int ID2Idx(EItemType _type, int _code)
    {
        int sub = _code / 100;
        int idx = _code % 100 - 1;
        switch (_type)
        {
            case EItemType.WEAPON:
                if(sub == 1) { return idx; }
                else if(sub == 2) { return idx + (int)EWeaponName.BASIC_SWORD; }
                else if(sub== 3) { return idx + (int)EWeaponName.BASIC_SCEPTER; }
                break;
            case EItemType.PATTERN:
                if(sub == 1) { return idx; }
                else if(sub == 2) { return idx + (int)EPatternName.WATER; }
                else if(sub == 3) { return idx + (int)EPatternName.GROUND; }
                break;
            case EItemType.THROW:
                if(sub == 1) { return idx; }
                else if(sub == 2) { return idx + (int)EThrowItemName.BOMB; }
                else if(sub == 3) { return idx + (int)EThrowItemName.SLOW; }
                else if (sub == 4) { return idx + (int)EThrowItemName.ENDER; }
                else if (sub == 5) { return idx + (int)EThrowItemName.PURIFY; }
                break;
            case EItemType.OTHERS:
                return MonsterManager.Code2Idx(sub, idx);
        }
        return -1;
    }


    // 아이템 별 종류 수
    private readonly uint[] ItemCounts = new uint[(int)EItemType.LAST]
    { (uint)EWeaponName.LAST, (uint)EPatternName.LAST, (uint)EThrowItemName.LAST, (uint)EOtherItemName.LAST };

    public void SetManager()
    {
        for (int i = 0; i<(int)EItemType.LAST; i++)
        {
            EItemType type = (EItemType)i;
            uint cnt = ItemCounts[i];
            for (int j = 0; j < cnt; j++)
            {
                SItem item = new(type, j);
                ItemScriptable scriptable = GameManager.GetItemData(item);
                m_itemInfo[item] = new(scriptable, item);
            }
        }
    }
}
