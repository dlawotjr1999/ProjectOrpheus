using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterHitState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.HIT; } }

    private float DelayCount { get; set; }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        m_monster.StartHit();
        DelayCount = m_monster.StunDelay;
    }

    public void Proceed()
    {
        DelayCount -= Time.deltaTime;

        if (DelayCount < 0 && !m_monster.IsDead)
        {
            if (m_monster.HasTarget)
                m_monster.ChangeState(EMonsterState.APPROACH);
            else
                m_monster.ChangeState(EMonsterState.IDLE);
        }
    }
}