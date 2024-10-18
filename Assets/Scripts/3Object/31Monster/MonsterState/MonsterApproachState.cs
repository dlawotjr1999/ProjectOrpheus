using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class MonsterApproachState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.APPROACH; } }

    private float DelayCount { get; set; }
    private float MissCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        m_monster.StartApproach();

        DelayCount = m_monster.ApproachDelay;
        MissCount = m_monster.MissTargetDelay;
    }

    public void Proceed()
    {
        if(DelayCount > 0)
        {
            DelayCount -= Time.deltaTime;
            m_monster.StopMove();
            m_monster.LookTarget(); return;
        }

        m_monster.ApplyMoveSpeed();

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
        if (m_monster.CanSkill)
        {
            m_monster.ChangeState(EMonsterState.SKILL);
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