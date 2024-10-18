using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductListScript : MonoBehaviour
{
    private OasisTradeUIScript m_parent;
    public void SetParent(OasisTradeUIScript _parent) { m_parent = _parent; }

    [SerializeField]
    private RectTransform m_boxTransform;

    private ProductListElmScript[] m_elms;

    private readonly int MaxProduct = 8;
    private readonly float ElmWidth = 872;
    private readonly float ElmHeight = 128;
    private readonly float ElmGap = 16;


    private OasisNPC CurOasis { get; set; }


    public void UpdateUI(OasisNPC _oasis)
    {
        CurOasis = _oasis;
        List<EThrowItemName> item = _oasis.ItemList;
        List<EPowerName> power = _oasis.PowerList;
        bool[] sold = _oasis.ProducSold;
        int itemNum = item.Count;
        int len = _oasis.ProductCount;
        if(len > MaxProduct) { Debug.LogError($"{_oasis.NPCName} 구매 목록 너무 많음"); return; }
        for (int i = 0; i<MaxProduct; i++)
        {
            if (i >= len) { m_elms[i].HideElm(); continue; }
            if (i< itemNum) { m_elms[i].SetElm(i, item[i], sold[i]); }
            else { m_elms[i].SetElm(i, power[i - itemNum], sold[i]); }
        }
        SetBoxHeight(len);
    }
    private void SetBoxHeight(int _num)
    {
        float height = ElmHeight * _num + ElmGap * (_num-1);
        m_boxTransform.sizeDelta = new(ElmWidth, height);
    }

    public void BuyProduct(int _idx)
    {
        CurOasis.BuyProduct(_idx);
        m_parent.UpdateUI();
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<ProductListElmScript>();
        if(m_elms.Length != MaxProduct) { Debug.LogError("진열대 개수 다름"); return; }
        foreach(ProductListElmScript elm in m_elms) { elm.SetParent(this); elm.SetComps(); }
    }
}
