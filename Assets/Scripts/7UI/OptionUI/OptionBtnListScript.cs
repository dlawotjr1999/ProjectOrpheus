using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionBtnListScript : MonoBehaviour
{
    private OptionUIScript m_parent;
    public void SetParent(OptionUIScript _parent) { m_parent = _parent; }


    private Button[] m_btns;




    private void SetBtns()
    {
        for (int i = 0; i<(int)EOptionFunction.LAST; i++)
        {
            EOptionFunction func = (EOptionFunction)i;
            int t = i;
            m_btns[t].onClick.AddListener(delegate { m_parent.OptionFunction(func); });
        }
    }

    public void SetComps()
    {
        m_btns = GetComponentsInChildren<Button>();
        if(m_btns.Length != (int)EOptionFunction.LAST) { Debug.LogError("옵션 버튼 개수 다름"); return; }
        SetBtns();
    }
}
