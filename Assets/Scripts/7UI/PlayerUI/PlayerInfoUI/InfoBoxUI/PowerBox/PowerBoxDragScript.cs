using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerBoxDragScript : DragDropUIScript
{
    private PowerBoxElmScript m_parent;
    public void SetParent(PowerBoxElmScript _parent) { m_parent = _parent; }

    private EPowerName Power { get { return m_parent.CurPower; } }
    private PowerBoxUIScript Box { get { return m_parent.Box; } }
    public override Transform MoveTrans => Box.transform;

    public override bool CanControl => m_parent.IsObtained && !m_parent.IsEquipped;

    private int DropIdx { get; set; }


    public override bool CheckPos()
    {
        RectTransform[] slots = Box.SlotTrans;
        float[] dists = new float[3];
        for (int i = 0; i < ValueDefine.MAX_POWER_SLOT; i++)
        {
            Vector3 slot = slots[i].position + Vector3.up * 48f;
            float dist = Vector2.Distance(m_rect.position, slot);
            dists[i] = dist;
            if (dist < 50)
            {
                DropIdx = i;
                m_rect.position = slot;
                return true;
            }
        }
        DropIdx = -1;
        return false;
    }
    public override void DropAction()
    {
        if (DropIdx == -1) { return; }
        PlayManager.RegisterPowerSlot(Power, DropIdx);
        Box.UpdateUI();
    }
}
