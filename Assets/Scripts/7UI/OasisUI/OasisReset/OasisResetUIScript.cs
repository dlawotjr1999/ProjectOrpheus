using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OasisResetUIScript : MonoBehaviour, IOasisUI
{
    private OasisUIScript m_parent;

    private bool IsCompsSet { get; set; }

    public void OpenUI(OasisUIScript _parent)
    {
        gameObject.SetActive(true);
        if (!IsCompsSet) { m_parent = _parent; SetComps(); }
        UpdateUI();
    }

    [SerializeField]
    private Button m_cancelBtn;

    private OasisStatResetUI m_statResetUI;
    private OasisTraitResetUI m_traitResetUI;

    public void UpdateUI()
    {
        m_statResetUI.UpdateUI();
        m_traitResetUI.UpdateUI();
    }


    public void CloseUI()
    {
        m_parent.FunctionDone();
        gameObject.SetActive(false);
    }


    private void SetBtns()
    {
        m_cancelBtn.onClick.AddListener(CloseUI);
    }

    private void SetComps()
    {
        m_statResetUI = GetComponentInChildren<OasisStatResetUI>();
        m_statResetUI.SetParent(this); m_statResetUI.SetComps();
        m_traitResetUI = GetComponentInChildren<OasisTraitResetUI>();
        m_traitResetUI.SetParent(this); m_traitResetUI.SetComps();
        SetBtns();
    }
}
