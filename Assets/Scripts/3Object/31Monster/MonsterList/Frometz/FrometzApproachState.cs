using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrometzApproachState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.APPROACH; } }

    private FrometzScript Frometz { get { return (FrometzScript)m_monster; } }

    private float DelayCount { get; set; }
    private float MissCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }

        Frometz.StartApproach();

        DelayCount = Frometz.ApproachDelay;
        MissCount = Frometz.MissTargetDelay;
    }

    public void Proceed()
    {
        if (DelayCount > 0) { DelayCount -= Time.deltaTime; Frometz.LookTarget(); return; }

        bool targetCheck = Frometz.CheckTarget();
        if (!targetCheck)
        {
            MissCount -= Time.deltaTime;
            if (MissCount <= 0) { Frometz.MissTarget(); }
        }
        else { MissCount = Frometz.MissTargetDelay; }

        if (!Frometz.HasTarget)
        {
            Frometz.ChangeState(EMonsterState.IDLE);
            return;
        }
        if (Frometz.CanAttack)
        {
            Frometz.ChangeState(EMonsterState.ATTACK);
            return;
        }
    }
}
