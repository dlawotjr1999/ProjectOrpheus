using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastUIScript : MonoBehaviour
{
    private Image m_img;

    private readonly Color[] m_aimColors = new Color[2] { Color.white, Color.red };

    public void ShowAim()
    {
        m_img.enabled = true;
    }
    public void SetAimState(bool _on)
    {
        if (_on)
        {
            m_img.color = m_aimColors[1];
        }
        else
        {
            m_img.color = m_aimColors[0];
        }
    }
    public void HideAim()
    {
        m_img.enabled = false;
    }


    public void SetComps()
    {
        m_img = GetComponent<Image>();
    }
}
