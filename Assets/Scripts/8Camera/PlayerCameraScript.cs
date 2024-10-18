using Cinemachine;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    private CinemachineFreeLook m_cameraDetail;
    public CinemachineFreeLook PlayerFreeLook { get { return m_cameraDetail; } }

    // private string[] m_collideAgainstLayers = { "Ground" };

    private const float XMoveMultiplier = 100;              // 민감도 1당 X 움직임
    private const float YMoveMultiplier = 1.5f;                // 민감도 1당 Y 움직임

    private EControlMode CurCameraMode { get; set; }        // 현재 조작 모드
    private float MouseSensitive { get; set; }              // 마우스 민감도

    public void SetThirdPerson()                            // 3인칭 모드 설정
    {
        CurCameraMode = EControlMode.THIRD_PERSON;
        m_cameraDetail.m_Lens.FieldOfView = 40;
        m_cameraDetail.m_YAxis.Value = 0.5f;
        SetCinemachineSpeed(MouseSensitive);
    }
    public void ResetTrace()
    {
        m_cameraDetail.Follow = PlayManager.PlayerTransform;
    }
    public void LooseFocus()
    {
        m_cameraDetail.Follow = PlayManager.CameraFocusTransform;
    }
    public void SetCameraSensitive(float _sensitive)        // 마우스 민감도 설정
    {
        MouseSensitive = _sensitive;
        if (CurCameraMode == EControlMode.THIRD_PERSON)
        {
            SetCinemachineSpeed(MouseSensitive);
        }
    }
    public void SetUIControl()                              // UI 조작 모드 설정
    {
        CurCameraMode = EControlMode.UI_CONTROL;
        SetCinemachineSpeed(0);
    }
    public void SetNPCView()
    {
        m_cameraDetail.m_Lens.FieldOfView = 25;
        m_cameraDetail.m_YAxis.Value = 0.6f;
    }
    private void SetCinemachineSpeed(float _speed)          // 실제 민감도 설정 함수
    {
        m_cameraDetail.m_XAxis.m_MaxSpeed = XMoveMultiplier * _speed;
        m_cameraDetail.m_YAxis.m_MaxSpeed = YMoveMultiplier * _speed;
    }


    private void SetComps()
    {
        m_cameraDetail = GetComponent<CinemachineFreeLook>();
    }

    private void Awake()
    {
        SetComps();
    }
    private void Start()
    {
        m_cameraDetail.m_XAxis.Value = PlayManager.PlayerDirection;

        //if (m_cameraDetail != null)
        //{
        //    var collider = m_cameraDetail.GetComponent<CinemachineCollider>();
        //    if (collider != null)
        //    {
        //        collider.m_CollideAgainst = LayerMask.GetMask(m_collideAgainstLayers);
        //    }
        //}
    }
}