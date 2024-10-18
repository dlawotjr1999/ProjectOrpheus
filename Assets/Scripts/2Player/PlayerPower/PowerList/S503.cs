using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class S503 : SummonPowerScript
{
    public bool IsBuffOn { get; private set; }

    private float BuffTimeCount { get; set; }
    private bool IsBuffed { get { return BuffTimeCount > 0; } }

    [SerializeField]
    private float m_effectRadius = 8;           // 영향 범위
    [SerializeField]
    private float m_buffLastTime = 5;           // 버프 개별 지속 시간
    [SerializeField]
    private float m_buffResetTime = 1;          // 버프 시간 리셋 간격

    [SerializeField]
    private EAdjType m_adjType;                 // 종류
    [SerializeField]
    private float m_adjAmount = 1.2f;           // 배율임
    //     private float m_powerLastTime = 10;         // 권능 지속 시간 << 필요없다고 생각하여 삭제함

    private bool IsPlayerIn { get; set; }

    public override void PowerSummoned()
    {
        IsBuffOn = true;
    }
    public void PowerDone()
    {
        IsBuffOn = false;
    }

    private void CheckNBuff()
    {
        if (!IsBuffOn || IsBuffed) { return; }
        Collider[] cols = Physics.OverlapSphere(transform.position, m_effectRadius, ValueDefine.HITTABLE_PLAYER_LAYER);
        foreach (Collider col in cols)
        {
            PlayerController player = col.GetComponentInParent<PlayerController>();
            if(player == null || player.IsDead) { return; }
            if (player.CheckAdjusted(this)) { player.ModifyAdjust(this, m_buffLastTime); }
            else { player.GetAdjust(new(m_adjType, m_adjAmount, m_buffLastTime), this); }
            BuffTimeCount = m_buffResetTime;
            IsPlayerIn = true;
            return;
        }
        if (IsPlayerIn)
        {
            PlayerExit();
            IsPlayerIn = false;
        }
    }
    private void PlayerExit()
    {
        PlayManager.ModifyPlayerAdjust(this, m_buffLastTime);
    }


    public override void PowerCreated()
    {
        base.PowerCreated();
        IsBuffOn = false;
        BuffTimeCount = 0;
        IsPlayerIn = false;
    }

    private void Update()
    {
        if(IsBuffed) { BuffTimeCount -= Time.deltaTime; }
    }

    private void FixedUpdate()
    {
        CheckNBuff();
    }
}
