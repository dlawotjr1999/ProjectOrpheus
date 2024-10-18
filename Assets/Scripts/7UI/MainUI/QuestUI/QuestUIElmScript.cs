using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIElmScript : MonoBehaviour
{
    private TextMeshProUGUI m_title;

    public void SetElm(QuestInfo _info)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
        m_title.text = _info.QuestData.Name;
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    public void SetComps()
    {
        m_title = GetComponent<TextMeshProUGUI>();
    }
}
