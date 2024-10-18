using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OasisStatResetUI : MonoBehaviour
{
    private OasisResetUIScript m_parent;
    public void SetParent(OasisResetUIScript _parent) { m_parent = _parent; }

    [SerializeField]
    private Button m_resetBtn;
    private OasisStatResetElm[] m_elms;
    private TextMeshProUGUI m_resetTxt;

    private int UsedStats { get { return PlayManager.UsedStatPoint; } }

    public void UpdateUI()
    {
        PlayerStatInfo info = PlayManager.PlayerStatInfo;
        for (int i = 0; i<(int)EStatName.LAST; i++)
        {
            int stat = (int)info.GetStat((EStatName)i);
            m_elms[i].SetChangeTxt(stat);
        }

        m_resetBtn.interactable = UsedStats > 0;
        m_resetTxt.text = $"√ ±‚»≠ (øµ»•{UsedStats}∞≥ »πµÊ)";
    }

    private void ResetStats()
    {
        if (UsedStats == 0) { return; }
        PlayManager.ResetStat();
        m_parent.UpdateUI();
    }


    private void SetBtns()
    {
        m_resetBtn.onClick.AddListener(ResetStats);
    }

    public void SetComps()
    {
        m_elms = GetComponentsInChildren<OasisStatResetElm>();
        if(m_elms.Length != (int)EStatName.LAST) { Debug.LogError("Ω∫≈» ¡§∫∏ ∞≥ºˆ ¥Ÿ∏ß"); return; }
        foreach(OasisStatResetElm elm in m_elms) { elm.SetComps(); }
        m_resetTxt = m_resetBtn.GetComponentInChildren<TextMeshProUGUI>();
        SetBtns();
    }
}
