using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_staminaRateImg;
    [SerializeField]
    private GameObject m_lightRateImg;

    private Image m_staminaRate;
    private Image m_lightRate;

    private const float MaxAmount = 0.863f;
    private const float MinAmount = 0.137f;


    public void SetStaminaRate(float _rate)
    {
        float amount = (MaxAmount - MinAmount) *_rate + MinAmount;
        m_staminaRate.fillAmount = amount;
    }
    public void SetLightRate(float _rate)
    {
        float amount = (MaxAmount - MinAmount) *_rate + MinAmount;
        m_lightRate.fillAmount = amount;
    }

    public void SetLightState(bool _on)
    {
        if (_on) { m_lightRate.color = new(1f,1f,1f, 120/255f); }
        else { m_lightRate.color = new(1f, 0f, 0f, 120/255f); }
    }


    public void SetComps()
    {
        m_staminaRate = m_staminaRateImg.GetComponent<Image>();
        m_lightRate = m_lightRateImg.GetComponent<Image>();
    }
}
