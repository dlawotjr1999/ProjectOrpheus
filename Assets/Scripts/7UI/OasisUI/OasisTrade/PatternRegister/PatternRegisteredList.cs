using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternRegisteredList : MonoBehaviour
{
    private PatternRegisteredElm[] m_elms;

    public void UpdateUI()
    {
        EPatternName[] list = PlayManager.HealPatternList;
        for (int i = 0; i<ValueDefine.MAX_REGISTER_PATTERN; i++)
        {
            if(i >= list.Length) { m_elms[i].EmptyElm(); continue; }
            EPatternName pattern = list[i];
            Sprite icon = GameManager.GetItemSprite(new(EItemType.PATTERN, (int)pattern));
            m_elms[i].SetElm(icon);
        }
    }


    public void SetComps()
    {
        m_elms = GetComponentsInChildren<PatternRegisteredElm>();
        if(m_elms.Length != ValueDefine.MAX_REGISTER_PATTERN) { Debug.Log("등록된 문양 UI 개수 틀림"); return; }
        foreach(PatternRegisteredElm elm in m_elms) { elm.SetComps(); }
    }
}
