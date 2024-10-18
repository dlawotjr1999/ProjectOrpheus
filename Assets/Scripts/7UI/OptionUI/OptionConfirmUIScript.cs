using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionConfirmUIScript : BaseUI
{
    private OptionUIScript m_parent;
    public void SetParent(OptionUIScript _parent) { m_parent = _parent; }

    private EOptionFunction CurFunction { get; set; }

    [SerializeField]
    private TextMeshProUGUI m_questionTxt;
    [SerializeField]
    private Button m_confirmBtn;
    [SerializeField]
    private Button m_cancelBtn;


    public void SetQuestion(EOptionFunction _func)
    {
        CurFunction = _func;
        base.OpenUI();
        gameObject.SetActive(true);
    }
    public override void UpdateUI()
    {
        if (CurFunction == EOptionFunction.TITLE)
        {
            m_questionTxt.text = "메인 화면으로 이동합니다.";

        }
        else if (CurFunction == EOptionFunction.QUIT)
        {
            m_questionTxt.text = "게임을 종료합니다.";
        }
        else
        {
            Debug.Log("잘못된 게 들어옴"); return;
        }
    }
    private void ConfirmQuestion()
    {
        if (CurFunction == EOptionFunction.TITLE)
        {
            m_parent.MoeToTitle();
        }
        else if (CurFunction == EOptionFunction.QUIT)
        {
            m_parent.QuitGame();
        }
    }
    public override void CloseUI()
    {
        m_parent.PopupClosed();
        base.CloseUI();
    }


    public override void SetComps()
    {
        m_confirmBtn.onClick.AddListener(ConfirmQuestion);
        m_cancelBtn.onClick.AddListener(CloseUI);
    }
}
