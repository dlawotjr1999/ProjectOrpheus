using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponInfoUIScript : MonoBehaviour
{
    private RectTransform m_rect;
    private TextMeshProUGUI m_nameTxt;
    private TextMeshProUGUI m_descTxt;
    private TextMeshProUGUI m_elseTxt;

    private bool IsCompsSet { get; set; }

    public void ShowUI(ItemInfo _weapon)
    {
        if (!IsCompsSet) { SetComps(); }
        gameObject.SetActive(true);
        m_nameTxt.text = _weapon.ItemName;
        m_descTxt.text = _weapon.ItemDescription;
        m_elseTxt.text = "¾îÂ¼±¸ ÀúÂ¼±¸";
    }
    public void SetPos(Vector2 _pos)
    {
        m_rect.anchoredPosition = _pos;
    }
    public void HideUI()
    {
        gameObject.SetActive(false);
    }


    public void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_nameTxt = txts[0];
        m_descTxt = txts[1];
        m_elseTxt = txts[2];
        IsCompsSet = true;
    }
}
