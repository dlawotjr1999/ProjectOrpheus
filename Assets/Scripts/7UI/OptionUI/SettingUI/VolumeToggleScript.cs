using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VolumeToggleScript : MonoBehaviour
{
    private VolumeCtrlScript m_parent;
    public void SetParent(VolumeCtrlScript _parent) { m_parent = _parent; }

    private RectTransform m_rect;

    private const float MaxX = 323;

    private bool Tracking { get; set; }
    private Vector2 StartPos { get; set; }
    private Vector2 StartAnchor { get; set; }
    private float CurVol { get; set; }

    private void SetEvent()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        FunctionDefine.AddEvent(trigger, EventTriggerType.PointerDown, StartTrack);
        FunctionDefine.AddEvent(trigger, EventTriggerType.PointerUp, EndTrack);
    }

    private void StartTrack(PointerEventData _data)
    {
        StartPos = _data.position;
        StartAnchor = m_rect.anchoredPosition;
        Tracking = true;
    }
    private void OnTrack()
    {
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector2 offset = (mouse - StartPos) * GameManager.WidthRatio;
        CurVol = StartAnchor.x + offset.x;
        if (CurVol > MaxX)
            CurVol = MaxX;
        if (CurVol < -MaxX)
            CurVol = -MaxX;
        Vector2 pos = new(CurVol, 0);
        m_rect.anchoredPosition = pos;

        SetVolume();
    }
    private void EndTrack(PointerEventData _data)
    {
        OnTrack();
        Tracking = false;
        SetVolume();
    }

    public void SetPoint(int _vol)
    {
        float x = _vol/100f*(2*MaxX) - MaxX;
        m_rect.anchoredPosition = new(x, 0);
    }

    public void SetVolume()
    {
        int vol = (int)((CurVol+MaxX) / (2*MaxX) * 100);
        m_parent.SetVolume(vol);
    }

    private void Awake()
    {
        m_rect = GetComponent<RectTransform>();
        SetEvent();
    }

    private void Update()
    {
        if (Tracking)
            OnTrack();
    }
}
