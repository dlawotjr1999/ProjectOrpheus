using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillState : MonoBehaviour, IMonsterState
{
    private MonsterScript m_monster;

    public EMonsterState StateEnum => EMonsterState.SKILL;

    public void ChangeTo(MonsterScript _monster)
    {
        if(m_monster == null) { m_monster = _monster; }

        m_monster.StartSkill();
    }

    public void Proceed()
    {

    }
}
