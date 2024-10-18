using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragDropUIScript : MonoBehaviour
{
    protected RectTransform m_rect;
    protected EventTrigger m_trigger;

    public virtual Transform MoveTrans { get { return m_rect.parent; } private set { } }

    protected Vector2 StartPos { get; set; }
    protected Vector2 MouseStart { get; set; }
    protected Transform ParentTrans { get; set; }
    public void ChangeParentTrans(Transform _trans) { ParentTrans = _trans; ParentChanged = true; }
    private bool ParentChanged { get; set; }
    protected int SiblingIdx { get; set; }

    public virtual bool CanControl => true;
    public bool Dragging { get; private set; }


    public virtual void StartDrag(PointerEventData _data)
    {
        if(!CanControl || _data.button == PointerEventData.InputButton.Right) { return; }
        ParentTrans = m_rect.parent;
        SiblingIdx = transform.GetSiblingIndex();
        m_rect.SetParent(MoveTrans);
        StartPos = m_rect.anchoredPosition;
        MouseStart = Mouse.current.position.ReadValue();
        Dragging = true;
    }

    public virtual void OnDrag(PointerEventData _data)
    {
        if (!CanControl || _data.button == PointerEventData.InputButton.Right) { return; }
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector2 move = mouse - MouseStart;
        move.x *= GameManager.WidthRatio; move.y *= GameManager.HeightRatio;
        m_rect.anchoredPosition = StartPos + move;
        CheckPos();
    }

    public virtual void EndDrag(PointerEventData _data)
    {
        if (!CanControl || _data.button == PointerEventData.InputButton.Right) { return; }
        if (CheckPos())
        {
            DropAction();
        }
        m_rect.anchoredPosition = StartPos;
        m_rect.SetParent(ParentTrans);
        if (ParentChanged) { m_rect.anchoredPosition = Vector2.zero; ParentChanged = false; }
        transform.SetSiblingIndex(SiblingIdx);
        Dragging = false;
    }

    public virtual bool CheckPos()
    {
        return false;
    }
    public virtual void DropAction()
    {
        
    }


    public virtual void SetEvents()
    {
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerDown, StartDrag);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.Drag, OnDrag);
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerUp, EndDrag);
    }

    public virtual void SetComps()
    {
        m_rect = GetComponent<RectTransform>();
        m_trigger = gameObject.AddComponent<EventTrigger>();
    }

    public virtual void Awake()
    {
        SetComps();
        SetEvents();
    }
}
