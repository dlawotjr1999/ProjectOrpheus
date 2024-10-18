using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoxUIScript : PlayerInfoBoxScript
{
    private StatInfoUIScript m_statInfoUI;
    private CombatInfoUIScript m_combatInfoUI;


    public override void InitUI()
    {
        UpdateUI();
    }

    public override void UpdateUI()
    {
        PlayerStatInfo statInfo = PlayManager.PlayerStatInfo;
        m_statInfoUI.InitStats(statInfo);
        m_combatInfoUI.InitStats(statInfo);
    }

    public void UpdateStatUse(int[] _points)
    {
        m_combatInfoUI.UpdateUsingPoint(_points);
    }
    public void ResetStatUse()
    {
        m_combatInfoUI.ResetUsingPoint();
    }

    public override void SetComps()
    {
        m_statInfoUI = GetComponentInChildren<StatInfoUIScript>();
        m_statInfoUI.SetParent(this);
        m_combatInfoUI = GetComponentInChildren<CombatInfoUIScript>();
        m_combatInfoUI.SetParent(this);
    }
}
