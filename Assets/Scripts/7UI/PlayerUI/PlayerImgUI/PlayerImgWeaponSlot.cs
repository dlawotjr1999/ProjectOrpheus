using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImgWeaponSlot : DragMouseOverInfoUI
{
    private PlayerImgUIScript m_parent;
    public void SetParent(PlayerImgUIScript _parent) { m_parent = _parent; }
    
    private Image m_weaponImg;

    public void SetImage(Sprite _img)
    {
        m_weaponImg.sprite = _img;
    }

    public override void ShowInfo()
    {
        m_parent.ShowItemInfo();
    }
    public override void HideInfo()
    {
        m_parent.HideItemInfo();
    }
    public override void SetInfoPos(Vector2 _pos)
    {
        m_parent.SetItemInfoPos(_pos);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_weaponImg = GetComponentsInChildren<Image>()[1];
    }
}
