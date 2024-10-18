using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHPBarScript : MonoBehaviour
{
    private Slider m_slider;
    private TextMeshProUGUI m_hpValue;

    private float MaxHP { get; set; }

    public void SetMaxHP(float _hp)
    {
        m_slider.maxValue = _hp;
        MaxHP = _hp;
        SetCurHP(_hp);
    }

    public void SetCurHP(float _hp)
    {
        m_slider.value = _hp;
        m_hpValue.text = $"{_hp:F0}/{MaxHP:F0}"; 
    }


    public void SetComps()
    {
        m_slider = GetComponent<Slider>();
        m_hpValue = GetComponentInChildren<TextMeshProUGUI>();
    }
}