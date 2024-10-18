using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractInfoUI : MonoBehaviour
{
    private Animator m_anim;
    private TextMeshProUGUI m_infoTxt;

    public void ShowInteractInfo(string _info)
    {
        m_anim.SetBool("IS_SHOWING", true);
        m_infoTxt.text = _info;
    }

    public void HideInteractInfo()
    {
        m_anim.SetBool("IS_SHOWING", false);
    }



    private void SetComps()
    {
        m_anim = GetComponent<Animator>();
        m_infoTxt = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }

    private void Awake()
    {
        SetComps();
    }
}
