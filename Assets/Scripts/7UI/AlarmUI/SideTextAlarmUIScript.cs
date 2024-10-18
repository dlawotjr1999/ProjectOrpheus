using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SideTextAlarmUIScript : TimeLimitUIScript      // �ؽ�Ʈ �˶� UI
{
    private RectTransform m_rect;
    private TextMeshProUGUI m_alarmTxt;

    private const float HeightGap = 57.5f;                  // �̵� ���� (����)
    private const float MoveUpTime = 0.2f;                  // �̵� �ð�

    public void SetAlarmTxt(string _alarm) { m_alarmTxt.text = _alarm; }        // �˶� �ؽ�Ʈ ����

    private Vector2 MoveTarget { get; set; }                // �̵� ��ǥ ����
    private IEnumerator MovingCoroutine { get; set; }       // �̵� �ڷ�ƾ

    public void MoveUp(List<SideTextAlarmUIScript> _list)   // ���� �̵� ����
    {
        if(MovingCoroutine != null) { MoveTarget += Vector2.up * HeightGap; return; }
        MovingCoroutine = MovingProcess(_list);
        StartCoroutine(MovingCoroutine);
    }
    private IEnumerator MovingProcess(List<SideTextAlarmUIScript> _list)    // �̵� ����
    {
        MoveTarget = (Vector2)m_rect.position + Vector2.up*HeightGap;
        float move = HeightGap / MoveUpTime;
        while (m_rect.position.y < MoveTarget.y)
        {
            m_rect.position += move*Time.deltaTime*Vector3.up;
            yield return null;
        }
        m_rect.position = MoveTarget;
        MovingCoroutine = null;
        _list.Remove(this);
    }

    public override void SetComps()
    {
        base.SetComps();
        m_rect = GetComponent<RectTransform>();
        m_alarmTxt = GetComponentInChildren<TextMeshProUGUI>();
    }
}
