using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OasisNPC : NPCScript
{
    [SerializeField]
    private EOasisName m_pointName;
    [SerializeField]
    private Transform m_respawnSpot;

    public EOasisName PointName { get { return m_pointName; } }
    public Vector3 RespawnPoint { get { return m_respawnSpot.position; } }

    private OasisScriptable OasisInfo { get { return (OasisScriptable)m_scriptable; } }

    public List<EThrowItemName> ItemList { get { return OasisInfo.BuyableThrowItems; } }
    public List<EPowerName> PowerList { get { return OasisInfo.BuyablePowers; } }

    private bool[] m_productSold;
    public bool[] ProducSold { get { return m_productSold; } }
    public int ItemCount { get { return ItemList.Count; } }
    public int ProductCount { get { return ItemList.Count + PowerList.Count; } }

    public void SetOasis(uint _idx, NPCScriptable _scriptable)
    {
        m_pointName = (EOasisName)_idx;
        SetScriptable(_scriptable);
    }

    public void BuyProduct(int _idx)
    {
        if (m_productSold[_idx]) { return; }

        if(_idx < ItemList.Count)
        {
            SItem item = new(EItemType.THROW, (int)ItemList[_idx]);
            PlayManager.AddInventoryItem(item, 1);
            int price = GameManager.GetItemData(item).ItemPrice;
            PlayManager.UseSoul(price);
        }
        else
        {
            EPowerName power = PowerList[_idx - ItemCount];
            PlayManager.ObtainPower(power);
            int price = GameManager.GetPowerData(power).PowerPrice;
            PlayManager.UseSoul(price);
        }

        m_productSold[_idx] = true;
    }


    public override void NPCInteraction()
    {
        PlayManager.OpenOasisUI(this);
        PlayManager.VisitOasis(PointName);
    }

    public override void ApplyLoadedData(NPCSaveData _save)
    {
        base.ApplyLoadedData(_save);
        if(ProductCount != _save.ProductSold.Length) { Debug.LogError("NPC 판매 세이브 정보 오류"); return; }
        for (int i = 0; i<ProductCount; i++)
        {
            m_productSold[i] = _save.ProductSold[i];
        }
    }
    public override NPCSaveData ModifiedData()
    {
        return new(NPC, m_dialInfos, m_productSold);
    }

    public override void InitNPCData()
    {
        base.InitNPCData();
        m_productSold = new bool[ProductCount];
    }
}
