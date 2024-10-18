using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMaterialUIScript : MonoBehaviour
{
    private TextMeshProUGUI m_soulTxt;
    private TextMeshProUGUI m_purifiedTxt;


    public void OpenUI()
    {
        UpdateMaterials();
    }


    public void UpdateMaterials()
    {
        int soul = PlayManager.SoulNum;
        int purified = PlayManager.PurifiedNum;
        int[] pattern = PlayManager.PatternNum;

        m_soulTxt.text = $"��ȥ ���� : {soul}��";
        m_purifiedTxt.text = $"������ ��ȥ ���� : {purified}��";

    }


    public void SetComps()
    {
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_soulTxt = txts[0];
        m_purifiedTxt = txts[1];
    }
}
