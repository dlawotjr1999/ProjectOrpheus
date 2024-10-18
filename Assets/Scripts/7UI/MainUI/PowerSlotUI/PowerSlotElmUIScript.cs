using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSlotElmUIScript : MonoBehaviour
{
    private Image m_powerImg;
    private Image m_cooltimeImg;



    public void SetPower(EPowerName _power)
    {
        Sprite powerImg = _power != EPowerName.LAST ? GameManager.GetPowerSprite(_power) : null;
        m_powerImg.sprite = powerImg;
    }

    public void UsePower(float _cooltime)
    {
        StartCoroutine(ShowCooltime(_cooltime));
    }
    private IEnumerator ShowCooltime(float _cooltime)
    {
        float cnt = _cooltime;
        while (cnt > 0)
        {
            cnt-=Time.deltaTime;
            float per = cnt / _cooltime;
            SetCooltime(per);
            yield return null;
        }
    }
    private void SetCooltime(float _per)
    {
        m_cooltimeImg.fillAmount = _per;
    }



    public void SetComps()
    {
        m_powerImg = GetComponentsInChildren<Image>()[1];
        m_cooltimeImg = GetComponentsInChildren<Image>()[2];
    }
}
