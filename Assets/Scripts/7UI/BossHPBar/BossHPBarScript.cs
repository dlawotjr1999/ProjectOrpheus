using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBarScript : MonoBehaviour
{
    private Animator m_anim;

    private TextMeshProUGUI m_bossName;
    private Slider m_slider;
    private TextMeshProUGUI m_hpTxt;

    private float MaxHP { get; set; }


    public void ShowHPBar(BossMonster _boss)
    {
        m_anim.SetBool("IS_SHOWING", true);
        m_bossName.text = _boss.ObjectName;
        SetMaxHP(_boss.MaxHP);
    }

    private void SetMaxHP(float _max)
    {
        MaxHP = _max;
        m_slider.maxValue = _max;
        SetCurHP(_max);
    }
    public void SetCurHP(float _hp)
    {
        m_slider.value = _hp;
        m_hpTxt.text = $"{_hp:F0} / {MaxHP:F0}";
    }

    public void HideHPBar()
    {
        m_anim.SetBool("IS_SHOWING", false);
    }


    private void SetComps()
    {
        m_anim = GetComponent<Animator>();
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_bossName = txts[0];
        m_slider = GetComponentInChildren<Slider>();
        m_hpTxt = txts[1];
    }
    private void Awake()
    {
        SetComps();
    }
}
