using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadListScript : BaseUI
{
    private LoadListElm[] m_elms;

    [SerializeField]
    private Button m_closeBtn;


    public override void UpdateUI()
    {
        List<SaveData> data = GameManager.GameData;
        for (int i = 0; i<ValueDefine.MAX_SAVE; i++)
        {
            if (i >= data.Count) { m_elms[i].EmptySlot(i); continue; }
            m_elms[i].LoadData(i, data[i]);
        }
    }

    public virtual void LoadGame(int _idx)
    {
        GameManager.LoadGame(_idx);
    }


    private void SetBtns()
    {
        m_closeBtn.onClick.AddListener(CloseUI);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_elms = GetComponentsInChildren<LoadListElm>();
        if(m_elms.Length != ValueDefine.MAX_SAVE) { Debug.Log("로딩 슬롯 개수 다름"); return; }
        foreach(LoadListElm elm in m_elms) { elm.SetParent(this); elm.SetComps(); }
        SetBtns();
    }
}
