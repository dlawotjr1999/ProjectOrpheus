using UnityEngine;

public enum EControlMode            // 조작 모드
{
    THIRD_PERSON,           // 3인칭
    UI_CONTROL              // UI 조작
}

public class InputManager : MonoBehaviour
{
    private InputSystem m_inputSystem;                                                                  // 전체 Input System
    public InputSystem.PlayerActions PlayerInputs { get { return m_inputSystem.Player; } }              // 플레이어 조작 Input System
    public InputSystem.UIControlActions UIControlInputs { get { return m_inputSystem.UIControl; } }     // UI 조작 Input System

    public EControlMode CurControlMode { get; private set; } = EControlMode.UI_CONTROL;     // 현재 조작 모드

    private static float m_mouseSensitive = 1;                                              // 마우스 민감도
    public static float MouseSensitive { get { return m_mouseSensitive; } }


    public void SetMouseSensitive(float _sensitive)                                         // 마우스 민감도 설정
    {
        m_mouseSensitive = _sensitive;
        PlayManager.SetCameraSensitive(_sensitive);
    }

    public void SetControlMode(EControlMode _mode)                                          // 조작 모드 설정
    {
        CurControlMode = _mode;
        switch (_mode)
        {
            case EControlMode.THIRD_PERSON:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                PlayerInputs.Enable();
                UIControlInputs.Disable();
                break;
            case EControlMode.UI_CONTROL:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                UIControlInputs.Enable();
                PlayerInputs.Disable();
                break;
        }

        if (PlayManager.IsPlaying) { PlayManager.SetCameraMode(_mode); ; }
    }


    public void SetManager()
    {
        SetInputActions();
    }

    private void SetInputActions()
    {
        m_inputSystem = new();
    }

    private void Update()
    {
        if (!PlayManager.IsPlaying) { return; }
        if (CurControlMode == EControlMode.THIRD_PERSON)            // 플레이어 조작 모드일 때
        {
            if (PlayerInputs.OpenPlayUI.triggered)              // Tab 누르면
            {
                PlayManager.TogglePlayerUI(true);           // PlayerUI 열기
                PlayManager.ResetPlayer();
                SetControlMode(EControlMode.UI_CONTROL);    // UI 조작 모드로
            }
            else if (PlayerInputs.OpenOptionUI.triggered)       // Escape 누르면
            {
                PlayManager.ToggleOptionUI(true);
                SetControlMode(EControlMode.UI_CONTROL);    // UI 조작 모드로
            }
            else if (PlayerInputs.OpenMapUI.triggered)
            {
                // 맵 여닫기
                PlayManager.ToggleMapUI(true);
                SetControlMode(EControlMode.UI_CONTROL);
            }
            else if (PlayerInputs.OpenQuestUI.triggered)
            {
                // 퀘스트 창 열기
                PlayManager.ToggleQuestUI(true);
                SetControlMode(EControlMode.UI_CONTROL);
            }
        }
        else if (CurControlMode == EControlMode.UI_CONTROL)         // UI 조작 모드일 때
        {
            if (UIControlInputs.CloseUI.triggered)              // Escape 누르면
            {
                if(PlayManager.IsOptionOpen)
                {
                    PlayManager.ToggleOptionUI(false);
                    return;
                }
                else if (PlayManager.IsPlayerUIOpen)
                {
                    PlayManager.TogglePlayerUI(false);          // PlayerUI 닫기
                    return;
                }
                else if (PlayManager.IsMapUIOpen)
                {
                    PlayManager.ToggleMapUI(false);
                    return;
                }
                else if (PlayManager.IsQuestUIOpen)
                {
                    PlayManager.ToggleQuestUI(false);
                    return;
                }
            }

        }
    }
}
