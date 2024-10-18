using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OasisStatResetElm : MonoBehaviour
{
    private TextMeshProUGUI m_changeInfo;

    private int InitialStat { get { return ValueDefine.INITIAL_STAT; } }

    public void SetChangeTxt(int _stat)
    {
        if(_stat == InitialStat) { m_changeInfo.text = InitialStat.ToString(); return; }
        m_changeInfo.text = $"<color=red>{_stat:F0}</color> ¡æ {InitialStat}";
    }

    public void SetComps()
    {
        m_changeInfo = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }
}
