using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlotUIScript : MonoBehaviour
{
    [SerializeField]
    private Image m_throwItemImg;
    [SerializeField]
    private Image m_healItemImg;


    public void UpdateThrowItemImg()
    {
        EThrowItemName item = PlayManager.CurThrowItem;
        if (item == EThrowItemName.LAST) { m_throwItemImg.gameObject.SetActive(false); return; }
        else if (!m_throwItemImg.gameObject.activeSelf) { m_throwItemImg.gameObject.SetActive(true); }
        m_throwItemImg.sprite = GameManager.GetItemSprite(new(EItemType.THROW, (int)item));
    }

    public void UpdateHealItemImg()
    {
        EPatternName pattern = PlayManager.CurHealPattern;
        if(pattern == EPatternName.LAST) { m_healItemImg.gameObject.SetActive(false); return; }
        else if (!m_healItemImg.gameObject.activeSelf) { m_healItemImg.gameObject.SetActive(true); }
        m_healItemImg.sprite = GameManager.GetItemSprite(new(EItemType.PATTERN, (int)pattern));
    }
}
