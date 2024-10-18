using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryElm       // 각 인벤토리 칸의 정보
{
    public SItem Item;                                                            // 아이템 이름 idx
    public int Num;                                                               // 아이템 개수
    public bool IsEmpty { get { return Item.IsEmpty; } }                          // 비었는지
    public void SetItem(SItem _item, int _num) { Item = _item; Num = _num; }    // 인벤토리 설정
    public void SetItem(InventoryElm _other) { Item = _other.Item; Num = _other.Num; }
    public void Additem(int _num) { Num += _num; }
    public void UseItem(int _num) { if(Num >= _num) { Num -= _num; } }          // 아이템 사용
    public void EmptyInventory() { Item = SItem.Empty; Num = 0; }               // 비우기
    public InventoryElm() { EmptyInventory(); }                                     // 빈 생성자
    public InventoryElm(InventoryElm _other) { SetItem(_other); }
}

public class ItemInventory
{
    private readonly InventoryElm[] m_inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];        // 인벤토리 배열
    public InventoryElm[] Inventory { get { return m_inventory; } }

    public int EmptyIdx { get { for (int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { if (m_inventory[i].IsEmpty) return i; } return -1; } }     // 빈 인벤토리 idx


    public void AddItem(SItem _item, int _num)                  // 빈 인벤토리에 아이템 추가
    {
        if(EmptyIdx == -1) { Debug.LogError("빈 인벤토리 없음"); return; }
        for(int i=0;i<m_inventory.Length;i++) { if (m_inventory[i].Item == _item) { m_inventory[i].Additem(_num); return; } }
        SetItem(EmptyIdx, _item, _num);
    }
    public void SetItem(int _idx, SItem _item, int _num)        // Idx번째 인벤토리에 아이템 할당
    {
        m_inventory[_idx].SetItem(_item, _num);
    }
    public void SwapItem(int _idx1, int _idx2)
    {
        if (_idx2 >= m_inventory.Length || _idx1 >= m_inventory.Length) { return; }
        InventoryElm item1 = m_inventory[_idx1], item2 = m_inventory[_idx2];
        m_inventory[_idx2] = item1; m_inventory[_idx1] = item2;
    }
    public bool ChkNUseItem(SItem _item, int _num)              // 아이템 확인 후 사용
    {
        for (int i = 0; i<Inventory.Length; i++)
        {
            if (_item == Inventory[i].Item)
            {
                if (Inventory[i].Num >= _num)
                {
                    Inventory[i].UseItem(_num);
                    return true;
                }
                return false;
            }
        }
        return false;
    }
    public void RemoveItem(int _idx)                            // Idx번째 인벤토리 아이템 제거
    {
        m_inventory[_idx].EmptyInventory();
    }


    public ItemInventory() { for(int i=0;i<ValueDefine.MAX_INVENTORY; i++) { m_inventory[i] = new(); } }
    public ItemInventory(InventoryElm[] _data) { for(int i=0;i<ValueDefine.MAX_INVENTORY; i++) { m_inventory[i] = new(_data[i]); } }
}
