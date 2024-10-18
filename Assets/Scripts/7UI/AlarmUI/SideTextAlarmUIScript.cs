using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SideTextAlarmUIScript : TimeLimitUIScript      // 텍스트 알람 UI
{
    private RectTransform m_rect;
    private TextMeshProUGUI m_alarmTxt;

    private const float HeightGap = 57.5f;                  // 이동 높이 (간격)
    private const float MoveUpTime = 0.2f;                  // 이동 시간

    public void SetAlarmTxt(string _alarm) { m_alarmTxt.text = _alarm; }        // 알람 텍스트 설정

    private Vector2 MoveTarget { get; set; }                // 이동 목표 지점
    private IEnumerator MovingCoroutine { get; set; }       // 이동 코루틴

    public void MoveUp(List<SideTextAlarmUIScript> _list)   // 위로 이동 시작
    {
        if(MovingCoroutine != null) { MoveTarget += Vector2.up * HeightGap; return; }
        MovingCoroutine = MovingProcess(_list);
        StartCoroutine(MovingCoroutine);
    }
    private IEnumerator MovingProcess(List<SideTextAlarmUIScript> _list)    // 이동 과정
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
