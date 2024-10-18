using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragMouseOverInfoUI : DragDropUIScript            // 마우스 올리면 정보 뜨는 애
{
    private bool IsMouseOn { get; set; }
    private float m_lastClickTime = 0;
    private float m_doubleClickThreshold = 0.3f;

    public override void StartDrag(PointerEventData _data)
    {
        base.StartDrag(_data);
        HideInfo();
    }


    private void PointerOn(PointerEventData _data) { if (Dragging) { return; } IsMouseOn = true; ShowInfo(); StartCoroutine(ShowingUI()); }
    private void PointerOff(PointerEventData _data) { IsMouseOn = false; HideInfo(); }
    protected void DoubleClick(PointerEventData _data)
    {
        if (Time.time - m_lastClickTime < m_doubleClickThreshold)
        {
            OnDoubleClick(_data);
        }
        m_lastClickTime = Time.time;
    }

    public virtual void ShowInfo() { }
    public virtual void HideInfo() { }
    public virtual void SetInfoPos(Vector2 _pos) { }
    public virtual void OnDoubleClick(PointerEventData _data) { }
    private IEnumerator ShowingUI()
    {
        while (!Dragging && IsMouseOn)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();
            SetInfoPos(mouse);
            yield return null;
        }
    }


    public override void SetEvents()
    {
        base.SetEvents();
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerEnter, PointerOn);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerExit, PointerOff);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerClick, DoubleClick);
    }
}
