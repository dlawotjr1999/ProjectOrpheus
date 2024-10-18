using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVolumeType
{
    BGM,
    SE,
    LAST
}

public class VolumeCtrlScript : MonoBehaviour
{
    private OptionSettingUI m_parent;
    public void SetParent(OptionSettingUI _parent) { m_parent = _parent; }

    [SerializeField]
    private EVolumeType m_volume;

    private VolumeToggleScript m_toggle;

    public void SetPoint(int _vol)
    {
        m_toggle.SetPoint(_vol);
    }

    public void SetVolume(int _vol)
    {
        m_parent.SetVolume(m_volume, _vol);
    }


    private void Awake()
    {
        m_toggle = GetComponentInChildren<VolumeToggleScript>();
        m_toggle.SetParent(this);
    }
}
