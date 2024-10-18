using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoxSlotElmScript : MonoBehaviour
{
    [SerializeField]
    private Image m_powerImg;


    public void SetPower(EPowerName _skill)
    {
        if(_skill == EPowerName.LAST) { m_powerImg.gameObject.SetActive(false); return; }
        else if(!m_powerImg.gameObject.activeSelf) { m_powerImg.gameObject.SetActive(true); }
        m_powerImg.sprite = GameManager.GetPowerSprite(_skill);
    }
}
