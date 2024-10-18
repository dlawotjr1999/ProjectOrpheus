using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDieState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;
    public EMonsterState StateEnum { get { return EMonsterState.DIE; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if (m_monster == null) { m_monster = _monster; }

        m_monster.StartDie();           // 사망 상태 시작
        m_monster.DeathResult();        // 사망 원인에 따른 결과물
    }

    public void Proceed()
    {

    }
}
