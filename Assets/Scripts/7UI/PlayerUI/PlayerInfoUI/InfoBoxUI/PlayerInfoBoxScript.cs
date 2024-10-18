using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoBoxScript : MonoBehaviour
{
    protected PlayerInfoUIScript m_parent;
    public void SetParent(PlayerInfoUIScript _parent) { m_parent = _parent; }

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        InitUI();
    }

    public virtual void InitUI()
    {

    }

    public virtual void UpdateUI()
    {

    }

    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetComps()
    {

    }
}
