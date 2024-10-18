using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettingScript : MonoBehaviour
{
    private OptionSettingUI m_parent;
    public void SetParent(OptionSettingUI _parent) { m_parent = _parent; SetComps(); }

    private Button[] m_btn;

    public void SetFullScreenMode()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        Debug.Log("전체화면");
    }

    public void SetWindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(1280, 720, false);
        Debug.Log("창 모드");
    }

    public void SetComps()
    {
        m_btn = GetComponentsInChildren<Button>();
        m_btn[0].onClick.AddListener(SetFullScreenMode);   // 전체 화면
        m_btn[1].onClick.AddListener(SetWindowMode);       // 창 모드
    }
}
