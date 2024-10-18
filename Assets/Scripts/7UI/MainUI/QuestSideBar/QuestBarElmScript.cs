using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestBarElmScript : MonoBehaviour
{
    private TextMeshProUGUI m_descTxt;
    private TextMeshProUGUI m_stateTxt;

    public void SetElm(QuestInfo _info)
    {
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }

        string desc = _info.QuestData.Description;
        string prog = $"({_info.QuestProgress}/{_info.QuestData.Content.Amount})";
        bool isAccepted = _info.State == EQuestState.ACCEPTED;
        m_descTxt.text = isAccepted ?  $"{desc} {prog}" : _info.QuestData.CompleteInfo;
        m_stateTxt.text = isAccepted ? "진행중" : "완료됨";
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }


    public void SetComps()
    {
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_descTxt = txts[0];
        m_stateTxt = txts[1];
    }
}
