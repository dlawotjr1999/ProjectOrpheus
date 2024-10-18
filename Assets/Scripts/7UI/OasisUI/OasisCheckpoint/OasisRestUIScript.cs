using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisRestUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;

    private bool IsCompsSet { get; set; }

    public void OpenUI(OasisUIScript _parent)
    {
        gameObject.SetActive(true);
        if (!IsCompsSet)
        {
            m_parent = _parent;
            SetComps();
        }
    }


    private void RestInPeace()
    {
        PlayManager.RestAtPoint(m_parent.Oasis);
        CloseUI();
    }

    private void CancelUI()
    {
        CloseUI();
    }

    public void CloseUI()
    {
        m_parent.FunctionDone();
        m_parent.CloseUI();
        gameObject.SetActive(false);
    }


    private void SetComps()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(RestInPeace);
        btns[1].onClick.AddListener(CancelUI);
        IsCompsSet = true;
    }
}
