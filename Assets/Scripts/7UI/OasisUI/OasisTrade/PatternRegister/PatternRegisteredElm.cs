using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternRegisteredElm : MonoBehaviour
{
    private Image m_patternIcon;

    public void SetElm(Sprite _icon)
    {
        m_patternIcon.enabled = true;
        m_patternIcon.sprite = _icon;
    }

    public void EmptyElm()
    {
        m_patternIcon.enabled = false;
    }

    public void SetComps()
    {
        m_patternIcon = GetComponentsInChildren<Image>()[1];
    }
}
