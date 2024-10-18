using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OasisSoulInfoScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_soulNum;

    public void UpdateUI()
    {
        int soul = PlayManager.SoulNum;
        m_soulNum.text = soul.ToString();
    }
}
