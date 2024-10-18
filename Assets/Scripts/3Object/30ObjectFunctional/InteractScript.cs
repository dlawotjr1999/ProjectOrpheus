using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class InteractScript : MonoBehaviour                 // 상호작용이 가능한 오브젝트에 넣는 스크립트
{
    private Vector2 Position2 { get { return new(transform.position.x, transform.position.z); } }

    [SerializeField]
    private float m_canInteractDist = 2.5f;                                             // 상호작용 가능 거리
    private float m_canInteractAngle = 120f;

    private IInteractable m_interactable;                                               // 오브젝트 내 IInteractable을 상속한 오브젝트
    private QuestNPCScript m_questNPC;

    private float InteractAngle { get { if (m_interactable.InteractType == EInteractType.NPC) return m_canInteractAngle / 2; return m_canInteractAngle; } }
    public bool CanInteract { get { return DistToPlayer <= m_canInteractDist && CheckInteractable; } }  // 상호작용 가능한지


    public float DistToPlayer { get { return PlayManager.GetDistToPlayer(Position2); } }           // 플레이어와의 거리
    public float AngleToPlayer { get {
            Vector2 dir = (PlayManager.PlayerPos2 - Position2).normalized;
            float rot = FunctionDefine.VecToDeg(dir);
            Vector2 forward = new(transform.forward.x, transform.forward.z);
            float fRot = FunctionDefine.VecToDeg(forward);
            float gap = rot - fRot;
            if (gap <= -360) { gap += 360; } else if(gap >= 360) { gap -= 360; }
            return gap; } }
    public Transform InteractTransform { get { return transform; } }                                        // 상호작용 대상의 위치

    public bool CheckInteractable { get { return m_interactable.CanInteract; } }

    public void AbleInteract()                 // 조작 허용
    {
        if (!CanInteract) { return; }
        ShowToggleUI();
    }
    public void DisableInteract()              // 조작 비허용
    {
        HideToggleUI();
    }
    private void ShowToggleUI()                 // 조작 UI 띄우기
    {
        PlayManager.ShowInteractInfo(m_interactable.InfoTxt);
    }
    private void HideToggleUI()                 // 조작 UI 숨기기
    {
        PlayManager.HideInteractInfo();
    }


    public void StartInteract()                // 상호작용 시작
    {
        m_interactable.StartInteract();
        if (m_questNPC != null) { PlayManager.SetNPCView(); }
        HideToggleUI();
    }
    public void StopInteract()                  // 상호작용 중단
    {
        m_interactable.StopInteract();
        ShowToggleUI();
    }

    private void Awake()
    {
        m_interactable = GetComponent<IInteractable>();
        m_interactable.SetInteractScript(this);
        m_questNPC = GetComponent<QuestNPCScript>();
    }
}
