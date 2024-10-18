using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatternLeftElm : MonoBehaviour
{
    private PatternLeftList m_parent;
    public void SetParent(PatternLeftList _parent) { m_parent = _parent; }

    private Image m_patternIcon;
    private TextMeshProUGUI m_patternName;
    private TextMeshProUGUI m_patternNum;
    private Button m_registerBtn;

    private EPatternName CurPattern { get; set; }
    private bool CanRegister { get; set; }


    private void SetPatternInfo()
    {
        SItem item = new(EItemType.PATTERN, (int)CurPattern);
        ItemScriptable data = GameManager.GetItemData(item);
        m_patternName.text = data.ItemName;
        m_patternIcon.sprite = data.ItemImage;
    }


    public void UpdateElm()
    {
        int num = PlayManager.PatternNum[(int)CurPattern];
        CanRegister = num > 0;
        m_patternNum.text = num.ToString();
        m_registerBtn.interactable = CanRegister;
    }

    private void RegisterPattern()
    {
        if (!CanRegister) { return; }
        m_parent.RegisterPattern(CurPattern);
    }



    private void SetBtns()
    {
        m_registerBtn.onClick.AddListener(RegisterPattern);
    }

    public void SetComps(int _idx)
    {
        CurPattern = (EPatternName)_idx;
        m_patternIcon = GetComponentsInChildren<Image>()[1];
        TextMeshProUGUI[] txts = GetComponentsInChildren<TextMeshProUGUI>();
        m_patternName = txts[2];
        m_patternNum = txts[1];
        m_registerBtn = GetComponentInChildren<Button>();
        SetBtns();
        SetPatternInfo();
    }
}
