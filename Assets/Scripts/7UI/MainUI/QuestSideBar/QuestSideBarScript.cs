using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSideBarScript : MonoBehaviour
{
    private RectTransform m_rect;

    private QuestBarElmScript[] m_elms;


    private readonly float ElmWidth = 448;
    private readonly float ElmHeight = 64;
    private readonly float ElmSpace = 8;

    public void UpdateUI()
    {
        List<QuestInfo> curList = new();
        List<QuestInfo> infos = PlayManager.QuestInfoList;
        foreach(QuestInfo info in infos) 
        {
            if(info.State == EQuestState.ACCEPTED || info.State == EQuestState.COMPLETE)
            {
                curList.Add(info);
            }
        }
        ApplyInfos(curList);
    }

    private void ApplyInfos(List<QuestInfo> _infos)
    {
        int count = _infos.Count;
        SetBoxSize(count);
        for (int i = 0; i<ValueDefine.MAX_QUEST_NUM; i++)
        {
            if(i >= count) { m_elms[i].HideElm(); continue; }
            m_elms[i].SetElm(_infos[i]);
        }
    }
    private void SetBoxSize(int _count)
    {
        float width = ElmWidth + ElmSpace * 2;
        float height = _count > 0 ? ElmHeight * _count + ElmSpace * (_count + 3) : 0;
        m_rect.sizeDelta = new(width, height);
    }


    private void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        m_elms = GetComponentsInChildren<QuestBarElmScript>();
        if (m_elms.Length != ValueDefine.MAX_QUEST_NUM) { Debug.Log("퀘스트 UI 개수 다름"); return; }
        foreach(QuestBarElmScript elm in m_elms) { elm.SetComps(); }
    }
    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        UpdateUI();
    }
}
