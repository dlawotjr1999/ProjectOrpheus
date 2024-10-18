using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCameraScript m_playerCamera;                                  // 현재 카메라
    public CinemachineFreeLook PlayerFreeLook { get { return m_playerCamera.PlayerFreeLook; } }

    public float CameraRotation { get { return transform.eulerAngles.y; } }     // 카메라가 좌우 각도
    public float CameraAngle { get { return -transform.eulerAngles.x; } }       // 카메라 위아래 각도

    public void SetCameraMode(EControlMode _mode)                               // 조작 모드 전달 받음
    {
        if (_mode == EControlMode.THIRD_PERSON) { m_playerCamera.SetThirdPerson(); }
        else if (_mode == EControlMode.UI_CONTROL) { m_playerCamera.SetUIControl(); }
    }
    public void SetNPCView() { m_playerCamera.SetNPCView(); }

    public void SetCameraSensitive(float _sensitive)                            // 카메라 민감도 전달 받음
    {
        m_playerCamera.SetCameraSensitive(_sensitive);
    }

    public void LooseFocus()
    {
        m_playerCamera.LooseFocus();
    }
}
