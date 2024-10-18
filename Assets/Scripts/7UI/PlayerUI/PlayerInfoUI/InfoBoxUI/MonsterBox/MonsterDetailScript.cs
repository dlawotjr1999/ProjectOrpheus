using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDetailScript : MonoBehaviour
{
    [SerializeField]
    private Image m_monsterImage;
    private TextMeshProUGUI m_monsterName;
    private TextMeshProUGUI m_monsterDesc;

    public void SetMonsterDetail(EMonsterName _monster)
    {
        if (_monster == EMonsterName.LAST)
        {
            m_monsterImage.enabled = false;
            m_monsterName.text = "-";
            m_monsterDesc.text = "";
            return;
        }

        MonsterScriptable data = GameManager.GetMonsterData(_monster);

        m_monsterImage.enabled = true;
        m_monsterImage.sprite  = data.MonsterBodyImg;
        m_monsterName.text = data.MonsterName;
        m_monsterDesc.text = data.Description;
    }


    public void SetComps()
    {
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_monsterName = txts[0];
        m_monsterDesc = txts[1];
    }
}
