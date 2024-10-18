using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    private CinemachineFreeLook m_primaryCamera;
    private CinemachineFreeLook m_currentCamera;
    [SerializeField]
    private CinemachineFreeLook m_changeCamera;

    public CinemachineFreeLook[] m_changeCameras;


    // 카메라 전환 액션 메소드
    public void SwitchToCamera(CinemachineFreeLook targetCamera)
    {
        (m_currentCamera.Priority, targetCamera.Priority) = (targetCamera.Priority, m_currentCamera.Priority);   // cinemachinefreelook의 priority 교환
        m_currentCamera = targetCamera;
    }

    // 벽에 닿았을 때 카메라 액션 메소드
    private void WallCameraAction(float newFOV)
    {
        PlayManager.PlayerFreeLook.m_Lens.FieldOfView = newFOV;
    }

    public void SetCameraComp()
    {
        m_primaryCamera = PlayManager.PlayerFreeLook;
        m_changeCamera = GetComponentInChildren<CinemachineFreeLook>();
    }

    public void SetCameraPriority()
    {
        m_currentCamera = m_primaryCamera;
        m_currentCamera.Priority = 20;
        m_changeCamera.Priority = 10;
    }

    private void Start()
    {
        SetCameraComp();
        SetCameraPriority();
    }
}
