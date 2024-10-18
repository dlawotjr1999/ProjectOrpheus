using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitPoisonUIScript : MonoBehaviour
{
    private Animator m_anim;

    private bool IsBlind { get; set; }

    public void ShowBlind() 
    {
        IsBlind = true;
        m_anim.SetBool("IS_SHOWING", true);
    }
    public void HideBlind() 
    {
        IsBlind = false;
        m_anim.SetBool("IS_SHOWING", false); 
    }



    private void Awake()
    {
        m_anim = GetComponent<Animator>();
    }
}
