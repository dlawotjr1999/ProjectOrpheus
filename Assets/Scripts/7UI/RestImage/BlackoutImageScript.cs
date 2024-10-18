using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackoutImageScript : MonoBehaviour
{
    private Animator m_anim;

    public void ShowImg() { m_anim.SetBool("IS_SHOWING", true); }
    public void HideImg() { m_anim.SetBool("IS_SHOWING", false); }



    private void Awake()
    {
        m_anim = GetComponent<Animator>();
    }
}
