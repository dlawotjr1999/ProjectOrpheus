using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHPBarScript : MonoBehaviour          // 오브젝트 HP바
{
    private Slider m_hpSlider;

    private bool IsCompsSet { get; set; }


    public void ShowUI()
    {
        if (gameObject.activeSelf) { return; }
        gameObject.SetActive(true);
    }

    public void SetMaxHP(float _max)
    {
        if(!IsCompsSet) { SetComps(); }
        m_hpSlider.maxValue = (int)_max;
        SetCurHP(_max);
    }

    public void SetCurHP(float _hp)
    {
        m_hpSlider.value = (int)_hp;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }


    private void SetComps()
    {
        m_hpSlider = GetComponentInChildren<Slider>();
        IsCompsSet = true;
    }
    private void Awake()
    {
        if (!IsCompsSet)
        {
            SetComps();
        }
    }
}
