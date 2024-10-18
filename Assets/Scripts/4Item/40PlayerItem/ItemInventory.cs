using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryElm       // �� �κ��丮 ĭ�� ����
{
    public SItem Item;                                                            // ������ �̸� idx
    public int Num;                                                               // ������ ����
    public bool IsEmpty { get { return Item.IsEmpty; } }                          // �������
    public void SetItem(SItem _item, int _num) { Item = _item; Num = _num; }    // �κ��丮 ����
    public void SetItem(InventoryElm _other) { Item = _other.Item; Num = _other.Num; }
    public void Additem(int _num) { Num += _num; }
    public void UseItem(int _num) { if(Num >= _num) { Num -= _num; } }          // ������ ���
    public void EmptyInventory() { Item = SItem.Empty; Num = 0; }               // ����
    public InventoryElm() { EmptyInventory(); }                                     // �� ������
    public InventoryElm(InventoryElm _other) { SetItem(_other); }
}

public class ItemInventory
{
    private readonly InventoryElm[] m_inventory = new InventoryElm[ValueDefine.MAX_INVENTORY];        // �κ��丮 �迭
    public InventoryElm[] Inventory { get { return m_inventory; } }

    public int EmptyIdx { get { for (int i = 0; i<ValueDefine.MAX_INVENTORY; i++) { if (m_inventory[i].IsEmpty) return i; } return -1; } }     // �� �κ��丮 idx


    public void AddItem(SItem _item, int _num)                  // �� �κ��丮�� ������ �߰�
    {
        if(EmptyIdx == -1) { Debug.LogError("�� �κ��丮 ����"); return; }
        for(int i=0;i<m_inventory.Length;i++) { if (m_inventory[i].Item == _item) { m_inventory[i].Additem(_num); return; } }
        SetItem(EmptyIdx, _item, _num);
    }
    public void SetItem(int _idx, SItem _item, int _num)        // Idx��° �κ��丮�� ������ �Ҵ�
    {
        m_inventory[_idx].SetItem(_item, _num);
    }
    public void SwapItem(int _idx1, int _idx2)
    {
        if (_idx2 >= m_inventory.Length || _idx1 >= m_inventory.Length) { return; }
        InventoryElm item1 = m_inventory[_idx1], item2 = m_inventory[_idx2];
        m_inventory[_idx2] = item1; m_inventory[_idx1] = item2;
    }
    public bool ChkNUseItem(SItem _item, int _num)              // ������ Ȯ�� �� ���
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
    public void RemoveItem(int _idx)                            // Idx��° �κ��丮 ������ ����
    {
        m_inventory[_idx].EmptyInventory();
    }


    public ItemInventory() { for(int i=0;i<ValueDefine.MAX_INVENTORY; i++) { m_inventory[i] = new(); } }
    public ItemInventory(InventoryElm[] _data) { for(int i=0;i<ValueDefine.MAX_INVENTORY; i++) { m_inventory[i] = new(_data[i]); } }
}
