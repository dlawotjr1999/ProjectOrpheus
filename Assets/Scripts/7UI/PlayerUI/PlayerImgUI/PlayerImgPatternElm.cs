using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImgPatternElm : DragMouseOverInfoUI
{
    private PlayerImgPatternSlot m_parent;
    public void SetParent(PlayerImgPatternSlot _parent) { m_parent = _parent; }

    private Image m_patternImg;


    public EPatternName CurPattern { get; private set; }
    private bool HasPattern { get { return CurPattern != EPatternName.LAST; } }
    
    
    
    public void SetPattern(EPatternName _pattern)
    {
        CurPattern = _pattern;
        if(_pattern == EPatternName.LAST) { m_patternImg.gameObject.SetActive(false); return; }
        else if (!m_patternImg.gameObject.activeSelf) { m_patternImg.gameObject.SetActive(true); }
        Sprite img = GameManager.GetItemSprite(new(EItemType.PATTERN, (int)_pattern));
        m_patternImg.sprite = img;
    }


    public override void ShowInfo()
    {
        if(!HasPattern) { return; }
        m_parent.ShowInfo(CurPattern);
    }
    public override void HideInfo()
    {
        if (!HasPattern) { return; }
        m_parent.HideInfo();
    }
    public override void SetInfoPos(Vector2 _pos)
    {
        if (!HasPattern) { return; }
        m_parent.SetInfoPos(_pos);
    }


    public override void SetComps()
    {
        base.SetComps();
        m_patternImg = GetComponentsInChildren<Image>()[1];
    }
}
