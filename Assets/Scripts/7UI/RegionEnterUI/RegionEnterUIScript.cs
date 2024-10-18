using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegionEnterUIScript : MonoBehaviour
{
    private Animator m_anim;
    private TextMeshProUGUI m_regionTxt;

    public void ShowEnterUI(ERegion _region)
    {
        string region = EnvironmentManager.Region2String(_region);
        m_regionTxt.text = region;
        m_anim.SetTrigger("SHOW");
    }

    private void SetComps()
    {
        m_anim = GetComponent<Animator>();
        m_regionTxt = GetComponent<TextMeshProUGUI>();
    }

    private void Awake()
    {
        SetComps();
    }
}
