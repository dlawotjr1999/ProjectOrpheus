using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmmApproachScript : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum => EMonsterState.APPROACH;

    private HmmScript Hmm { get { return (HmmScript)m_monster; } }

    private float DelayCount { get; set; }
    private float MissCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (Hmm == null) { m_monster = _monster; }
        Hmm.StartApproach();

        DelayCount = m_monster.ApproachDelay;
        MissCount = m_monster.MissTargetDelay;
    }

    public void Proceed()
    {
        if (DelayCount > 0) 
        {
            DelayCount -= Time.deltaTime;
            Hmm.RotateToAngle(Hmm.Direction + Random.Range(-10,10), ObjectScript.ERotateSpeed.DEFAULT);
            return;
        }

        bool targetCheck = m_monster.CheckTarget();
        if (!targetCheck)
        {
            MissCount -= Time.deltaTime;
            if (MissCount <= 0) { m_monster.MissTarget(); }
        }
        else { MissCount = m_monster.MissTargetDelay; }

        if (!m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.IDLE);
            return;
        }
        if (m_monster.CanAttack)
        {
            m_monster.ChangeState(EMonsterState.ATTACK);
            return;
        }

        m_monster.ApproachTarget();
    }
}
