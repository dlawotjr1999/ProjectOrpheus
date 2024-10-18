using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    [SerializeField]
    private InteractScript m_interactableObject;                                    // 상호작용 누르면 상호작용 할 대상

    private bool Interacting { get; set; }                                          // 상호작용 중인지

    private void DetactObjectsNear()                                                // 주변 상호작용 가능 오브젝트 탐지
    {
        if (Interacting) { return; } // 상호작용 상태일 경우

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 10, ValueDefine.INTERACT_LAYER);

        InteractScript interact = null;         // 상호작용 대상 탐색
        for (int i = 0; i < hits.Length; i++)   // 근처 대상 확인
        {
            GameObject obj = hits[i].collider.gameObject;
            InteractScript script = obj.GetComponentInParent<InteractScript>()
                ?? obj.GetComponentInChildren<InteractScript>();
            if (script == null || !script.CanInteract ) { continue; }               // 스크립트가 없거나 상호작용 불가능인 경우
            interact = script;
        }
        if (interact != null && interact != m_interactableObject)   // 대상이 바뀐 경우
        {
            if (m_interactableObject != null)
                m_interactableObject.DisableInteract();

            m_interactableObject = interact;
            m_interactableObject.AbleInteract();
        }
        if (m_interactableObject != null && !m_interactableObject.CanInteract)  // 대상이 사라진 경우
        {
            m_interactableObject.DisableInteract();
            m_interactableObject = null;
        }
    }

    public void StartInteract()                                                     // 상호작용 시작
    {
        Interacting = true;
        GameManager.PlaySE(EPlayerSE.INTERACT, transform.position);
    }
    public void StopInteract()                                                      // 상호작용 중단
    {
        m_interactableObject = null;
        Interacting = false;
    }
    public void StopInteract(InteractScript _interact)
    {
        if(m_interactableObject != _interact) { return; }
    }

    private void PlayerDetactUpdate()                                               // 탐지 관련 업데이트
    {
        DetactObjectsNear();
        if (m_interactableObject != null && PlayerInput.Interact.triggered)
        {
            StartInteract();
            m_interactableObject.StartInteract();
        }
    }
}
