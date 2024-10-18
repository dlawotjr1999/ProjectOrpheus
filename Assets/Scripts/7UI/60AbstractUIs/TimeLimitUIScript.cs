using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitUIScript : FadeUIScript                   // ���� �ð� �� ������� UI
{
    [SerializeField]
    private float m_destroyLimitTime = 1;                   // ������� �ð�
    private float DestroyTimeCount { get; set; }            // �ð� ī��Ʈ


    public void SetLimitTime(float _time)           // �ִ� �ð� ����
    {
        m_destroyLimitTime = _time;
        ResetTimeCount();
    }
    public void ResetTimeCount()                    // �ð� ī��Ʈ �缳��
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
