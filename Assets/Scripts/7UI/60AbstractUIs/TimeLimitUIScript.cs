using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitUIScript : FadeUIScript                   // 일정 시간 후 사라지는 UI
{
    [SerializeField]
    private float m_destroyLimitTime = 1;                   // 사라지는 시간
    private float DestroyTimeCount { get; set; }            // 시간 카운트


    public void SetLimitTime(float _time)           // 최대 시간 설정
    {
        m_destroyLimitTime = _time;
        ResetTimeCount();
    }
    public void ResetTimeCount()                    // 시간 카운트 재설정
    {
        DestroyTimeCount = m_destroyLimitTime;
    }

    public override void FadeOutDone()
    {
        Destroy(gameObject);
    }


    public override void Start()
    {
        base.Start();
        ResetTimeCount();
    }
    private void Update()
    {
        if(!m_ableFade || !Fading)
            DestroyTimeCount -= Time.deltaTime;
        if(DestroyTimeCount <= 0)
        {
            DestroyUI();
        }
    }
}
