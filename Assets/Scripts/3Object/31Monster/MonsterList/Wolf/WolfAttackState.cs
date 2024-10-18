using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum { get { return EMonsterState.ATTACK; } }

    private WolfScript Wolf { get { return (WolfScript)m_monster; } }

    public void ChangeTo(MonsterScript _monster)
    {
        if(Wolf == null) { m_monster = _monster; }

        Wolf.StartAttack();
    }

    public void Proceed()
    {
        Wolf.LookTarget();
    }
}
