using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAttackState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.ATTACK; } }


    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }
        m_monster.StartAttack();
    }

    public void Proceed()
    {
        if (!m_monster.HasTarget)
        {
            m_monster.ChangeState(EMonsterState.IDLE);
            return;
        }

        m_monster.LookTarget();
    }
}
