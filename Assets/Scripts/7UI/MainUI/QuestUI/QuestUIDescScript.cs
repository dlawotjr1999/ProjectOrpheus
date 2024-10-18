using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIDescScript : MonoBehaviour
{
    private Button m_btn;
    private TextMeshProUGUI m_desc;

    public void SetElm(QuestInfo _info)
    {
        m_desc.text = _info.QuestData.Description;
    }

    public void GiveUpQuest()
    {
        List<QuestInfo> infos = PlayManager.QuestInfoList;
        
        // PlayManager.GiveUpQuest()
    }

    public void SetComps()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(GiveUpQuest);
    }
}
