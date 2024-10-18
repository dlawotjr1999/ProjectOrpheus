using UnityEngine;
using UnityEngine.EventSystems;

public class StatCtrlBtnScript : MonoBehaviour
{
    private StatInfoUIScript m_parent;
    public void SetComps(StatInfoUIScript _parent, EStatName _stat) { m_parent = _parent; m_stat = _stat; SetComps(); }

    private EventTrigger m_trigger;

    private EStatName m_stat;

    private Color IdleColor = new Color(241f, 241f, 241f);


    private void ChangeStat(PointerEventData _data)
    {
        if (_data.button == PointerEventData.InputButton.Left)
        {
            m_parent.ChkNUpgradeStat(m_stat, true);
        }
        else if (_data.button == PointerEventData.InputButton.Right)
        {
            m_parent.ChkNUpgradeStat(m_stat, false);
        }
    }



    private void SetEvents()
    {
        FunctionDefine.AddEvent(m_trigger, EventTriggerType.PointerClick, ChangeStat);
    }

    private void SetComps()
    {
        m_trigger = gameObject.AddComponent<EventTrigger>();
        SetEvents();
    }
}
