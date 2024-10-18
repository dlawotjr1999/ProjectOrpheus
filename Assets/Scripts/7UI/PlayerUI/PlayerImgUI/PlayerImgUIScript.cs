using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImgUIScript : MonoBehaviour
{
    private PlayerUIScript m_parent;
    public void SetParent(PlayerUIScript _parent) { m_parent = _parent; }


    private PlayerImgWeaponSlot m_weaponSlot;

    private PlayerImgPatternSlot m_healSlot;

    private SItem CurWeapon { get; set; }

    public void OpenUI()
    {
        UpdatePlayerWeapon(PlayManager.CurWeapon);
        UpdateUI();
    }

    public void UpdateUI()
    {
        m_healSlot.UpdateUI();
    }
    public void UpdatePlayerWeapon(EWeaponName _weapon)
    {
        CurWeapon = new(EItemType.WEAPON, (int)_weapon);
        Sprite img = GameManager.GetItemSprite(CurWeapon);
        m_weaponSlot.SetImage(img);
    }

    public void ShowItemInfo()
    {
        m_parent.ShowItemInfoUI(CurWeapon);
    }
    public void ShowItemInfo(EPatternName _pattern)
    {
        m_parent.ShowItemInfoUI(new(EItemType.PATTERN, (int)_pattern));
    }
    public void HideItemInfo()
    {
        m_parent.HideItemInfoUI();
    }
    public void SetItemInfoPos(Vector2 _pos)
    {
        m_parent.SetItemInfoUIPos(_pos);
    }


    public void SetComps()
    {
        m_weaponSlot = GetComponentInChildren<PlayerImgWeaponSlot>();
        m_weaponSlot.SetParent(this);
        m_weaponSlot.SetComps();
        m_healSlot = GetComponentInChildren<PlayerImgPatternSlot>();
        m_healSlot.SetParent(this);
        m_healSlot.SetComps();
    }
}
