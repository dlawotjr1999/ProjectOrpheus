using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EOptionFunction
{
    RESUME,
    LOAD,
    SETTING,
    TITLE,
    QUIT,

    LAST
}

public class OptionUIScript : BaseUI
{
    private OptionBtnListScript m_btnList;
    [SerializeField]
    private OptionLoadUIScript m_loadUI;
    [SerializeField]
    private OptionSettingUI m_settingUI;
    [SerializeField]
    private OptionConfirmUIScript m_confirmUI;
    [SerializeField]
    private Animator m_keyInfoAnim;

    private bool CanControl { get; set; }

    public override void OpenUI()
    {
        base.OpenUI();
        CanControl = true;
        m_keyInfoAnim.SetTrigger("SHOW");
    }
    public override void CloseUI()
    {
        base.CloseUI();
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
    }


    public void OptionFunction(EOptionFunction _func)
    {
        if (!CanControl) { return; }
        switch (_func)
        {
            case EOptionFunction.RESUME: CloseUI(); break;
            case EOptionFunction.LOAD: OpenLoadUI(); break;
            case EOptionFunction.SETTING: OpenSettingUI(); break;
            case EOptionFunction.TITLE: AskToTitle(); break;
            case EOptionFunction.QUIT: AskQuitGame(); break;
        }
        CanControl = false;
    }
    private void OpenLoadUI()
    {
        m_loadUI.OpenUI();
    }
    private void OpenSettingUI()
    {
        m_settingUI.OpenUI();
    }
    private void AskToTitle()
    {
        m_confirmUI.SetQuestion(EOptionFunction.TITLE);
    }
    private void AskQuitGame()
    {
        m_confirmUI.SetQuestion(EOptionFunction.QUIT);
    }
    public void MoeToTitle() 
    {
        GameManager.ReturnToTitle();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PopupClosed()
    {
        CanControl = true;
    }

    public override void SetComps()
    {
        base.SetComps();
        m_btnList = GetComponentInChildren<OptionBtnListScript>();
        m_btnList.SetParent(this); m_btnList.SetComps();
        m_loadUI.SetParent(this);
        m_settingUI.SetParent(this);
        m_settingUI.InitValues();
        m_confirmUI.SetParent(this);
    }
}
