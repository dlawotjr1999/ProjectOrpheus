using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterBoxElmScript : MonoBehaviour
{
    [SerializeField]
    private Image m_mosnterProfile;

    private TextMeshProUGUI m_monsterNameTxt;
    private TextMeshProUGUI m_monsterStateTxt;
    private Button m_btn;

    private EMonsterName m_monster;

    private MonsterBoxUIScript m_parent;
    public void SetParent(MonsterBoxUIScript _parent) { m_parent = _parent; }

    public void SetMonsterInfo(EMonsterName _monster)
    {
        m_monster = _monster;
        Sprite img = GameManager.GetMonsterProfile(_monster);
        MonsterInfo info = GameManager.GetMonsterInfo(_monster);

        m_mosnterProfile.sprite = img;
        m_monsterNameTxt.text = info.MonsterName;
        if (info.Cleared)
            m_monsterStateTxt.text = "완료";
        else
            m_monsterStateTxt.text = "미완료";
    }

    public void HideElm()
    {
        gameObject.SetActive(false);
    }

    public void SetMonsterImg()
    {
        m_parent.SetMonsterDetail(m_monster);
    }

    private void SetComps()
    {
        m_monsterNameTxt = GetComponentsInChildren<TextMeshProUGUI>()[0];
        m_monsterStateTxt = GetComponentsInChildren<TextMeshProUGUI>()[1];
        m_btn = GetComponent<Button>(); 
        m_btn.onClick.AddListener(SetMonsterImg);
    }

    private void Awake()
    {
        SetComps();
    }
}
