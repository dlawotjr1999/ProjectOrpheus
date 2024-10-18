using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : AnimatedAttackMonster
{
    private bool IsFightingPlayer { get; set; }

    public override void SetMaxHPMultiplier(float _multiplier) { }

    public override void StartIdle()
    {
        base.StartIdle();
        CheckNEndPlayerBattle();
    }
    public override void StartApproach()
    {
        base.StartApproach();
        CheckNStartPlayerCombat();
    }
    public override bool GetHit(HitData _hit)
    {
        bool isHit = base.GetHit(_hit);
        CheckNStartPlayerCombat();
        return isHit;
    }

    public void CheckNStartPlayerCombat()
    {
        if (IsFightingPlayer || !PlayManager.CheckIsPlayer(CurTarget)) { return; }
        PlayManager.ShowBossHPBar(this);
        IsFightingPlayer = true;
        GameManager.ChangeBGM(EBGM.BOSS_BGM);
    }

    public override void ApplyHPUI()
    {
        PlayManager.SetBossHP(CurHP);
    }
    public override void HideHPUI()
    {
        PlayManager.HideBossHPBar();
    }

    public void CheckNEndPlayerBattle()
    {
        if (!IsFightingPlayer) { return; }
        HideHPUI();
        GameManager.ChangeBGM(EBGM.FIELD_BGM);
    }


    public override void OnSpawned()
    {
        base.OnSpawned();
        IsFightingPlayer = false;
    }
    public override void SetUI() { }
}
