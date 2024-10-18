using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionLoadUIScript : LoadListScript
{
    private OptionUIScript m_parent;
    public void SetParent(OptionUIScript _parent) { m_parent = _parent; }

    public override void LoadGame(int _idx)
    {
        GameManager.LoadGame(_idx);
    }

    public override void CloseUI()
    {
        m_parent.PopupClosed();
        base.CloseUI();
    }
}
