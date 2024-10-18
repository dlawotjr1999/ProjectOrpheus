using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DropItemScript : PooledItem, IInteractable
{
    [SerializeField]
    private List<SItem> m_dropItems;
    [SerializeField]
    private int m_itemNum;

    public InteractScript InteractManager { get; private set; }
    public void SetInteractScript(InteractScript _interact) {  InteractManager = _interact; }

    public bool CanInteract => true;

    public List<SItem> DropItem { get { return m_dropItems; } }

    public void SetDropItems(List<SItem> _items)
    {
        m_dropItems = new();
        foreach(SItem item in _items) { m_dropItems.Add(item); }
    }

    // ------------- 인터페이스 구현 ------------------ //
    public EInteractType InteractType { get { return EInteractType.ITEM; } }


    public virtual string InfoTxt { get { return "획득"; } }

    public virtual void StartInteract()
    {
        GetItem();
        PlayManager.StopPlayerInteract();
    }

    public virtual void StopInteract()
    {
        PlayManager.StopPlayerInteract();
    }


    public override void ReleaseToPool()
    {
        m_dropItems.Clear();
        base.ReleaseToPool();
    }


    public void GetItem()
    {
        foreach (SItem item in m_dropItems)
        {
            PlayManager.AddInventoryItem(item, 1, true);
        }

        GameManager.PlaySE(EPlayerSE.ITEM_GET, transform.position);
        ReleaseToPool();
    }
}
