using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettingUI : BaseUI
{
    private OptionUIScript m_parent;
    public void SetParent(OptionUIScript _parent) { m_parent = _parent; }

    [SerializeField]
    private Button m_closeBtn;

    private VolumeCtrlScript[] m_volumes;
    private ScreenSettingScript m_screen;

    public override void UpdateUI()
    {
        
    }

    public override void CloseUI()
    {
        base.CloseUI();
        m_parent.PopupClosed();
    }

    // 사운드
    public void SetBGMPoint(int _vol)
    {
        m_volumes[0].SetPoint(_vol);
    }
    public void SetSEPoint(int _vol)
    {
        m_volumes[1].SetPoint(_vol);
    }
    public void InitValues()
    {

    }
    public void SetVolume(EVolumeType _type, int _vol)
    {
        if (_type == EVolumeType.BGM)
            GameManager.SetBGMVolume(_vol);
        else if (_type == EVolumeType.SE)
            GameManager.SetSEVolume(_vol);
    }

    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        SetBtns();

        m_volumes = GetComponentsInChildren<VolumeCtrlScript>();
        foreach (VolumeCtrlScript volume in m_volumes) volume.SetParent(this);
        m_screen = GetComponentInChildren<ScreenSettingScript>();
        m_screen.SetParent(this);
    }
}
