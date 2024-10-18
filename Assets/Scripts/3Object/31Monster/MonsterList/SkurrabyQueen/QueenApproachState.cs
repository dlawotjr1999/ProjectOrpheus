using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenApproachState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum => EMonsterState.ATTACK;

    private QueenScript Queen { get { return (QueenScript)m_monster; } }

    private const float QueenAttackDelay = 3;
    private float AttackCount { get; set; }
    private float MissCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Queen == null) { m_monster = _monster; }

        Queen.StartApproach();
        AttackCount = QueenAttackDelay;
    }

    private void ControlDistance()
    {
        if (Queen.TargetDistance > Queen.ReturnRange)
        {
            Queen.ApproachTarget();
        }
        else if (Queen.TargetDistance < Queen.EvadeRange)
        {
            Queen.EvadeQueen();
        }
        else Queen.LookTarget();
    }

    public void Proceed()
    {
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
        if (Queen.CanUseSkill)
        {
            m_monster.ChangeState(EMonsterState.ATTACK);
            return;
        }

        ControlDistance();
    }
}
