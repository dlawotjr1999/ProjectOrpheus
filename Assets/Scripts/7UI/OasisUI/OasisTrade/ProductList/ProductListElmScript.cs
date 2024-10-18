using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductListElmScript : MonoBehaviour
{
    private ProductListScript m_parent;
    public void SetParent(ProductListScript _parent) { m_parent = _parent; }

    private Image m_productIcon;
    private TextMeshProUGUI m_productName;
    private TextMeshProUGUI m_productDesc;
    private TextMeshProUGUI m_productPrice;
    private Button m_controlBtn;
    private TextMeshProUGUI m_controlTxt;


    private int CurIdx { get; set; }
    private bool CanBuy { get; set; }

    public void SetElm(int _idx, EThrowItemName _item, bool _sold)
    {
        CurIdx = _idx;

        SItem item = new(EItemType.THROW, (int)_item);
        ItemScriptable data = GameManager.GetItemData(item);
        Sprite icon = data.ItemImage;
        string name = data.ItemName;
        string desc = data.Description;
        int price = data.ItemPrice;
        SetElm(icon, name, desc, price, false, _sold);
    }
    public void SetElm(int _idx, EPowerName _power, bool _sold)
    {
        CurIdx = _idx;

        PowerScriptable data = GameManager.GetPowerData(_power);
        Sprite icon = data.PowerIcon;
        string name = data.PowerName;
        string desc = data.Description;
        int price = data.PowerPrice;
        bool bought = PlayManager.PowerObtained[(int)_power];
        SetElm(icon, name, desc, price, bought, _sold);
    }

    private void SetElm(Sprite _icon, string _name, string _desc, int _price, bool _bought, bool _sold)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
        m_productIcon.sprite = _icon;
        m_productName.text = _name;
        m_productDesc.text = _desc;
        m_productPrice.enabled = !_sold;

        bool hasSoul = _price <= PlayManager.SoulNum;
        if (!_sold)
        {
            m_productPrice.text = $"영혼 {_price}개";
            m_productPrice.color = hasSoul ? Color.black : Color.red;
        }
        m_controlTxt.text = _sold ? "구매함" : _bought ? "보유중" : "구매";

        CanBuy = !_sold && hasSoul;
        m_controlBtn.interactable = CanBuy;
    }

    private void BuyProduct()
    {
        m_parent.BuyProduct(CurIdx);
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    private void SetBtns()
    {
        m_controlBtn.onClick.AddListener(BuyProduct);
    }

    public void SetComps()
    {
        m_productIcon = GetComponentsInChildren<Image>()[1];
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_productName = txts[0];
        m_productDesc = txts[1];
        m_productPrice = txts[2];
        m_controlTxt = txts[3];
        m_controlBtn = GetComponentInChildren<Button>();
        SetBtns();
    }
}
